/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 零件安装类
/// </summary>
namespace WyzLink.Manager
{
    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.Control;
    using WyzLink.Parts;

    public class AssembleManager : Singleton<AssembleManager>
    {
        public Node InstalledNode;                      //当前已经被安装完成的零件
        private IEnumerable<Node> NextInstallNode;                   //下一步将要被安装的零件集合
        private DependencyGraph _DependencyGraph;


        // Use this for initialization
        void Start()
        {
            #region Test
            InstalledNode = NodesController.Instance.GetNodeList()[0];
            NextInstallNode = _DependencyGraph.GetNextSteps(InstalledNode);
            if (null != NextInstallNode)
            {
                string err = "";
                foreach (Node node in NextInstallNode)
                {
                    node.SetInstallationState(InstallationState.NextInstalling);
                    err += node.name + "/";
                }
                GlobalVar._ErrorMassage.GetComponent<Text>().text = "现在应该安装:" + err;
            }
            #endregion
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDependencyGraph(DependencyGraph dependencyGraph)
        {
            _DependencyGraph = dependencyGraph;
        }

        public void NextInstall(Node node)
        {
            NextInstallNode = _DependencyGraph.GetNextSteps(node);
            if (null != NextInstallNode)
            {
                string err = "";
                foreach (Node nodes in NextInstallNode)
                {
                    nodes.SetInstallationState(InstallationState.NextInstalling);
                    nodes.gameObject.AddComponent<HandDraggable>();
                    if (null != nodes.gameObject.GetComponent<MeshRenderer>())
                    {
                        nodes.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.NextInstallMate;
                    }
                    err += nodes.name + "/";
                    GlobalVar._ErrorMassage.GetComponent<Text>().text = "现在应该安装:" + err;
                }
            }
        }
    }
}
