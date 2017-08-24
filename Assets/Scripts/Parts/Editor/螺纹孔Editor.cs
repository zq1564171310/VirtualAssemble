/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(螺纹孔))]
    [CanEditMultipleObjects]
    public class 螺纹孔Editor : Editor
    {
        private void OnSceneGUI()
        {
            螺纹孔 t = target as 螺纹孔;
            Handles.color = Color.yellow;
            HandleExtention.DrawColumn(t.transform, Vector3.zero + Vector3.forward * t.厚度 * 0.001f / 2, t.直径 * 0.001f / 2, t.厚度 * 0.001f);
            HandleExtention.DrawEnd(t.transform, Vector3.zero + Vector3.forward * t.厚度 * 0.001f, t.直径 * 0.001f / 2);
        }
    }
}