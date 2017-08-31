﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 零件安装类
/// </summary>
namespace WyzLink.Manager
{
    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.Control;
    using WyzLink.Parts;

#if NETFX_CORE  //UWP下编译  
using Windows.Storage;
#endif

    public class AssembleManager : Singleton<AssembleManager>
    {
        public Node InstalledNode;                      //当前已经被安装完成的零件
        private IEnumerable<Node> NextInstallNode;                   //下一步将要被安装的零件集合
        private DependencyGraph _DependencyGraph;                    //节点类实例化，用于获取下一步安装
        private float WorkSpaceScalingNum = 1;                          //工作区缩放倍数
        private float WorkSpaceRotaAngle = 90;                               //工作区旋转角度

        protected override void Awake()
        {
            base.Awake();
            Find();
        }

        // Use this for initialization
        void Start()
        {
            Init();
        }

        public void Init()
        {
            #region Test
            InstalledNode = NodesController.Instance.GetNodeList()[0];
            NextInstallNode = _DependencyGraph.GetNextSteps(InstalledNode).Cast<Node>();
            if (null != NextInstallNode && EntryMode.Mode != "Test")
            {
                string err = "";
                int index = 1;
                foreach (Node node in NextInstallNode)
                {
                    node.SetInstallationState(InstallationState.NextInstalling);
                    #region Test 跳转页面
                    GlobalVar._UIPartsPage.SetIndex(node);
                    index = GlobalVar._UIPartsPage.GetIndex(node);
                    #endregion
                    err += node.name + "(第" + index + "页）" + "/";
                }
                GlobalVar._Tips.text = "现在应该安装:" + err;
            }
            #endregion
            //获取物体的绝对路径，新的UI中都会改掉
            GlobalVar._Slider.onValueChanged.AddListener(SlideTheSlider);
            GlobalVar._UIPartsPage.RefreshItems();
        }

        void Find()
        {
            GameObject.Find("Canvas/Floor/MainWorkSpace/Rota_Left").GetComponent<Button>().onClick.AddListener(RotaLeftBtnClick);
            GameObject.Find("Canvas/Floor/MainWorkSpace/Rota_Right").GetComponent<Button>().onClick.AddListener(RotaRightBtnClick);
            GameObject.Find("Canvas/BG/PartsPanel/CaptureScreens_Btn").GetComponent<Button>().onClick.AddListener(CaptureScreensBtnClick);
        }

        // Update is called once per frame
        void Update()
        {

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
        /// 获取下一步可以安装的零件集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Node> GetNextInstallNode()
        {
            return NextInstallNode;
        }

        public void NextInstall(Node node)
        {
            NextInstallNode = _DependencyGraph.GetNextSteps(node).Cast<Node>();
            if (null != NextInstallNode)
            {
                string err = "";
                int index = 1;
                foreach (Node nodes in NextInstallNode)
                {
                    nodes.SetInstallationState(InstallationState.NextInstalling);
                    nodes.gameObject.AddComponent<HandDraggable>();
                    if (null != nodes.gameObject.GetComponent<MeshRenderer>())
                    {
                        nodes.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.NextInstallMate;
                    }
                    #region Test 跳转页面
                    GlobalVar._UIPartsPage.SetIndex(nodes);
                    index = GlobalVar._UIPartsPage.GetIndex(nodes);
                    #endregion
                    err += nodes.name + "(第" + index + "页）" + "/";
                }
                GlobalVar._Tips.text = "现在应该安装:" + err;
            }
        }

        public void SlideTheSlider(float value)
        {
            int Num = 100;
            if (10 > value)              //缩小
            {
                Num = (int)(value * 10);
            }
            else if (10 == value)         //正常比例
            {

            }
            else                       //放大
            {
                Num = (int)((value - 10) * 100);
            }
            WorkSpaceScalingNum = (float)(Num / 100.00);
            WorkSpaceScal(WorkSpaceScalingNum);
            GlobalVar._SliderText.text = "工作区显示比例" + Num + "%";
        }

        public void RotaLeftBtnClick()
        {
            WorkSpaceRota(0);
        }

        public void RotaRightBtnClick()
        {
            WorkSpaceRota(1);
        }

        public void CaptureScreensBtnClick()
        {
#if !NETFX_CORE  
            //ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/Test.png");
#else
            //ScreenCapture.CaptureScreenshot(Windows.Storage.KnownFolders.PicturesLibrary + "/Test.png");
#endif
        }

        /// <summary>
        /// 工作取缩放
        /// </summary>
        public void WorkSpaceScal(float scalNum)
        {
            //将已经安装的或者正在安装的模型缩放
            for (int i = 0; i < NodesController.Instance.GetNodeList().Count; i++)
            {
                if (InstallationState.Step1Installed == NodesController.Instance.GetNodeList()[i].GetInstallationState() || InstallationState.Installed == NodesController.Instance.GetNodeList()[i].GetInstallationState())
                {
                    //将模型缩放，此处有隐患（组合模型）
                    NodesController.Instance.GetNodeList()[i].gameObject.transform.localScale = NodesController.Instance.GetNodeList()[i].gameObject.GetComponent<Node>().LocalSize * scalNum;
                }
            }
            //工作区大小随着缩放
            GameObject.Find("Canvas/WorkSpacePanel").transform.localScale *= scalNum;
        }

        /// <summary>
        /// 工作区旋转
        /// </summary>
        /// <param name="rota"></param>
        public void WorkSpaceRota(int rota)
        {
            //获取工作区中心位置
            Vector3 vec = GameObject.Find("Canvas/WorkSpacePanel").transform.position;
            if (0 == rota)        //左旋转
            {
                for (int i = 0; i < NodesController.Instance.GetNodeList().Count; i++)
                {
                    if (InstallationState.Step1Installed == NodesController.Instance.GetNodeList()[i].GetInstallationState())
                    {
                        NodesController.Instance.GetNodeList()[i].gameObject.transform.Rotate(vec, -WorkSpaceRotaAngle, Space.World);
                    }
                }
            }
            else                //右旋转
            {
                for (int i = 0; i < NodesController.Instance.GetNodeList().Count; i++)
                {
                    if (InstallationState.Step1Installed == NodesController.Instance.GetNodeList()[i].GetInstallationState())
                    {
                        NodesController.Instance.GetNodeList()[i].gameObject.transform.Rotate(vec, WorkSpaceRotaAngle, Space.World);
                    }
                }

            }
        }
    }
}
