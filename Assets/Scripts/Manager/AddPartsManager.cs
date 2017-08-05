/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加零件到集合
/// </summary>
namespace WyzLink.Manager
{
    using UnityEngine;
    using WyzLink.Assemble;
    using WyzLink.Control;
    using WyzLink.Parts;

    public class AddPartsManager : MonoBehaviour
    {
        private Transform[] PartsTransform;

        private GameObject RootPartGameObject;

        //private NodesController _NodesController;

        void Awake()
        {
            //_NodesController = NodesController.Instance;
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
                    if (null != child.GetComponent<Node>() && child.name != "底座平台" && child.name != "储气罐" && child.name != "抽屉气推" && child.name != "背门合页" && child.name != "背门合页 (1)" && child.name != "背门合页 (2)" && child.name != "背门合页 (3)" && child.name != "内部结构" && child.name != "背板锁" && child.name != "轨道固定座0" && child.name != "轨道固定座1" && child.name != "轨道固定杆" && child.name != "滑杆")
                    {
                        child.gameObject.AddComponent<NodeManager>();
                        node = child.gameObject.GetComponent<Node>();
                        node.EndPos = child.transform.position;
                        node.LocalSize = GlobalVar._GetModelSize.GetPartModelRealSize(child.gameObject);
                        node.partName = child.name;
                        NodesController.Instance.NodeList.Add(node);
                    }
                }

                float ScalingNum = 1;
                for (int i = 0; i < NodesController.Instance.NodeList.Count; i++)
                {
                    ScalingNum = GlobalVar._GetModelSize.Scaling(NodesController.Instance.NodeList[i].gameObject, ModelType.Part);
                    NodesController.Instance.NodeList[i].gameObject.transform.localScale = new Vector3(NodesController.Instance.NodeList[i].gameObject.transform.localScale.x / ScalingNum, NodesController.Instance.NodeList[i].gameObject.transform.localScale.y / ScalingNum, NodesController.Instance.NodeList[i].gameObject.transform.localScale.z / ScalingNum);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
