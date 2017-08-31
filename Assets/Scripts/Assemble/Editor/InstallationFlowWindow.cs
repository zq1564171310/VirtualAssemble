/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble
{
    using UnityEditor;
    using UnityEngine;
    using WyzLink.Utils;

    public class InstallationFlowWindow<T> : EditorWindow where T: Parts.IFlowNode
    {
        public INodeLoader target;

        private WindowManager<T> windowManager;

        public enum UIState
        {
            normalState,
            connectingState,
        };
        public UIState uiState = UIState.normalState;

        private WindowItem<T> dragStartWindow;
        private WindowItem<T> dragEndWindow;
        private Vector2 draggingPos;

        //
        // Display options
        //
        private bool hideLinkedObjects = false;
        private bool hideUnlinkedObjects = false;
        private string searchText = "";

        private int PanelMarginTop = 20;
        private int PanelMarginLeft = 0;

        private Vector2 scrollPosition;
        private Vector2 panelSize = new Vector2(3000, 1000);

        private const float autoScrollingMargin = 40.0f;

        private void OnEnable()
        {
            if (windowManager == null)
            {
                windowManager = new WindowManager<T>();
            }
            if (target != null)
            {
                windowManager.LoadWindows(target, (r) => this.panelSize = r.size);
            }
        }

        private void OnDisable()
        {
            if (windowManager != null)
            {
                windowManager.SetObjectVisibilities(true, true);
            }
        }

        void OnGUI()
        {
            UpdateTopToolbar();

            scrollPosition = GUI.BeginScrollView(new Rect(PanelMarginLeft, PanelMarginTop, position.width - PanelMarginLeft, position.height - PanelMarginTop), scrollPosition, new Rect(Vector2.zero, panelSize));
            BeginWindows();
            if (windowManager != null)
            {
                windowManager.UpdateWindows();
                windowManager.UpdateWindowsLate();
                UpdateConnecting();
                UpdateContextMenu();
            }
            EndWindows();
            GUI.EndScrollView();
        }

        private void UpdateContextMenu()
        {
            switch (Event.current.type)
            {
                case EventType.ContextClick:
                    var window = this.windowManager.GetCurrentWindow(Event.current.mousePosition, true);
                    if (window != null)
                    {
                        var menu = window.CreateMenu(this.windowManager);
                        if (menu.GetItemCount() > 0)
                        {
                            menu.ShowAsContext();
                        }
                    }
                    break;
            }
        }

        private void UpdateTopToolbar()
        {
            GUILayout.BeginHorizontal();
            if (this.target == null)
            {
                GUI.enabled = false;
            }
            ToolbarContent();

            GUI.enabled = true;

            GUILayout.EndHorizontal();

            // Error message
            if (this.target == null)
            {
                var style = new GUIStyle(GUI.skin.label);
                style.normal.textColor = Color.red;
                GUILayout.Label("请关闭该窗口并从安装控制器入口重新打开。", style);
            }
        }

        private void ToolbarContent()
        {
            if (GUILayout.Button("扫描零件树", GUILayout.Width(100)))
            {
                if (this.target != null)
                {
                    if (windowManager == null)
                    {
                        windowManager = new WindowManager<T>();
                    }
                    windowManager.LoadWindows(target, (r) => this.panelSize = r.size);
                }
                else
                {
                    Debug.Log("Lost original game object. Please close the window and restart again");
                }
            }

            if (GUILayout.Button("视图刷新", GUILayout.Width(100)))
            {
                windowManager.RefreshLayout((r) => this.panelSize = r.size);
            }

            if (GUILayout.Button("保存", GUILayout.Width(100)))
            {
                windowManager.SaveToAsset();
            }

            GUILayout.FlexibleSpace();

            searchText = GUILayout.TextField(searchText, GUILayout.Width(100));
            if (GUILayout.Button("搜索", GUILayout.Width(80)) && !string.IsNullOrEmpty(searchText))
            {
                var rect = this.windowManager.FindNextMatch(searchText);
                if (rect.width > 0)
                {
                    ScrollToInclude(rect);
                }
            }

            GUILayout.FlexibleSpace();

            hideLinkedObjects = GUILayout.Toggle(hideLinkedObjects, "隐藏流程对象", GUILayout.Width(100));
            hideUnlinkedObjects = GUILayout.Toggle(hideUnlinkedObjects, "隐藏非流程对象", GUILayout.Width(100));
            windowManager.SetObjectVisibilities(!hideLinkedObjects, !hideUnlinkedObjects);
        }

        private void ScrollToInclude(Rect rect)
        {
            var viewPort = this.GetViewPortSize();
            rect = rect.Extend(12, 12);
            this.scrollPosition.x += Mathf.Max(0, rect.xMax - (this.scrollPosition.x + viewPort.x));
            this.scrollPosition.x += Mathf.Min(0, rect.xMin - this.scrollPosition.x);
            this.scrollPosition.y += Mathf.Max(0, rect.yMax - (this.scrollPosition.y + viewPort.y));
            this.scrollPosition.y += Mathf.Min(0, rect.yMin - this.scrollPosition.y);
        }

        private void UpdateConnecting()
        {
            switch (Event.current.type)
            {
                case EventType.mouseDown:
                    if (Event.current.isMouse && Event.current.clickCount == 1)
                    {
                        this.dragStartWindow = this.windowManager.GetCurrentWindow(Event.current.mousePosition, true);
                        if (this.dragStartWindow != null)
                        {
                            this.uiState = UIState.connectingState;
                            this.draggingPos = Event.current.mousePosition;
                        }
                    }
                    else if (Event.current.isMouse && Event.current.clickCount == 2)
                    {
                        var window = this.windowManager.GetCurrentWindow(Event.current.mousePosition, true);
                        if (window != null)
                        {
                            window.SelectAndFrameObject();
                        }
                    }
                    break;
                case EventType.mouseDrag:
                    this.draggingPos = Event.current.mousePosition;
                    this.dragEndWindow = this.windowManager.GetCurrentWindow(Event.current.mousePosition, false);
                    this.scrollPosition += GetAdjustmentOffset(this.draggingPos);
                    Repaint();
                    break;
                case EventType.mouseUp:
                    if (this.dragEndWindow != null && this.dragStartWindow != null && this.dragEndWindow != this.dragStartWindow)
                    {
                        // Connect the two nodes
                        this.dragEndWindow.AddPreviousStep(this.dragStartWindow);
                        this.windowManager.SetDirty();
                    }
                    this.uiState = UIState.normalState;
                    this.dragStartWindow = null;
                    this.dragEndWindow = null;
                    Repaint();
                    break;
                case EventType.repaint:
                    if (this.uiState == UIState.connectingState)
                    {
                        if (this.dragEndWindow != null && this.dragEndWindow != this.dragStartWindow)
                        {
                            WindowItem<T>.curveFromToBg(this.dragStartWindow, this.dragEndWindow);
                            WindowItem<T>.curveFromTo(this.dragStartWindow, this.dragEndWindow);
                        }
                        else
                        {
                            WindowItem<T>.curveFromTo(this.dragStartWindow, this.draggingPos);
                        }
                    }
                    break;
            }
        }

        private Vector2 GetAdjustmentOffset(Vector2 mousePos)
        {
            var relativeMousePos = mousePos - this.scrollPosition;
            Vector2 offset = Vector2.zero;
            Vector2 viewPortSize = GetViewPortSize();
            if (relativeMousePos.x < autoScrollingMargin && relativeMousePos.x > 0)
            {
                offset += Vector2.left * (autoScrollingMargin - relativeMousePos.x);
            }
            if (relativeMousePos.y < autoScrollingMargin && relativeMousePos.y > 0)
            {
                offset += Vector2.down * (autoScrollingMargin - relativeMousePos.y);
            }
            if (viewPortSize.x - relativeMousePos.x < autoScrollingMargin && viewPortSize.x - relativeMousePos.x > 0)
            {
                offset += Vector2.right * (autoScrollingMargin - relativeMousePos.x + viewPortSize.x);
            }
            if (viewPortSize.y - relativeMousePos.y < autoScrollingMargin && viewPortSize.y - relativeMousePos.y> 0)
            {
                offset += Vector2.up * (autoScrollingMargin - relativeMousePos.y + viewPortSize.y);
            }
            return offset;
        }

        private Vector2 GetViewPortSize()
        {
            // TODO: Will consider the scroll bar size if we have extra time
            return new Vector2(position.width - PanelMarginLeft, position.height - PanelMarginTop);
        }

        public void LoadContent(INodeLoader myController)
        {
            this.target = myController;
            this.windowManager = new WindowManager<T>();
            windowManager.LoadWindows(myController, (r) => this.panelSize = r.size);
        }

        private void OnHierarchyChange()
        {
            // Update the content from Hierarchy
            Debug.Log("Update from Hierarchy notification");

            if (target != null)
            {
                if (windowManager.UpdateFromHierarchy(target))
                {
                    windowManager.RefreshLayout((r) => this.panelSize = r.size);
                    Repaint();
                }
            }
        }

        private void OnProjectChange()
        {
            Debug.Log("Update from Project notification");
            if (target != null)
            {
                //windowManager.LoadWindows(target);
            }
        }
    }
}
