/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
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

        private AssembleController RootPartGameObject;

        //private NodesController _NodesController;

        void Awake()
        {
            //_NodesController = NodesController.Instance;
        }

        // Use this for initialization
        void Start()
        {
            //获取根节点的物体
            RootPartGameObject = GameObject.FindObjectOfType<AssembleController>();
            if (null != RootPartGameObject)
            {
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
                        child.gameObject.AddComponent<NodeManager>();
                        node = child.gameObject.GetComponent<Node>();
                        node.EndPos = child.transform.position;
                        node.LocalSize = node.GetDimensions();
                        node.partName = child.name;
                        #region  Test
                        if (node.partName.Contains("底座"))
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
                List<string> list = NodesCommon.Instance.GetNodeTypes();
                float ScalingNum = 1;
                for (int i = 0; i < NodesController.Instance.GetNodeList().Count; i++)
                {
                    ScalingNum = GlobalVar._GetModelSize.Scaling(NodesController.Instance.GetNodeList()[i].gameObject, ModelType.Part);
                    NodesController.Instance.GetNodeList()[i].gameObject.transform.localScale /= ScalingNum;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
