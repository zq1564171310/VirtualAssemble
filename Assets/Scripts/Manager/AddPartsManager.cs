/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加零件到集合
/// </summary>
namespace WyzLink.Manager
{
    using UnityEngine;
    using WyzLink.Assemble;
    using WyzLink.Common;
    using WyzLink.Control;
    using WyzLink.Parts;

    public class AddPartsManager : MonoBehaviour
    {
        private GameObject _RuntimeObjectNodes;                           //获取物体RuntimeObject

        private Transform[] PartsTransform;                    //零件Transform集合

        private AssembleController RootPartGameObject;          //零件跟节点

        private Vector3 MainWorkSpacePos;           //主工作区位置

        private Vector3 SecondWorkSpacePos;        //第二工作区位置

        // Use this for initialization
        void Start()
        {

        }

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
                if (NodesController.Instance == null)
                {
                    var _NodesController = new GameObject("NodesController", typeof(NodesController));
                    _NodesController.transform.parent = _RuntimeObjectNodes.transform;
                }
                if (NodesCommon.Instance == null)
                {
                    var _NodesCommon = new GameObject("NodesCommon", typeof(NodesCommon));
                    _NodesCommon.transform.parent = _RuntimeObjectNodes.transform;
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
                        else if (node.partName.Contains("按钮"))
                        {
                            node.Type = "按钮以及按钮相关";
                        }
                        else if (node.partName.Contains("照明"))
                        {
                            node.Type = "照明以及照明相关";
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

                        NodesController.Instance.AddNodeList(node);
                    }
                }
                float ScalingNum = 1;
                for (int i = 0; i < NodesController.Instance.GetNodeList().Count; i++)
                {
                    if (null != NodesController.Instance.GetNodeList()[i].GetComponent<MeshFilter>())
                    {
                        ScalingNum = NodesController.Instance.GetNodeList()[i].Scaling(ModelType.Part);
                        NodesController.Instance.GetNodeList()[i].gameObject.transform.localScale /= ScalingNum;
                    }
                }

                if (AssembleManager.Instance == null)
                {
                    var _AssembleManager = new GameObject("AssembleManager", typeof(AssembleManager));
                    _AssembleManager.transform.parent = _RuntimeObjectNodes.transform;
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
