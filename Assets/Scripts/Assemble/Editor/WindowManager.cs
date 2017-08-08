/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// Manages the list of the windows on the screen
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
    using Utils;
    using WyzLink.Parts;

    public class WindowManager<T> where T: IFlowNode
    {
        private IDictionary<int, WindowItem<T>> windowList = new Dictionary<int, WindowItem<T>>();
        private IList<WindowItem<T>> headers;
        private IList<WindowItem<T>> standalones;
        private const string defaultFileName = "AssembleFlow.txt";      // TODO: Will change this to a new file format later

        private bool isDirty = true;
        private bool doLayout = false;
        private int layoutToken = 0;
        private float[] layoutLayerBuffer;

        private bool displayLinkedObjects = true;     // Display objects by default
        private bool displayUnlinkedObjects = true;     // Display objects by default

        private Action<Rect> layoutCallback = null;

        public WindowManager()
        {
            this.windowList = new Dictionary<int, WindowItem<T>>();
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

        internal void UpdateWindowsLate()
        {
            foreach (var window in windowList.Values)
            {
                window.UpdateLate();
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

        private void ApplyAssembleFlow(IDictionary<int, WindowItem<T>> windowList, string flowString)
        {
            AssembleFlowParser.ParseAssembleFlowFile(flowString, (int step0, int step1) =>
            {
                WindowItem<T> w0;
                WindowItem<T> w1;
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

        private void PrepareList(IDictionary<int, WindowItem<T>> windowList)
        {
            this.headers = new List<WindowItem<T>>();
            this.standalones = new List<WindowItem<T>>();
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
            var startPoint = Vector2.one * WindowItem<T>.LayoutGapVerticallyGroup;
            this.layoutToken++;     // Start a new layout
            var panelSize = Rect.zero;
            if (this.headers != null)
            {
                foreach (var window in this.headers)
                {
                    var rect = LayoutFlow(window, startPoint);
                    startPoint.y = rect.yMax + WindowItem<T>.LayoutGapVerticallyGroup;
                    panelSize = panelSize.Union(rect);
                }
            }
            if (this.standalones != null)
            {
                foreach (var window in this.standalones)
                {
                    // Layout individual items
                    window.LayoutAt(startPoint);
                    startPoint.x += window.GetWindowWidth() + WindowItem<T>.WindowMarginLeft;
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

        private Rect LayoutFlow(WindowItem<T> startWindow, Vector2 startPoint)
        {
            var list = new List<WindowItem<T>>() { startWindow };
            this.layoutLayerBuffer = new float[1000];
            return LayoutWindowLayer(list, startPoint, 0);
        }

        private Rect LayoutWindowLayer(IList<WindowItem<T>> list, Vector2 point, int layer)
        {
            if (layer > 1000)
            {
                Debug.LogError("Stack overflow. Please check if there is a loop in the graph and remove it.");
                return new Rect(point, Vector2.zero);
            }
            if (list.Count == 0)
            {
                return new Rect(point, Vector2.zero);
            }

            Rect rect = new Rect(point, Vector2.zero);
            float maxHeight = point.y;
            foreach (var item in list)
            {
                if (item.LayoutToken != this.layoutToken || item.GetWindowRect().x < point.x)
                {
                    maxHeight = Mathf.Max(maxHeight, TestChildrenHeights(item.GetNextSteps(), layer + 1));
                    item.LayoutAt(new Vector2(point.x, maxHeight));
                    rect = rect.Union(item.GetWindowRect());

                    var childRect = LayoutWindowLayer(item.GetNextSteps(), new Vector2(point.x + item.GetWindowWidth() + WindowItem<T>.LayoutGapHorizontally, maxHeight), layer + 1);
                    rect = rect.Union(childRect);

                    // Mark the item
                    item.LayoutToken = this.layoutToken;
                    item.Layer = layer;

                    // Progress to next item
                    maxHeight += item.GetWindowRect().height + WindowItem<T>.LayoutGapVertically;
                    this.layoutLayerBuffer[layer] = maxHeight;
                }
            }
            return rect;
        }

        private float TestChildrenHeights(List<WindowItem<T>> list, int layer)
        {
            if (layer > 1000)
            {
                Debug.LogError("Stack overflow. Please check if there is a loop in the graph and remove it.");
                return 0;
            }

            var maxHeight = this.layoutLayerBuffer[layer];
            foreach (var item in list)
            {
                if (item.LayoutToken != this.layoutToken)
                {
                    maxHeight = Mathf.Max(maxHeight, TestChildrenHeights(item.GetNextSteps(), layer + 1));
                }
            }
            return maxHeight;
        }

        public WindowItem<T> GetCurrentWindow(Vector2 mousePosition, bool connectionAreaOnly)
        {
            var position = mousePosition;
            WindowItem<T> windowHit = null;
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
                UpdateVisibility();
            }
            return needFresh;
        }

        private bool UpdateWindowList(AssembleController assembleController)
        { 
            bool needRefresh = false;
            // A bit hacky, but so far the best way to convert any type to T
            IEnumerable<T> nodeList = assembleController.GetAllNodes<T>();
            var nodeSetToBeDeleted = new HashSet<int>(windowList.Keys);
            foreach (var node in nodeList)
            {
                WindowItem<T> window;
                this.windowList.TryGetValue(node.GetID(), out window);
                if (window != null)
                {
                    // Exist item, just remove from node set and do nothing
                    nodeSetToBeDeleted.Remove(node.GetID());
                    
                    if (window.IsDeleted())
                    {
                        window.MarkDeleted(false);
                    }
                    if (node.GetName() != window.GetNodeName())
                    {
                        window.UpdateNodeName(node.GetName());
                        needRefresh = true;
                    }
                }
                else
                {
                    // The key is new, add new item
                    var newWindow = new WindowItem<T>(node);
                    this.windowList.Add(newWindow.GetNodeId(), newWindow);
                    needRefresh = true;
                    //Debug.Log("Added item " + newWindow.GetNodeId());
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