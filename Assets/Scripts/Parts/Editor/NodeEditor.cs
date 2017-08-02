/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// This is the editor options for node component
/// </summary>

using UnityEditor;

namespace WyzLink.Parts
{
    [CustomEditor(typeof(Node))]
    public class NodeEditor : Editor
    {
        // Exclude the script part in component
        private static readonly string[] _dontIncludeMe = new string[] { "m_Script" };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);

            serializedObject.ApplyModifiedProperties();
        }
    }
}