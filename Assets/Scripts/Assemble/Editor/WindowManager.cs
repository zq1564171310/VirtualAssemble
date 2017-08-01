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
        private IDictionary<int, WindowItem> windowList;
        private IList<WindowItem> heads;
        private IList<WindowItem> standalones;
        private const string defaultFileName = "AssembleFlow.txt";      // TODO: Will change this to a new file format later

        public List<Rect> debugRects = new List<Rect>();
        private bool isDirty = true;
        private int layoutToken = 0;

        public WindowManager()
        {
            this.windowList = new Dictionary<int, WindowItem>();
        }

        public IEnumerable<WindowItem> GetWindowList()
        {
            return windowList.Values;
        }

        public int Count()
        {
            return windowList.Count;
        }

        public Rect LoadWindows(AssembleController assembleController)
        {
            if (assembleController == null)
            {
                return Rect.zero;
            }
            if (isDirty)
            {
                this.windowList = LoadAllNodesToDictionary(assembleController);
                var flowString = LoadAssembleFlowFromFile(defaultFileName);
                ApplyAssembleFlow(this.windowList, flowString);
                PrepareList(windowList);
                this.isDirty = false;
            }
            return LayoutWindows();
        }

        public Rect RefreshLayout()
        {
            if (isDirty)
            {
                PrepareList(windowList);
                this.isDirty = false;
            }
            return LayoutWindows();
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
            this.heads = new List<WindowItem>();
            this.standalones = new List<WindowItem>();
            foreach (var window in windowList.Values)
            {
                if (!window.HasPreviousSteps())
                {
                    if (window.HasNextSteps())
                    {
                        this.heads.Add(window);
                    }
                    else
                    {
                        this.standalones.Add(window);
                    }
                }
            }
        }

        private IDictionary<int, WindowItem> LoadAllNodesToDictionary(AssembleController target)
        {
            var dictionary = new Dictionary<int, WindowItem>();
            if (target == null)
            {
                return dictionary;
            }

            int index = 0;
            Vector2 position = new Vector2(20, 20);

            var nodeList = target.GetAllNodes<Node>();
            foreach (var node in nodeList)
            {
                var window = new WindowItem(index, position, node);
                // TODO: Do position with Layout flow
                index++;
                //position.x += 90;
                dictionary.Add(window.GetNodeId(), window);
            }
            return dictionary;
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

        private Rect LayoutWindows()
        {
            var startPoint = Vector2.one * 20;
            this.layoutToken++;     // Start a new layout
            var panelSize = Rect.zero;
            if (this.heads != null)
            {
                foreach (var window in this.heads)
                {
                    var rect = LayoutWindows(window, startPoint);
                    startPoint.y = rect.yMax + 20;
                    panelSize = panelSize.Union(rect);
                }
            }
            if (this.standalones != null)
            {
                foreach (var window in this.standalones)
                {
                    // Layout individual items
                    window.MoveTo(startPoint);
                    startPoint.x += 90;
                    panelSize = panelSize.Union(window.GetWindowRect());
                }
            }
            panelSize.height += 20; // Buttom margin
            return panelSize;
        }

        private Rect LayoutWindows(WindowItem firstWindow, Vector2 startPoint)
        {
            var list = new List<WindowItem>();
            list.Add(firstWindow);
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
            List<WindowItem> nextList = new List<WindowItem>();
            foreach (var item in list)
            {
                if (item.LayoutToken != this.layoutToken || item.Layer < layer)
                {
                    item.MoveTo(point);
                    rect = rect.Union(item.GetWindowRect());
                    var childRect = LayoutWindowLayer(item.GetNextSteps(), point + new Vector2(item.GetWindowWidth() + WindowItem.LayoutGapHorizontally, 0), layer + 1);
                    point.y += Mathf.Max(childRect.height, item.GetWindowRect().height) + WindowItem.LayoutGapVertically;
                    rect = rect.Union(childRect);

                    // Mark the item
                    item.LayoutToken = this.layoutToken;
                    item.Layer = layer;
                }
            }
            this.debugRects.Add(rect);
            return rect;
        }

        private Vector2 NextRow(Vector2 point, int maxWidth)
        {
            return new Vector2(point.x + maxWidth + WindowItem.LayoutGapHorizontally, 20);
        }

        private IEnumerable<object> GetHeads()
        {
            throw new NotImplementedException();
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
        }
    }
}