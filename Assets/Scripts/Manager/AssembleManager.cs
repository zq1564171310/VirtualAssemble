/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
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

    public class AssembleManager : Singleton<AssembleManager>
    {
        public List<Node> InstalledNodeList = new List<Node>();                      //当前已经被安装完成的零件列表，按顺序放
        private IEnumerable<Node> NextInstallNode;                   //下一步将要被安装的零件集合
        private DependencyGraph _DependencyGraph;                    //节点类实例化，用于获取下一步安装
        private float WorkSpaceScalingNum = 1;                          //工作区缩放倍数
        private float WorkSpaceRotaAngle = 90;                               //工作区旋转角度

        private UIPartsPanelClass _UIPartsPanelClass;
        private UIPartsPage _UIPartsPage;

        private Button RotaLeftBut;            //向左转
        private Button RotaRightBut;           //向右转
        private Button CaptureScreensBtn;        //拍照
        private Slider _Slider;                 //放大缩小
        private Text _SliderText;               //放大缩小的文本框
        private Button _ChangeModeBtn;          //切换模式按钮
        private Button _LastNode;               //回退
        private Text _Tips;         //提示框

        private GameObject _TipErrorPlane;                       //错误提示画布
        private GameObject _TipErrBtn;                           //错误提示的确认按钮
        private GameObject _PartsInfoPlane;                      //零件信息提示画布
        private GameObject _PartsInfoText;                       //零件信息提示文本
        private GameObject _PartInfoBtn;                         //零件信息提示关闭按钮

        // Use this for initialization
        void Start()
        {

        }

        public void Init()
        {
            RotaLeftBut = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/Contrarotate").GetComponent<Button>();
            RotaRightBut = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/ClockwiseRotation").GetComponent<Button>();
            _Slider = GameObject.Find("Canvas/BG/WorkAreaControl/SliderPlane/Slider").GetComponent<Slider>();
            _SliderText = GameObject.Find("Canvas/BG/WorkAreaControl/SliderPlane/SliderText").GetComponent<Text>();
            _ChangeModeBtn = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/SwitchMode").GetComponent<Button>();
            _LastNode = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/Backspace").GetComponent<Button>();
            _Tips = GameObject.Find("Canvas/BG/PartsPanel/Tips").GetComponent<Text>();         //提示框

            RotaLeftBut.onClick.AddListener(RotaLeftBtnClick);
            RotaRightBut.onClick.AddListener(RotaRightBtnClick);
            _ChangeModeBtn.onClick.AddListener(ChangeModeClick);
            _LastNode.onClick.AddListener(LastNodeClick);

            _UIPartsPanelClass = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel").GetComponent<UIPartsPanelClass>();
            _UIPartsPage = GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel").GetComponent<UIPartsPage>();

            #region Test
            InstalledNodeList.Add(NodesController.Instance.GetNodeList()[0]);

            NextInstallNode = _DependencyGraph.GetNextSteps(InstalledNodeList[InstalledNodeList.Count - 1]).Cast<Node>();
            if (null != NextInstallNode && EntryMode.GeAssembleModel() != AssembleModel.ExamModel)
            {
                string err = "";
                int index = 1;
                foreach (Node node in NextInstallNode)
                {
                    node.SetInstallationState(InstallationState.NextInstalling);
                    //跳转页面
                    _UIPartsPage.SetIndex(node);
                    index = _UIPartsPage.GetIndex(node);
                    err += node.name + "(第" + index + "页）" + "/";
                }
                _Tips.text = "现在应该安装:" + err;
                if (EntryMode.GeAssembleModel() == AssembleModel.ExamModel)
                {
                    _Tips.gameObject.SetActive(false);
                }
            }
            #endregion
            //获取物体的绝对路径，新的UI中都会改掉
            _Slider.onValueChanged.AddListener(SlideTheSlider);
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
        /// 设置当前已经安装的零件
        /// </summary>
        public void AddInstalledNodeList(Node installedNode)
        {
            InstalledNodeList.Add(installedNode);
        }

        public void ReMoveInstalledNodeList(Node installedNode)
        {
            InstalledNodeList.Remove(installedNode);
        }

        /// <summary>
        /// 获取错误提示框的物体
        /// </summary>
        /// <param name="go"></param>
        public void GetTipCanvas(GameObject go)
        {
            _TipErrorPlane = go;
        }

        /// <summary>
        /// 获取错误提示框的确认按钮物体
        /// </summary>
        /// <param name="go"></param>
        public void GetTipErrBtn(GameObject go)
        {
            _TipErrBtn = go;
        }

        public void GetPartsInfoPlane(GameObject go)
        {
            _PartsInfoPlane = go;
        }

        public void GetPartsInfoText(GameObject go)
        {
            _PartsInfoText = go;
        }

        public void GetPartsInfoBtn(GameObject go)
        {
            _PartInfoBtn = go;
        }

        /// <summary>
        /// 设置错误提示框的显示状态
        /// </summary>
        /// <param name="status"></param>
        public void SetTipCanvasStatus(bool status)
        {
            _TipErrorPlane.SetActive(status);
        }

        public void SetPartsInfoPlane(bool status)
        {
            _PartsInfoPlane.SetActive(status);
        }

        public void SetPartsInfoTextValue(string value)
        {
            _PartsInfoText.GetComponent<Text>().text = value;
        }

        /// <summary>
        /// 提供获取错误提示的确认按钮方法
        /// </summary>
        /// <returns></returns>
        public Button GetTipErrBtn()
        {
            return _TipErrBtn.GetComponent<Button>();
        }

        public Button GetPartsInfoBtn()
        {
            return _PartInfoBtn.GetComponent<Button>();
        }

        /// <summary>
        /// 回退之后，让按钮看起来能被点击
        /// </summary>
        public void AbleButton(Node node)
        {
            _UIPartsPage.AbleButtonPart(node);
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
                    _UIPartsPage.SetIndex(nodes);
                    index = _UIPartsPage.GetIndex(nodes);
                    #endregion
                    err += nodes.name + "(第" + index + "页）" + "/";
                }
                _Tips.text = "现在应该安装:" + err;
                if (EntryMode.GeAssembleModel() == AssembleModel.ExamModel)
                {
                    _Tips.gameObject.SetActive(false);
                }
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
            _SliderText.text = "工作区缩放" + Num + "%";
        }

        public void RotaLeftBtnClick()
        {
            WorkSpaceRota(0);
        }

        public void RotaRightBtnClick()
        {
            WorkSpaceRota(1);
        }

        public void ChangeModeClick()
        {
            EntryMode.SetAssembleModel(AssembleModel.DemonstrationModel);
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// 回退
        /// </summary>
        public void LastNodeClick()
        {
            NodesCommon.Instance.SetInstallationState(InstalledNodeList[InstalledNodeList.Count - 1].nodeId, InstallationState.NotInstalled);                //回退之前设置一下安装状态
            Destroy(InstalledNodeList[InstalledNodeList.Count - 1].gameObject);        //回退之前，删除已经安装的零件
            AbleButton(InstalledNodeList[InstalledNodeList.Count - 1]);                //零件架上该零件可以被点击
            ReMoveInstalledNodeList(InstalledNodeList[InstalledNodeList.Count - 1]);    //将已经安装列表更新
            NextInstallNode = _DependencyGraph.GetNextSteps(InstalledNodeList[InstalledNodeList.Count - 1]).Cast<Node>();   //更新当前应该被安装的零件列表
            if (null != NextInstallNode && EntryMode.GeAssembleModel() != AssembleModel.ExamModel)
            {
                string err = "";
                int index = 1;
                foreach (Node node in NextInstallNode)
                {
                    node.SetInstallationState(InstallationState.NextInstalling);
                    //跳转页面
                    _UIPartsPage.SetIndex(node);
                    index = _UIPartsPage.GetIndex(node);
                    err += node.name + "(第" + index + "页）" + "/";
                }
                _Tips.text = "现在应该安装:" + err;
                if (EntryMode.GeAssembleModel() == AssembleModel.ExamModel)
                {
                    _Tips.gameObject.SetActive(false);
                }
            }
        }

        public void CaptureScreensBtnClick()
        {
            //#if !NETFX_CORE
            //            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/Test.png");
            //#else
            //                        ScreenCapture.CaptureScreenshot(Windows.Storage.KnownFolders.PicturesLibrary + "/Test.png");
            //#endif
        }

        /// <summary>
        /// 工作取缩放
        /// </summary>
        public void WorkSpaceScal(float scalNum)
        {
            for (int i = 0; i < InstalledNodeList.Count; i++)
            {
                InstalledNodeList[i].gameObject.transform.localScale = InstalledNodeList[i].gameObject.GetComponent<Node>().LocalSize * scalNum;
            }
        }

        /// <summary>
        /// 工作区旋转
        /// </summary>
        /// <param name="rota"></param>
        public void WorkSpaceRota(int rota)
        {
            //获取工作区中心位置
            Vector3 vec = GameObject.Find("Canvas/Floor/MainWorkSpace").transform.position;
            if (0 == rota)        //左旋转
            {
                for (int i = 0; i < InstalledNodeList.Count; i++)
                {
                    InstalledNodeList[i].gameObject.transform.RotateAround(vec, Vector3.up, -WorkSpaceRotaAngle);
                }
            }
            else                //右旋转
            {
                for (int i = 0; i < InstalledNodeList.Count; i++)
                {
                    InstalledNodeList[i].gameObject.transform.RotateAround(vec, Vector3.up, WorkSpaceRotaAngle);
                }
            }
        }

    }
}
