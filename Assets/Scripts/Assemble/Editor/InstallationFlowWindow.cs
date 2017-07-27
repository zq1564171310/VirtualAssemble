/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using WyzLink.Parts;

    public class InstallationFlowWindow : EditorWindow
    {
        public AssembleController target;

        List<Node> nodeList;

        Vector2 scrollPosition;

        void OnGUI()
        {
            if (GUILayout.Button("扫描零件树", GUILayout.Width(100)))
            {
                if (this.target != null)
                {
                    nodeList = target.GetAllNodes<Node>();
                }
                else
                {
                    Debug.Log("Lost original game object. Please close the window and restart again");
                }
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.Height(80));
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            if (this.nodeList != null)
            {
                int counter = 0;
                foreach (var r in nodeList)
                {
                    if (GUILayout.Button(r.partName, GUILayout.MaxWidth(80)))
                    {
                        Selection.activeGameObject = r.gameObject;
                    }
                    counter++;
                    if (counter % 10 == 0)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("This is an sized label");
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginArea(new Rect(10, 180, 1000, 700));
            GUILayout.Button("Some button", GUILayout.Width(100));
            GUILayout.EndArea();

            DrawCurves(new Rect(0, 0, 40, 40), new Rect(300, 300, 50, 50));

            Handles.DrawLine(new Vector3(300, 0, 0), new Vector3(0, 300, 0));
        }

        private void OnHierarchyChange()
        {
            // Update the content from Hierarchy
            Debug.Log("Update from Hierarchy notification");

            // TODO: Optimization: We should check the performance on it, and only to add/remove when needed
            if (target != null)
            {
                this.nodeList = target.GetAllNodes<Node>();
            }
        }

        void DrawCurves(Rect wr, Rect wr2)
        {
            Color color = new Color(0.4f, 0.4f, 0.5f);
            var startPos = new Vector3(wr.x + wr.width, wr.y + 3 + wr.height / 2, 0);
            var endPos = new Vector3(wr2.x, wr2.y + wr2.height / 2, 0);
            var startTangent = startPos + Vector3.right * 50.0f;
            var endTangent = endPos - Vector3.left * 50.0f;
            Handles.DrawBezier(startPos, endPos, startTangent, endTangent, color, null, 5f);
        }
    }
}
