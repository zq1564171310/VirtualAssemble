/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(双层长孔))]
    [CanEditMultipleObjects]
    public class 双层长孔Editor : Editor
    {
        private void OnSceneGUI()
        {
            双层长孔 t = target as 双层长孔;
            Handles.color = Color.yellow;

            HandleExtention.DrawDualHole(t.transform, Vector3.zero + Vector3.forward * t.厚度1 * 0.001f / 2, t.长度 * 0.001f, t.半径1 * 0.001f, t.厚度1 * 0.001f);
            HandleExtention.DrawDualHole(t.transform, Vector3.zero - Vector3.forward * t.厚度2 * 0.001f / 2, t.长度 * 0.001f, t.半径2 * 0.001f, t.厚度2 * 0.001f);
        }
    }
}