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
        string myString = "Hello World";
        bool groupEnabled;
        bool myBool = true;
        float myFloat = 1.23f;

        public AssembleController target;

        List<Node> activePartList;

        void OnGUI()
        {
            if (GUILayout.Button("扫描零件树"))
            {
                activePartList = target.GetAllPartObject<Node>();
            }

            GUILayout.BeginHorizontal();
            if (this.activePartList != null)
            {
                foreach (var r in activePartList)
                {
                    if (GUILayout.Button(r.partName))
                    {
                        Selection.activeGameObject = r.gameObject;
                    }
                    Debug.Log("Got part: " + r.partName);
                }
            }
        }
    }
}
