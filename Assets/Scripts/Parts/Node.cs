/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;
    using Utils;

    /// <summary>
    /// 节点是每个零件的安装点。包括它的位置，旋转，还包括是哪个零件（零件名称）
    /// </summary>
    public class Node : MonoBehaviour
    {
        [Header("零件属性")]
        [Tooltip("零件名称")]
        public string partName;
        [Tooltip("零件唯一标识")]
        public string partId;

        [Tooltip("节点唯一标识")]
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
            // 添加文件的时候或者重置的时候生成新节点标识
            nodeId = IdCounter.Instance.GetNextId();
        }
    }
}