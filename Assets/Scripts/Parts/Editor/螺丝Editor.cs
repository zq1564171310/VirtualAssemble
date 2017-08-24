/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(螺丝))]
    [CanEditMultipleObjects]
    public class 螺丝Editor : Editor
    {
        private void OnSceneGUI()
        {
            螺丝 t = target as 螺丝;
            Handles.color = Color.yellow;
            HandleExtention.DrawColumn(t.transform, Vector3.zero + Vector3.forward * t.长度 * 0.001f / 2, t.直径 * 0.001f / 2, t.长度 * 0.001f);
            HandleExtention.DrawEnd(t.transform, Vector3.zero + Vector3.forward * t.长度 * 0.001f, t.直径 * 0.001f / 2, 0);
        }
    }
}