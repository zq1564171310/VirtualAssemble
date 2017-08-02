/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// Manages the list of the windows on the screen
/// </summary>

namespace WyzLink.Assemble
{
    using Parts;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using System.Text;
    using System.IO;
    using UnityEditor;
    using Utils;

    public class WindowManager
    {
        private IDictionary<int, WindowItem> windowList = new Dictionary<int, WindowItem>();
        private IList<WindowItem> headers;
        private IList<WindowItem> standalones;
        private const string defaultFileName = "AssembleFlow.txt";      // TODO: Will change this to a new file format later

        private bool isDirty = true;
        private bool doLayout = false;
        private int layoutToken = 0;

        private bool displayLinkedObjects = true;     // Display objects by default
        private bool displayUnlinkedObjects = true;     // Display objects by default

        private Action<Rect> layoutCallback = null;

        public WindowManager()
        {
            this.windowList = new Dictionary<int, WindowItem>();
        }

        public int Count()
        {
            return windowList.Count;
        }

        public void LoadWindows(AssembleController assembleController, Action<Rect> layoutCallback)
        {
            if (assembleController == null)
            {
                layoutCallback(Rect.zero);
            }

            var needRefresh = UpdateFromHierarchy(assembleController);
            if (needRefresh)
            {
                var flowString = LoadAssembleFlowFromFile(defaultFileName);
                ApplyAssembleFlow(this.windowList, flowString);
                PrepareList(windowList);
                this.isDirty = false;
            }
            LayoutWindows(layoutCallback);
        }

        internal void UpdateWindows()
        {
            if (doLayout)
            {
                Debug.Log("Layout windows");
                LayoutWindowsOnGUI(layoutCallback);
                layoutCallback = null;
                doLayout = false;
            }

            foreach (var window in windowList.Values)
            {
                window.Update();
            }
        }

        public void RefreshLayout(Action<Rect> layoutCallback)
        {
            if (isDirty)
            {
                PrepareList(windowList);
                this.isDirty = false;
            }
            LayoutWindows(layoutCallback);
        }

        private void ApplyAssembleFlow(IDictionary<int, WindowItem> windowList, string flowString)
        {
            AssembleFlowParser.ParseAssembleFlowFile(flowString, (int step0, int step1) =>
            {
                WindowItem w0;
                WindowItem w1;
                if (windowList.TryGetValue(step0, out w0) && windowList.TryGetValue(step1, out w1))
                {
                    w1.AddPreviousStep(w0);
                }
                else
                {
                    Debug.LogError("Failed to create flow nodes of " + step0 + "->" + step1);
                }
            });
        }

        private void PrepareList(IDictionary<int, WindowItem> windowList)
        {
            this.headers = new List<WindowItem>();
            this.standalones = new List<WindowItem>();
            foreach (var window in windowList.Values)
            {
                if (!window.HasPreviousSteps())
                {
                    if (window.HasNextSteps())
                    {
                        this.headers.Add(window);
                    }
                    else
                    {
                        this.standalones.Add(window);
                    }
                }
            }
        }

        private string LoadAssembleFlowFromFile(string fileName)
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
            return text;
        }

        private void LayoutWindows(Action<Rect> layoutCallback)
        {
            // All the layout work will need to be done inside OnGUI() function.
            // This function will trigger it
            this.layoutCallback = layoutCallback;
            this.doLayout = true;
        }

        // This function could only be called inside OnGUI
        private void LayoutWindowsOnGUI(Action<Rect> layoutCallback)
        {
            var startPoint = Vector2.one * WindowItem.LayoutGapVerticallyGroup;
            this.layoutToken++;     // Start a new layout
            var panelSize = Rect.zero;
            if (this.headers != null)
            {
                foreach (var window in this.headers)
                {
                    var rect = LayoutFlow(window, startPoint);
                    startPoint.y = rect.yMax + WindowItem.LayoutGapVerticallyGroup;
                    panelSize = panelSize.Union(rect);
                }
            }
            if (this.standalones != null)
            {
                foreach (var window in this.standalones)
                {
                    // Layout individual items
                    window.LayoutAt(startPoint);
                    startPoint.x += window.GetWindowWidth() + WindowItem.WindowMarginLeft;
                    panelSize = panelSize.Union(window.GetWindowRect());
                }
            }
            panelSize.height += 20; // Buttom margin
            if (layoutCallback != null)
            {
                layoutCallback(panelSize);
            }
            Debug.Log("Finished layout with panel size: " + panelSize);
        }

