/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEngine;
    using WyzLink.Parts;

    public class InstallationFlowWindow : EditorWindow
    {
        public AssembleController target;

        IList<WindowItem> windowList;

        Vector2 scrollPosition;

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

        private string defaultFileName = "AssembleFlow.txt";

        void OnGUI()
        {
            UpdateTopToolbar();

            scrollPosition = GUI.BeginScrollView(new Rect(0, PanelMarginTop, position.width, position.height - PanelMarginTop), scrollPosition, new Rect(0, 0, 3000, 1000));
            BeginWindows();
            if (windowList != null)
            {
                int count = 0;
                foreach (var w in windowList)
                {
                    w.Update();
                    count++;
                }
                //Debug.Log("Item update count:" + count);
            }
            UpdateConnecting();
            UpdateContextMenu();

            EndWindows();
            GUI.EndScrollView();
        }

        private void UpdateContextMenu()
        {
            switch (Event.current.type)
            {
                case EventType.ContextClick:
                    var window = GetCurrentWindow(Event.current.mousePosition, true);
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
                    windowList = LoadAllNodes().ToList();
                }
                else
                {
                    Debug.Log("Lost original game object. Please close the window and restart again");
                }
            }

            if (GUILayout.Button("视图刷新", GUILayout.Width(100)))
            {
                if (windowList != null && windowList.Count() > 1)
                {
                    LayoutWindowItems(windowList[0]);
                }
            }

            if (GUILayout.Button("保存", GUILayout.Width(100)))
            {
                if (windowList != null)
                {
                    SaveToAsset(defaultFileName);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void LayoutWindowItems(WindowItem firstWindow)
        {
            Vector2 point = Vector2.one * 20;
            firstWindow.CalculateChildrenHeight();

            var list = new List<WindowItem>();
            list.Add(firstWindow);
            LayoutWindowLayer(list, point);
        }

        private void LayoutWindowLayer(List<WindowItem> list, Vector2 point)
        {
            if (list.Count() == 0)
            {
                return;
            }

            List<WindowItem> nextList = new List<WindowItem>();
            int maxWidth = 0;
            foreach (var item in list)
            {
                item.MoveTo(point);
                nextList.AddRange(item.GetNextSteps());
                point.y += item.GetChildrenHeight();
                maxWidth = Math.Max(maxWidth, item.GetWindowWidth());
            }
            LayoutWindowLayer(nextList, NextRow(point, maxWidth));
        }

        private Vector2 NextRow(Vector2 point, int maxWidth)
        {
            return new Vector2(point.x + maxWidth + WindowItem.LayoutGapHorizontally, 20);
        }

        private void UpdateConnecting()
        {
            switch (Event.current.type)
            {
                case EventType.mouseDown:
                    this.dragStartWindow = GetCurrentWindow(Event.current.mousePosition, true);
                    if (this.dragStartWindow != null)
                    {
                        this.uiState = UIState.connectingState;
                        this.draggingPos = Event.current.mousePosition;
                    }
                    break;
                case EventType.mouseDrag:
                    this.draggingPos = Event.current.mousePosition;
                    this.dragEndWindow = GetCurrentWindow(Event.current.mousePosition, false);
                    Repaint();
                    break;
                case EventType.mouseUp:
                    if (this.dragEndWindow != null && this.dragStartWindow != null)
                    {
                        // Connect the two nodes
                        this.dragEndWindow.AddPreviousStep(this.dragStartWindow);
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

        private WindowItem GetCurrentWindow(Vector2 mousePosition, bool connectionAreaOnly)
        {
            var position = mousePosition;
            WindowItem windowHit = null;
            if (windowList != null)
            {
                foreach (var w in windowList)
                {
                    if (w.HitTest(position, connectionAreaOnly))
                    {
                        windowHit = w;
                        break;
                    }
                }
            }
            return windowHit;
        }

        internal void LoadContent(AssembleController myController)
        {
            this.target = myController;
            LoadAssembleFlowFromFile(defaultFileName);
        }

        private void LoadAssembleFlowFromFile(string fileName)
        {
            string text = null;
            try
            {
                text = File.ReadAllText(Application.dataPath + "/Resources/" + fileName);
            }
            catch (DirectoryNotFoundException)
            {
                // Leave the text as null
            }
            catch (FileNotFoundException)
            {
                // Leave the text as null
            }
            if (text == null)
            {
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                File.WriteAllText(Application.dataPath + "/Resources/" + fileName, "# assemble flow created\n");
                AssetDatabase.Refresh();
                text = "";
            }

            Debug.Log("The target is " + target);
            if (target != null)
            {
                this.windowList = ParseAssembleFlow(text, target.GetAllNodes<Node>());
                //Repaint();
            }
        }

        private IList<WindowItem> ParseAssembleFlow(string text, IEnumerable<Node> nodes)
        {
            Debug.Log("Parsing: " + text);
            return LoadAllNodes().ToList();
        }

        private void SaveToAsset(string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# File updated");
            foreach (var w in windowList)
            {
                foreach (var next in w.GetNextSteps())
                {
                    sb.Append(w.GetNodeId()).Append("->").Append(next.GetNodeId()).AppendLine();
                }
            }
            Debug.Log("Saved file in");
            File.WriteAllText(Application.dataPath + "/Resources/" + fileName, sb.ToString());
        }

        private IEnumerable<WindowItem> LoadAllNodes()
        {
            if (target == null)
            {
                return Enumerable.Empty<WindowItem>();
            }

            int index = 0;
            Vector2 position = new Vector2(20, 20);
            WindowItem previousWindow = null;

            var nodeList = target.GetAllNodes<Node>();
            return nodeList.Select((Node node) => {
                var window = new WindowItem(index, position, node);
                index++;
                position.x += 90;
                //window.AddPreviousStep(previousWindow);
                previousWindow = window;
                Debug.Log("Idx:" + index);
                return window;
            });
        }

        private void OnHierarchyChange()
        {
            // Update the content from Hierarchy
            Debug.Log("Update from Hierarchy notification");

            // TODO: Optimization: We should check the performance on it, and only to add/remove when needed
            if (target != null)
            {
                this.windowList = LoadAllNodes().ToList();
            }
        }
    }
}
