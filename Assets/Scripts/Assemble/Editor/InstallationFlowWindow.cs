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

    public class InstallationFlowWindow : EditorWindow
    {
        public AssembleController target;

        private WindowManager windowManager;

        private Vector2 scrollPosition;

        public enum UIState
        {
            normalState,
            connectingState,
        };
        public UIState uiState = UIState.normalState;

        private WindowItem dragStartWindow;
        private WindowItem dragEndWindow;
        private Vector2 draggingPos;

        private int PanelMarginTop = 20;

        private Vector2 panelSize = new Vector2(3000, 1000);

        void OnGUI()
        {
            UpdateTopToolbar();

            scrollPosition = GUI.BeginScrollView(new Rect(0, PanelMarginTop, position.width, position.height - PanelMarginTop), scrollPosition, new Rect(Vector2.zero, panelSize));
            BeginWindows();
            if (windowManager != null)
            {
                int count = 0;
                foreach (var w in windowManager.GetWindowList())
                {
                    w.Update();
                    count++;
                }
                UpdateConnecting();
                UpdateContextMenu();
                //Debug.Log("Item update count:" + count);
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
                        var menu = window.CreateMenu();
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
            if (GUILayout.Button("扫描零件树", GUILayout.Width(100)))
            {
                if (this.target != null)
                {
                    if (windowManager == null)
                    {
                        windowManager = new WindowManager();
                    }
                    this.panelSize = windowManager.LoadWindows(target).size;
                }
                else
                {
                    Debug.Log("Lost original game object. Please close the window and restart again");
                }
            }

            if (GUILayout.Button("视图刷新", GUILayout.Width(100)))
            {
                this.panelSize = windowManager.RefreshLayout().size;
            }

            if (GUILayout.Button("保存", GUILayout.Width(100)))
            {
                windowManager.SaveToAsset();
            }
            GUILayout.EndHorizontal();
        }

        private void UpdateConnecting()
        {
            switch (Event.current.type)
            {
                case EventType.mouseDown:
                    this.dragStartWindow = this.windowManager.GetCurrentWindow(Event.current.mousePosition, true);
                    if (this.dragStartWindow != null)
                    {
                        this.uiState = UIState.connectingState;
                        this.draggingPos = Event.current.mousePosition;
                    }
                    break;
                case EventType.mouseDrag:
                    this.draggingPos = Event.current.mousePosition;
                    this.dragEndWindow = this.windowManager.GetCurrentWindow(Event.current.mousePosition, false);
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
                            WindowItem.curveFromTo(this.dragStartWindow, this.dragEndWindow);
                        }
                        else
                        {
                            WindowItem.curveFromTo(this.dragStartWindow, this.draggingPos);
                        }
                    }
                    break;
            }
        }

        public void LoadContent(AssembleController myController)
        {
            this.target = myController;
            this.windowManager = new WindowManager();
            windowManager.LoadWindows(myController);
        }

        private void OnHierarchyChange()
        {
            // Update the content from Hierarchy
            Debug.Log("Update from Hierarchy notification");

            // TODO: Optimization: We should check the performance on it, and only to add/remove when needed
            if (target != null)
            {
                this.panelSize = windowManager.LoadWindows(target).size;
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
