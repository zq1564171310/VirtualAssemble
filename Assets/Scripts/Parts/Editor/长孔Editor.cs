/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(长孔))]
    [CanEditMultipleObjects]
    public class 长孔Editor : Editor
    {
        private void OnSceneGUI()
        {
            长孔 t = target as 长孔;
            Handles.color = Color.yellow;
            HandleExtention.DrawDualHole(t.transform, Vector3.zero, t.长度 * 0.001f, t.直径 * 0.001f / 2, t.厚度 * 0.001f);
        }
    }
}