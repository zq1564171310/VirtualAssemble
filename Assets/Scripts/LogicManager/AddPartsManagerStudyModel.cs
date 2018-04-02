/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 学习模式扫描添加零件到集合
/// </summary>
namespace WyzLink.LogicManager
{
    using HoloToolkit.Unity;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Assemble;
    using WyzLink.Common;
    using WyzLink.Control;
    using WyzLink.Parts;

    public class AddPartsManagerStudyModel : Singleton<AddPartsManagerStudyModel>, AddPartsManager
    {
        private GameObject _RuntimeObjectNodes;                           //获取物体RuntimeObject

        private Transform[] PartsTransform;                    //零件Transform集合

        private AssembleController RootPartGameObject;          //零件跟节点

        private Vector3 MainWorkSpacePos;           //主工作区位置

        private Vector3 SecondWorkSpacePos;        //第二工作区位置

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            _RuntimeObjectNodes = GameObject.Find("RuntimeObject/Nodes");
            MainWorkSpacePos = GameObject.Find("Canvas/Floor/MainWorkSpace").transform.position;
            //SecondWorkSpacePos = GameObject.Find("Canvas/Floor/MainWorkSpace2").transform.position;

            //获取根节点的物体
            RootPartGameObject = FindObjectOfType<AssembleController>();

            if (null != RootPartGameObject)
            {
                if (null == NodesControllerStudyModel.Instance)
                {
                    var _NodesControllerStudyModel = new GameObject("NodesControllerStudyModel", typeof(NodesControllerStudyModel));
                    _NodesControllerStudyModel.transform.parent = _RuntimeObjectNodes.transform;
                }
                if (null == NodesCommonStudyModel.Instance)
                {
                    var _NodesCommonStudyModel = new GameObject("NodesCommonStudyModel", typeof(NodesCommonStudyModel));
                    _NodesCommonStudyModel.transform.parent = _RuntimeObjectNodes.transform;
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
                    if (null != child.GetComponent<Node>())
                    {
                        node = child.gameObject.GetComponent<Node>();
                        node.EndPos = child.transform.position;
                        node.EndPosForScale = node.EndPos;
                        //node.LocalSize = node.GetDimensions();
                        node.LocalSize = child.transform.localScale;
                        node.partName = child.name;

                        #region  Test       暂时的零件类型划分，后期需要单独界面，做成可配置的
                        if (child.name == "底盘平台")
                        {
                            node.SetInstallationState(InstallationState.Installed);     //底盘平台默认是安装的
                        }
                        else
                        {
                            //node.SetInstallationState(InstallationState.NotInstalled);  //其他零件默认是未安装的
                            node.SetInstallationState((InstallationState)PlayerPrefs.GetInt(node.nodeId.ToString()));
                        }

                        if (node.partName.Contains("测试箱"))
                        {
                            node.Type = "测试箱相关组件";
                        }
                        else if (node.partName.Contains("操作台"))
                        {
                            node.Type = "操作台相关组件";
                        }
                        else if (node.partName.Contains("底板"))
                        {
                            node.Type = "底板相关组件";
                        }
                        else if (node.partName.Contains("照明"))
                        {
                            node.Type = "照明相关组件";
                        }
                        else
                        {
                            node.Type = "其他";
                        }

                        //if (node.nodeId == 5007 || node.nodeId == 5008 || node.nodeId == 5012 || node.nodeId == 5020 || node.nodeId == 5018 || node.nodeId == 5013 || node.nodeId == 5021 || node.nodeId == 5022)
                        //{
                        //    node.WorkSpaceID = 2;
                        //    node.WorSpaceRelativePos = WorkSpaceManager.GetPartsInOtherWorkSpacePosition(node, MainWorkSpacePos, SecondWorkSpacePos);
                        //    node.WorkSpaceID = 1;
                        //}
                        //else
                        //{
                        node.WorkSpaceID = 1;
                        //}
                        #endregion

                        NodesControllerStudyModel.Instance.AddNodeList(node);
                    }
                }
                float ScalingNum = 1;
                for (int i = 0; i < NodesControllerStudyModel.Instance.GetNodeList().Count; i++)
                {
                    if (null != NodesControllerStudyModel.Instance.GetNodeList()[i].GetComponent<MeshFilter>())
                    {
                        ScalingNum = NodesControllerStudyModel.Instance.GetNodeList()[i].Scaling(ModelType.Part);
                        NodesControllerStudyModel.Instance.GetNodeList()[i].gameObject.transform.localScale /= ScalingNum;
                        if (NodesControllerStudyModel.Instance.GetNodeList()[i].gameObject.transform.name == "抽屉组件")
                        {
                            NodesControllerStudyModel.Instance.GetNodeList()[i].gameObject.transform.localScale /= 10;
                        }
                        if (NodesControllerStudyModel.Instance.GetNodeList()[i].gameObject.transform.name == "外壳组件")
                        {
                            NodesControllerStudyModel.Instance.GetNodeList()[i].gameObject.transform.localScale /= 10;
                        }
                    }
                }


                if (null == AssembleManagerStudyModel.Instance)
                {
                    var _AssembleManager = new GameObject("AssembleManager", typeof(AssembleManagerStudyModel));
                    _AssembleManager.transform.parent = _RuntimeObjectNodes.transform;
                    _AssembleManager.GetComponent<AssembleManagerStudyModel>().SetDependencyGraph(new DependencyGraph(RootPartGameObject.GetComponent<AssembleController>(), RootPartGameObject.GetComponent<AssembleController>().assembleFlow.text));
                }
            }
        }

    }
}
