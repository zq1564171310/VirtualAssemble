/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;

    public class Node : MonoBehaviour
    {
        [Header("零件属性")]
        [Tooltip("零件名称")]
        public string partName;
        [Tooltip("零件唯一标识")]
        public string partId;
        [Tooltip("零件唯一标识")]
        public int nodeId;
        [Tooltip("注释")]
        public string note;
        [Tooltip("库存编号")]
        public string inventoryId;

        [Header("零件信息")]
        public Vector3 position;

        private InstallationState installationState;

        private Vector3 originalPartPosition;
        private Quaternion originalPartRotation;

        private void Reset()
        {
            nodeId = Random.Range(0, 10000);
        }
    }
}