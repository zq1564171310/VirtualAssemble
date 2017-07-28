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
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using WyzLink.Parts;

    public class InstallationFlowWindow : EditorWindow
    {
        public AssembleController target;

        //List<Node> nodeList;
        IEnumerable<WindowItem> windowList;

        Vector2 scrollPosition;

        void OnGUI()
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
                // TODO: Reset the position of all windows
            }

            GUILayout.EndHorizontal();

            scrollPosition = GUI.BeginScrollView(new Rect(0, 20, position.width, position.height - 20), scrollPosition, new Rect(0, 0, 1000, 1000));
            BeginWindows();
            if (windowList != null)
            {
                int count = 0;
                foreach (var w in windowList)
                {
                    w.Update();
                    count++;
                }
                Debug.Log("Item update count:" + count);
            }

            EndWindows();
            GUI.EndScrollView();

        }

        private IEnumerable<WindowItem> LoadAllNodes()
        {
            if (target == null)
            {
                return Enumerable.Empty<WindowItem>();
            }

            int index = 0;
            Vector2 position = new Vector2(10, 10);
            WindowItem previousWindow = null;

            var nodeList = target.GetAllNodes<Node>();
            return nodeList.Select((Node node) => {
                var window = new WindowItem(index, position, node);
                index++;
                position.x += 90;
                window.AddPreviousStep(previousWindow);
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
