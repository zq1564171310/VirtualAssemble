﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(AssembleController))]
    public class AssembleControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AssembleController myController = (AssembleController)target;
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.white;
            GUI.backgroundColor = Color.blue;
            // TODO: For now, the file is fixed. We should consider make it a generic file and could be changed
            if (GUILayout.Button("打开装配工序窗口", style))
            {
                var window = EditorWindow.GetWindow<WyzLinkAssembleFlow>(false, "装配工序");
                window.LoadContent(myController);
                //window.Show();
            }
        }
    }
}