/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using Utils;

    /// <summary>
    /// 节点是每个零件的安装点。包括它的位置，旋转，还包括是哪个零件（零件名称）
    /// </summary>
    public class Node : MonoBehaviour
    {
        [Tooltip("节点唯一标识")]
        [ReadOnly]
        public int nodeId;

        [Header("零件属性")]
        [Tooltip("零件名称")]
        public string partName;
        [Tooltip("零件唯一标识")]
        public string partId;
        [Tooltip("零件所属类别")]
        public string Type;                      //零件类别（零件所在的不相同的Group）

        [Tooltip("注释")]
        public string note;
        [Tooltip("库存编号")]
        public string inventoryId;

        // 导入点
        [Header("零件信息")]
        public Vector3 position;

        private InstallationState installationState;      //安装状态

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        public Vector3 StartPos;                  //安装的起始位置（在零件架上的位置）
        public Vector3 EndPos;                    //安装的终点位置（最终在工作区上的位置）
        public Vector3 LocalSize;                 //原本尺寸（从零件架上取下之后的大小，零件架上的零件都会被放缩到差不多的大小）


        private void Reset()
        {
            // 添加文件的时候或者重置的时候生成新节点标识
            this.nodeId = IdCounter.Instance.GetNextId();
            this.partName = this.gameObject.name;
        }

        private void Start()
        {
            // Capture the target points
            this.targetPosition = this.transform.position;
            this.targetRotation = this.transform.rotation;
        }

        internal InstallationState GetInstallationState()
        {
            return this.installationState;
        }

        internal void SetInstallationState(InstallationState installationState)
        {
            this.installationState = installationState;
        }

        public Vector3 GetTargetPosition()
        {
            return targetPosition;
        }

    }
}