        private Rect LayoutFlow(WindowItem startWindow, Vector2 startPoint)
        {
            var list = new List<WindowItem>() { startWindow };
            return LayoutWindowLayer(list, startPoint, 0);
        }

        private Rect LayoutWindowLayer(IList<WindowItem> list, Vector2 point, int layer)
        {
            if (layer > 1000)
            {
                Debug.LogError("There is a loop in the graph. Can't finish it.");
                return new Rect(point, Vector2.zero);
            }
            if (list.Count == 0)
            {
                return new Rect(point, Vector2.zero);
            }

            Rect rect = new Rect(point, Vector2.zero);
            foreach (var item in list)
            {
                if (item.LayoutToken != this.layoutToken || item.Layer < layer)
                {
                    item.LayoutAt(point);
                    rect = rect.Union(item.GetWindowRect());
                    var childRect = LayoutWindowLayer(item.GetNextSteps(), point + new Vector2(item.GetWindowWidth() + WindowItem.LayoutGapHorizontally, 0), layer + 1);
                    point.y += Mathf.Max(childRect.height, item.GetWindowRect().height) + WindowItem.LayoutGapVertically;
                    rect = rect.Union(childRect);

                    // Mark the item
                    item.LayoutToken = this.layoutToken;
                    item.Layer = layer;
                }
            }
            return rect;
        }
        
        public WindowItem GetCurrentWindow(Vector2 mousePosition, bool connectionAreaOnly)
        {
            var position = mousePosition;
            WindowItem windowHit = null;
            foreach (var w in this.windowList.Values)
            {
                if (w.HitTest(position, connectionAreaOnly))
                {
                    windowHit = w;
                    break;
                }
            }
            return windowHit;
        }

        public void SaveToAsset()
        {
            SaveToAsset(defaultFileName);
        }

        private void SaveToAsset(string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# File updated");
            foreach (var w in this.windowList.Values)
            {
                foreach (var next in w.GetNextSteps())
                {
                    sb.Append(w.GetNodeId()).Append("->").Append(next.GetNodeId()).AppendLine();
                }
            }
            Debug.Log("Saved file in");
            File.WriteAllText(Application.dataPath + "/Resources/" + fileName, sb.ToString());
        }

        public void SetDirty()
        {
            this.isDirty = true;
            UpdateVisibility();
        }

        public void SetObjectVisibilities(bool displayLinkedObjects, bool displayUnlinkedObjects)
        {
            bool needsUpdate = false;
            if (displayLinkedObjects != this.displayLinkedObjects)
            {
                this.displayLinkedObjects = displayLinkedObjects;
                needsUpdate = true;
            }
            if (displayUnlinkedObjects != this.displayUnlinkedObjects)
            {
                this.displayUnlinkedObjects = displayUnlinkedObjects;
                needsUpdate = true;
            }
            if (needsUpdate)
            {
                UpdateVisibility();
            }
        }

        internal void UpdateVisibility()
        {
            foreach (var window in windowList.Values)
            {
                window.ShowObject((window.HasNextSteps() || window.HasPreviousSteps()) ? this.displayLinkedObjects : this.displayUnlinkedObjects);
            }
        }

        // TODO: Need furture clarification about the load process
        internal bool UpdateFromHierarchy(AssembleController assembleController)
        {
            var needFresh = UpdateWindowList(assembleController);
            if (needFresh)
            {
                PrepareList(this.windowList);
            }
            return needFresh;
        }

        private bool UpdateWindowList(AssembleController assembleController)
        { 
            bool needRefresh = false;
            var nodeList = assembleController.GetAllNodes<Node>();
            var nodeSetToBeDeleted = new HashSet<int>(windowList.Keys);
            foreach (var node in nodeList)
            {
                WindowItem window;
                this.windowList.TryGetValue(node.nodeId, out window);
                if (window != null)
                {
                    // Exist item, just remove from node set and do nothing
                    nodeSetToBeDeleted.Remove(node.nodeId);
                    
                    if (window.IsDeleted())
                    {
                        window.MarkDeleted(false);
                    }
                    window.UpdateNodeName(node.partName);
                }
                else
                {
                    // The key is new, add new item
                    var newWindow = new WindowItem(node);
                    this.windowList.Add(newWindow.GetNodeId(), newWindow);
                    needRefresh = true;
                    Debug.Log("Added item " + newWindow.GetNodeId());
                }
            }
            foreach (var item in nodeSetToBeDeleted)
            {
                this.windowList[item].MarkDeleted(true);
                needRefresh = true;
                Debug.Log("Item deleted " + item);
            }
            return needRefresh;
        }
    }
}