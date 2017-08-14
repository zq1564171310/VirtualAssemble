/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(螺纹通孔))]
    [CanEditMultipleObjects]
    public class 螺纹通孔Editor : Editor
    {
        private void OnSceneGUI()
        {
            螺纹通孔 t = target as 螺纹通孔;
            Handles.color = Color.yellow;
            HandleExtention.DrawColumn(t.transform, Vector3.zero, t.半径 * 0.001f, t.厚度 * 0.001f);
        }
    }
}