/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(双层通孔))]
    [CanEditMultipleObjects]
    public class 双层通孔Editor : Editor
    {
        private void OnSceneGUI()
        {
            双层通孔 t = target as 双层通孔;
            Handles.color = Color.yellow;

            HandleExtention.DrawColumn(t.transform, Vector3.zero + Vector3.forward * t.厚度1 * 0.001f / 2, t.直径1 * 0.001f / 2, t.厚度1 * 0.001f);
            HandleExtention.DrawColumn(t.transform, Vector3.zero - Vector3.forward * t.厚度2 * 0.001f / 2, t.直径2 * 0.001f / 2, t.厚度2 * 0.001f);
        }
    }
}