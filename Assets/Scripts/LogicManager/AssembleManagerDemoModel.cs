/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 演示模式零件安装类
/// </summary>
namespace WyzLink.LogicManager
{
    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.Common;
    using WyzLink.Control;
    using WyzLink.Parts;
    using WyzLink.UI;

#if NETFX_CORE  //UWP下编译  
using Windows.Storage;
#endif

    public class AssembleManagerDemoModel : Singleton<AssembleManagerDemoModel>, AssembleManager
    {
        public List<Node> InstalledNodeList = new List<Node>();       //当前已经被安装完成的零件列表，按顺序放，这个集合中只可能出现两种零件，一种是从零件架上取下来的零件，一种是已经安装的零件
        private IEnumerable<Node> NextInstallNode;                   //下一步将要被安装的零件集合
        List<Node> NextInstallNodeList = new List<Node>();
        public DependencyGraph _DependencyGraph;                    //节点类实例化，用于获取下一步安装

        private Text PartInfo;            //零件信息内容
        private Text NextParts;           //下一步该零件零件提示内容

        public void Init()
        {
            PartInfo = GameObject.Find("Canvas/BG/InfoPanel/Panel/Info").GetComponent<Text>();            //零件信息内容
            NextParts = GameObject.Find("Canvas/BG/InfoPanel/Panel/Tips").GetComponent<Text>();           //下一步该零件零件提示内容

            #region Test
            for (int i = 0; i < NodesControllerDemoModel.Instance.GetNodeList().Count; i++)
            {
                if (NodesControllerDemoModel.Instance.GetNodeList()[i].GetInstallationState() == InstallationState.Installed)
                {
                    InstalledNodeList.Add(NodesControllerDemoModel.Instance.GetNodeList()[i]);
                }

                if (NodesControllerDemoModel.Instance.GetNodeList()[i].GetInstallationState() == InstallationState.NextInstalling)
                {
                    NextInstallNodeList.Add(NodesControllerDemoModel.Instance.GetNodeList()[i]);
                }
            }

            if (NextInstallNodeList.Count <= 0)
            {
                int currentNodeId = PlayerPrefs.GetInt("CurrentNodeIDDemoModel");
                Node currentNode = NodesControllerDemoModel.Instance.GetNodeList()[0];
                for (int i = 0; i < InstalledNodeList.Count; i++)
                {
                    if (InstalledNodeList[i].nodeId == currentNodeId)
                    {
                        currentNode = InstalledNodeList[i];
                        break;
                    }
                }
                NextInstallNode = _DependencyGraph.GetNextSteps(currentNode).Cast<Node>();
            }
            #endregion

        }

        /// <summary>
        /// 获取当前正在被安装的零件
        /// </summary>
        public Node GetCurrentnode()
        {
            return InstalledNodeList[InstalledNodeList.Count - 1];
        }

        /// <summary>
        /// 获取零件的起点位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPartStartPosition()
        {
            //演示模式当前没用到该接口，所以先空实现
            return Vector3.zero;
        }

        /// <summary>
        /// 初始化DependencyGraph类，调用底层
        /// </summary>
        /// <param name="dependencyGraph"></param>
        public void SetDependencyGraph(DependencyGraph dependencyGraph)
        {
            _DependencyGraph = dependencyGraph;
        }

        /// <summary>
        /// 设置当前已经安装的零件
        /// </summary>
        public void AddInstalledNodeList(Node installedNode)
        {
            InstalledNodeList.Add(installedNode);
        }

        /// <summary>
        /// 移除当前已经安装的零件
        /// </summary>
        /// <param name="installedNode"></param>
        public void ReMoveInstalledNodeList(Node installedNode)
        {
            InstalledNodeList.Remove(installedNode);
        }

        /// <summary>
        /// 修改某个零件的安装状态
        /// </summary>
        /// <param name="installedNode"></param>
        public void SetInstalledNodeListStatus(Node installedNode, InstallationState installationState)
        {
            for (int i = 0; i < InstalledNodeList.Count; i++)
            {
                if (InstalledNodeList[i].nodeId == installedNode.nodeId)
                {
                    InstalledNodeList[i].SetInstallationState(installationState);
                    break;
                }
            }
        }


        /// <summary>
        /// 获取下一步可以安装的零件集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Node> GetNextInstallNode()
        {
            return NextInstallNodeList;
        }

        public void NextInstall(Node node)
        {
            NextInstallNode = _DependencyGraph.GetNextSteps(node).Cast<Node>();
            if (null != NextInstallNode || NextInstallNode.Count() == 0)
            {
                string err = "";
                int index = 1;
                foreach (Node nodes in NextInstallNode)
                {
                    nodes.SetInstallationState(InstallationState.NextInstalling);
                    #region Test 跳转页面
                    #endregion
                    NextInstallNodeList.Add(node);
                }
                NextParts.text = err;
            }

            if (NextInstallNodeList.Count == 0)
            {
                for (int i = 0; i < NodesCommonDemoModel.Instance.GetNodesList().Count; i++)
                {
                    if (InstallationState.NotInstalled == NodesCommonDemoModel.Instance.GetNodesList()[i].GetInstallationState())
                    {
                        NextInstallNodeList.Add(NodesCommonDemoModel.Instance.GetNodesList()[i]);
                        NodesCommonDemoModel.Instance.GetNodesList()[i].SetInstallationState(InstallationState.NextInstalling);
                        break;
                    }
                }
            }
        }

        public void CaptureScreens()
        {
#if !NETFX_CORE
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/Test.png");
#else
            ScreenCapture.CaptureScreenshot(Windows.Storage.KnownFolders.PicturesLibrary + "/Test.png");
#endif
        }

    }

}
