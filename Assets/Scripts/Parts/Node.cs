/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Utils;

    /// <summary>
    /// 节点是每个零件的安装点。包括它的位置，旋转，还包括是哪个零件（零件名称）
    /// </summary>
    public class Node : MonoBehaviour, IFlowNode
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
        public Quaternion LocalRotation;             //零件原本世界坐标的角度
        [Range(0, 0.1f)]
        public float HaulingDistance;                //吸附距离

        public bool hasAnimation;
        public AnimationPlayer[] animationPlayers;


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

        public Vector3 GetDimensions()
        {
            Bounds totalBounds = new Bounds();
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                totalBounds = renderer.bounds;
            }
            foreach (Transform t in transform)
            {
                var childRenderer = t.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    totalBounds.Encapsulate(childRenderer.bounds);
                }
            }
            return totalBounds.extents;
        }

        public Coroutine PlayAnimations()
        {
            return StartCoroutine(PlayAnimationCoroutine());
        }

        private IEnumerator PlayAnimationCoroutine()
        {
            if (hasAnimation)
            {
                foreach (var animationAnchor in this.animationPlayers)
                {
                    yield return animationAnchor.PlayAnimation();
                }
            }
        }

        public int GetID()
        {
            return this.nodeId;
        }

        public string GetName()
        {
            return this.partName;
        }

        public GameObject GetTarget()
        {
            return this.gameObject;
        }


        /// <summary>
        /// 获取零件的大小尺寸
        /// </summary>
        /// <param name="partModel">要获取大小的零件物体</param>
        /// <returns></returns>
        public Vector3 GetPartModelRealSize(GameObject partModel)
        {
            //调用GetDimensions（）方法，返回unity物体真实大小
            if (null != partModel.GetComponent<MeshFilter>())
            {
                Vector3 Ver;
                float xSize = partModel.GetComponent<MeshFilter>().mesh.bounds.size.x * partModel.transform.localScale.x;
                float ySize = (float)Math.Round(partModel.GetComponent<MeshFilter>().mesh.bounds.size.y * partModel.transform.localScale.y, 3);
                float zSize = partModel.GetComponent<MeshFilter>().mesh.bounds.size.z * partModel.transform.localScale.z;
                Ver = new Vector3(xSize, ySize, zSize);
                return Ver;
            }
            else
            {
                return partModel.GetComponent<Node>().GetDimensions();
            }
        }

        /// <summary>
        /// 获取工具大大小尺寸
        /// </summary>
        /// <param name="toolModel">要获取大小的零件物体</param>
        /// <returns></returns>
        public Vector3 GetToolModelRealSize(GameObject toolModel)
        {
            Vector3 ver;                //声明一个变量，存储大小
            Transform[] Trans = toolModel.GetComponentsInChildren<Transform>();      //获取要获取的物体的子物体的变换的集合
            GameObject realToolModel = null;
            foreach (Transform child in Trans)
            {
                if (null != child.GetComponent<MeshFilter>())
                {
                    realToolModel = child.gameObject;
                }
            }
            if (null != realToolModel)
            {
                float xSize = realToolModel.GetComponent<MeshFilter>().mesh.bounds.size.x * realToolModel.transform.localScale.x;
                float ySize = (float)Math.Round(realToolModel.GetComponent<MeshFilter>().mesh.bounds.size.y * realToolModel.transform.localScale.y, 3);
                float zSize = realToolModel.GetComponent<MeshFilter>().mesh.bounds.size.z * realToolModel.transform.localScale.z;
                ver = new Vector3(xSize, ySize, zSize);
            }
            else
            {
                ver = new Vector3(0, 0, 0);
            }
            return ver;
        }

        /// <summary>
        /// 获取统一规格需要缩放的大小比例
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public float Scaling(ModelType type)
        {
            float scalingNum = 1;
            Vector3 targeSize = GlobalVar.ModelSize;
            if (ModelType.Part == type)
            {
                Vector3 localSize = GetPartModelRealSize(gameObject);
                List<float> list = new List<float>();
                list.Add(localSize.x);
                list.Add(localSize.y);
                list.Add(localSize.z);
                list.Sort();
                if (list[2] >= targeSize.x)
                {
                    scalingNum = list[2] / targeSize.x;                    //目前定义的x，y，z是一样大的，先把逻辑简单化，将来在做复杂逻辑
                }
                else
                {
                    scalingNum = Mathf.Max(0.5f, list[2] / targeSize.x);        // Do not make the parts too big
                }
            }
            else if (ModelType.Tools == type)
            {

            }
            return scalingNum;
        }
    }
}