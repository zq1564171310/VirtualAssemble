/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(通孔))]
    public class 通孔Editor : Editor
    {
        private void OnSceneGUI()
        {
            通孔 t = target as 通孔;
            Handles.color = Color.yellow;
            HandleExtention.DrawColumn(t.transform, Vector3.zero, t.半径, t.厚度);
        }
    }
}