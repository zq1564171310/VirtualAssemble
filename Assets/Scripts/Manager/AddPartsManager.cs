﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加零件到集合
/// </summary>
namespace WyzLink.Manager
{
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Assemble;
    using WyzLink.Common;
    using WyzLink.Control;
    using WyzLink.Parts;

    public class AddPartsManager : MonoBehaviour
    {
        private Transform[] PartsTransform;

        private GameObject RootPartGameObject;

        void Awake()
        {

        }

        // Use this for initialization
        void Start()
        {
            //获取根节点的物体
            foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (null != rootObj.GetComponent<AssembleController>())
                {
                    RootPartGameObject = rootObj;
                    if (NodesController.Instance == null)
                    {
                        var _NodesController = new GameObject("NodesController", typeof(NodesController));
                        _NodesController.transform.parent = GlobalVar._RuntimeObject.transform;
                    }
                    if (NodesCommon.Instance == null)
                    {
                        var _NodesCommon = new GameObject("NodesCommon", typeof(NodesCommon));
                        _NodesCommon.transform.parent = GlobalVar._RuntimeObject.transform;
                    }
                }
            }

            //查找机器的子物体
            if (null != RootPartGameObject)
            {
                PartsTransform = RootPartGameObject.GetComponentsInChildren<Transform>();
            }

            if (null != PartsTransform)
            {
                Node node;
                foreach (Transform child in PartsTransform)
                {
                    if (null != child.GetComponent<Node>() && child.name != "接线器" && child.name != "气压瓶" && child.name != "底板组件0" && child.name != "耳机孔测试固定座" && child.name != "抽屉推拉杆")
                    {
                        child.gameObject.AddComponent<NodeManager>();
                        node = child.gameObject.GetComponent<Node>();
                        node.gameObject.AddComponent<BoxCollider>();
                        node.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                        node.gameObject.gameObject.AddComponent<Rigidbody>();
                        node.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        node.EndPos = child.transform.position;
                        if (null != child.GetComponent<MeshFilter>())
                        {
                            node.LocalSize = GlobalVar._GetModelSize.GetPartModelRealSize(child.gameObject);
                        }
                        else
                        {
                            node.GetComponent<BoxCollider>().size = node.GetComponent<BoxCollider>().size / 10;
                        }
                        node.partName = child.name;

                        #region  Test
                        if (child.name == "底盘平台")
                        {
                            node.SetInstallationState(InstallationState.Installed);     //底盘平台默认是安装的
                        }
                        else
                        {
                            node.SetInstallationState(InstallationState.NotInstalled);  //其他零件默认是未安装的
                        }
                        if (node.partName.Contains("工作台"))
                        {
                            node.Type = "底座以及底座相关";
                        }
                        else if (node.partName.Contains("抽屉"))
                        {
                            node.Type = "抽屉以及抽屉相关";
                        }
                        else
                        {
                            node.Type = "其他";
                        }
                        #endregion

                        NodesController.Instance.AddNodeList(node);
                    }
                }
                float ScalingNum = 1;
                for (int i = 0; i < NodesController.Instance.GetNodeList().Count; i++)
                {
                    if (null != NodesController.Instance.GetNodeList()[i].GetComponent<MeshFilter>())
                    {
                        ScalingNum = GlobalVar._GetModelSize.Scaling(NodesController.Instance.GetNodeList()[i].gameObject, ModelType.Part);
                        NodesController.Instance.GetNodeList()[i].gameObject.transform.localScale = new Vector3(NodesController.Instance.GetNodeList()[i].gameObject.transform.localScale.x / ScalingNum, NodesController.Instance.GetNodeList()[i].gameObject.transform.localScale.y / ScalingNum, NodesController.Instance.GetNodeList()[i].gameObject.transform.localScale.z / ScalingNum);
                    }
                }

                if (AssembleManager.Instance == null)
                {
                    var _AssembleManager = new GameObject("AssembleManager", typeof(AssembleManager));
                    _AssembleManager.transform.parent = GlobalVar._RuntimeObject.transform;
                    _AssembleManager.GetComponent<AssembleManager>().SetDependencyGraph(new DependencyGraph(RootPartGameObject.GetComponent<AssembleController>(), RootPartGameObject.GetComponent<AssembleController>().assembleFlow.text));
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
