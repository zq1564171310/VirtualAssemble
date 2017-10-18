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
        public List<Node> InstalledNodeList = new List<Node>();                      //当前已经被安装完成的零件列表，按顺序放，这个集合中只可能出现两种零件，一种是从零件架上取下来的零件，一种是已经安装的零件
        private IEnumerable<Node> NextInstallNode;                   //下一步将要被安装的零件集合
        List<Node> NextInstallNodeList = new List<Node>();
        private DependencyGraph _DependencyGraph;                    //节点类实例化，用于获取下一步安装

        private float WorkSpaceScalingNum = 1;                          //工作区缩放倍数
        private float WorkSpaceRotaAngle = 90;                               //工作区旋转角度
        private float RotaAngle = 0;                                         //工作区已经旋转了多少度
        private Vector3 RotaAngleCenter = new Vector3();                     //工作区旋转的中心点
        private Vector3 ScaleCenter = new Vector3();                         //物体缩放的中心点

        private Vector3 PartStartPosition = new Vector3(1.7f, -0.4f, 4.5f);     //零件从零件架上飞下来的初始化位置

        private UIPartsPanelClass _UIPartsPanelClass;
        private UIPartsPage _UIPartsPage;

        private Button RotaLeftBut;            //向左转
        private Button RotaRightBut;           //向右转
        private Button CaptureScreensBtn;        //拍照
        private Slider _Slider;                 //放大缩小
        private Text _SliderText;               //放大缩小的文本框
        private Button _ChangeModeBtn;          //切换模式按钮
        private Button _LastNode;               //回退
        //private Text _Tips;         //提示框

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
            ScaleCenter = GameObject.Find("RuntimeObject/Nodes").transform.position;
            RotaAngleCenter = GameObject.Find("Canvas/Floor/MainWorkSpace").transform.position;
            RotaLeftBut = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/Contrarotate").GetComponent<Button>();
            RotaRightBut = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/ClockwiseRotation").GetComponent<Button>();
            _Slider = GameObject.Find("Canvas/BG/WorkAreaControl/SliderPlane/Slider").GetComponent<Slider>();
            _SliderText = GameObject.Find("Canvas/BG/WorkAreaControl/SliderPlane/SliderText").GetComponent<Text>();
            _ChangeModeBtn = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/SwitchMode").GetComponent<Button>();
            _LastNode = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/Backspace").GetComponent<Button>();
            //_Tips = GameObject.Find("Canvas/BG/PartsPanel/Tips").GetComponent<Text>();         //提示框
            _UIPartsPanelClass = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel").GetComponent<UIPartsPanelClass>();
            _UIPartsPage = GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel").GetComponent<UIPartsPage>();

            RotaLeftBut.onClick.AddListener(RotaLeftBtnClick);
            RotaRightBut.onClick.AddListener(RotaRightBtnClick);
            _ChangeModeBtn.onClick.AddListener(ChangeModeClick);
            _LastNode.onClick.AddListener(LastNodeClick);

            #region Test
            InstalledNodeList.Add(NodesController.Instance.GetNodeList()[0]);

            NextInstallNode = _DependencyGraph.GetNextSteps(InstalledNodeList[InstalledNodeList.Count - 1]).Cast<Node>();
            if (null != NextInstallNode && EntryMode.GetAssembleModel() == AssembleModel.StudyModel)   //学习模式文字提示
            {
                string err = "";
                int index = 1;
                List<Node> NextInstallNodeList = new List<Node>();
                foreach (Node node in NextInstallNode)
                {
                    node.SetInstallationState(InstallationState.NextInstalling);
                    //跳转页面
                    _UIPartsPage.SetIndex(node);
                    index = _UIPartsPage.GetIndex(node);
                    err += node.name + "(第" + index + "页）" + "/";
                    NextInstallNodeList.Add(node);
                }
                //_Tips.text = "现在应该安装:" + err;
            }
            #endregion

            //菜单控件添加事件
            _Slider.onValueChanged.AddListener(SlideTheSlider);

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 获取零件的起点位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPartStartPosition()
        {
            return PartStartPosition;
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

        #region  安装过程按钮效果的处理
        /// <summary>
        /// 回退之后，让按钮看起来能被点击
        /// </summary>
        public void AbleButtonPart(Node node)
        {
            _UIPartsPage.AbleButtonPart(node);
        }

        public void DisButtonPart(Node node)
        {
            _UIPartsPage.DisButtonPart(node);
        }

        public void AbleButton(GameObject go)
        {
            _UIPartsPage.AbleButton(go);
        }

        public void DisButton(GameObject go)
        {
            _UIPartsPage.DisButton(go);
        }
        #endregion

        public bool IsView(Node node)
        {
            return _UIPartsPage.IsView(node);
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
                    if (null != nodes.gameObject.GetComponent<MeshRenderer>())
                    {
                        nodes.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.NextInstallMate;
                    }
                    #region Test 跳转页面
                    _UIPartsPage.SetIndex(nodes);
                    index = _UIPartsPage.GetIndex(nodes);
                    #endregion
                    err += nodes.name + "(第" + index + "页）" + "/";
                    NextInstallNodeList.Add(node);
                }
                // _Tips.text = "现在应该安装:" + err;
                if (EntryMode.GetAssembleModel() == AssembleModel.ExamModel)
                {
                    // _Tips.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 滑动条缩放
        /// </summary>
        /// <param name="value"></param>
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
            if (InstalledNodeList.Count <= 1)
            {
                return;
            }

            if (InstallationState.Step1Installed == InstalledNodeList[InstalledNodeList.Count - 1].GetInstallationState())
            {
                NodesCommon.Instance.SetInstallationState(InstalledNodeList[InstalledNodeList.Count - 1].nodeId, InstallationState.NextInstalling);                //回退之前设置一下安装状态

                Destroy(GameObject.Find("RuntimeObject/Nodes/" + InstalledNodeList[InstalledNodeList.Count - 1].name + InstalledNodeList[InstalledNodeList.Count - 1].nodeId));   //如果是准备安装状态，那么还要删掉提示的物体
                Destroy(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + InstalledNodeList[InstalledNodeList.Count - 1].nodeId).gameObject);               //删掉提示的文字

                Destroy(InstalledNodeList[InstalledNodeList.Count - 1].gameObject);        //回退之前，删除已经安装的零件
                //AbleButton(InstalledNodeList[InstalledNodeList.Count - 1]);                //零件架上该零件可以被点击
                NextInstallNodeList.Add(InstalledNodeList[InstalledNodeList.Count - 1]);
                ReMoveInstalledNodeList(InstalledNodeList[InstalledNodeList.Count - 1]);    //将已经安装列表更新
            }
            else if (InstallationState.Installed == InstalledNodeList[InstalledNodeList.Count - 1].GetInstallationState())
            {
                NodesCommon.Instance.SetInstallationState(InstalledNodeList[InstalledNodeList.Count - 1].nodeId, InstallationState.Step1Installed);                //回退之前设置一下安装状态
                InstalledNodeList[InstalledNodeList.Count - 1].transform.position = PartStartPosition;
                InstalledNodeList[InstalledNodeList.Count - 1].gameObject.transform.position = PartStartPosition;
                InstalledNodeList[InstalledNodeList.Count - 1].gameObject.AddComponent<HandDraggable>();
                InstalledNodeList[InstalledNodeList.Count - 1].gameObject.GetComponent<HandDraggable>().RotationMode = HandDraggable.RotationModeEnum.LockObjectRotation;

                SetInstalledNodeListStatus(InstalledNodeList[InstalledNodeList.Count - 1], InstallationState.Step1Installed);  //集合中元素的状态也要跟着改变

                GameObject Txt = Instantiate(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text"), GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1").transform, true);                             //克隆一份文本
                Txt.name = "Text" + InstalledNodeList[InstalledNodeList.Count - 1].nodeId;
                Txt.transform.position = InstalledNodeList[InstalledNodeList.Count - 1].gameObject.transform.position;
                Txt.GetComponent<Text>().text = InstalledNodeList[InstalledNodeList.Count - 1].gameObject.name;


                GameObject gameOb = Instantiate(InstalledNodeList[InstalledNodeList.Count - 1].gameObject, InstalledNodeList[InstalledNodeList.Count - 1].gameObject.transform, true);              //克隆一份作为提示
                gameOb.name = InstalledNodeList[InstalledNodeList.Count - 1].name + InstalledNodeList[InstalledNodeList.Count - 1].nodeId;
                gameOb.transform.localScale = InstalledNodeList[InstalledNodeList.Count - 1].LocalSize;
                gameOb.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                gameOb.transform.position = InstalledNodeList[InstalledNodeList.Count - 1].EndPos;
                gameOb.transform.RotateAround(GetRotaAngleCenter(), Vector3.up, GetRotaAngle());
                if (null != gameOb.GetComponent<MeshRenderer>())
                {
                    gameOb.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;
                }
                else
                {
                    Transform[] trans = gameOb.GetComponentsInChildren<Transform>();
                    foreach (Transform tran in trans)
                    {
                        if (null != tran.gameObject.GetComponent<MeshRenderer>())
                        {
                            tran.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;
                        }
                    }
                }
                if (null != gameOb.GetComponent<MeshFilter>())
                {
                    gameOb.transform.localScale = InstalledNodeList[InstalledNodeList.Count - 1].LocalSize;
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
        /// 返回旋转的总角度
        /// </summary>
        /// <returns></returns>
        public float GetRotaAngle()
        {
            return RotaAngle;
        }
        /// <summary>
        /// 返回旋转的中心点坐标
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRotaAngleCenter()
        {
            return RotaAngleCenter;
        }

        /// <summary>
        /// 工作取缩放
        /// </summary>
        public void WorkSpaceScal(float scalNum)
        {
            //for (int i = 0; i < InstalledNodeList.Count; i++)
            //{
            //    InstalledNodeList[i].gameObject.transform.localScale = InstalledNodeList[i].gameObject.GetComponent<Node>().LocalSize * scalNum;
            //    InstalledNodeList[i].gameObject.transform.position = scalNum * (InstalledNodeList[i].gameObject.transform.position - ScaleCenter) + ScaleCenter;
            //}

            //for (int i = 0; i < NodesCommon.Instance.GetNodesList().Count; i++)
            //{
            //    NodesCommon.Instance.GetNodesList()[i].EndPos = scalNum * (NodesCommon.Instance.GetNodesList()[i].EndPos - ScaleCenter) + ScaleCenter;
            //    NodesCommon.Instance.GetNodesList()[i].LocalSize = NodesCommon.Instance.GetNodesList()[i].LocalSize * scalNum;
            //}
        }

        /// <summary>
        /// 工作区旋转
        /// </summary>
        /// <param name="rota"></param>
        public void WorkSpaceRota(int rota)
        {
            //获取工作区中心位置
            if (0 == rota)        //左旋转
            {
                for (int i = 0; i < InstalledNodeList.Count; i++)
                {
                    InstalledNodeList[i].gameObject.transform.RotateAround(RotaAngleCenter, Vector3.up, -WorkSpaceRotaAngle);
                }

                for (int i = 0; i < NodesCommon.Instance.GetNodesList().Count; i++)
                {
                    NodesCommon.Instance.GetNodesList()[i].EndPos = Quaternion.AngleAxis(-WorkSpaceRotaAngle, Vector3.up) * (NodesCommon.Instance.GetNodesList()[i].EndPos - RotaAngleCenter) + RotaAngleCenter;
                }

                RotaAngle -= WorkSpaceRotaAngle;
            }
            else                //右旋转
            {
                for (int i = 0; i < InstalledNodeList.Count; i++)
                {
                    InstalledNodeList[i].gameObject.transform.RotateAround(RotaAngleCenter, Vector3.up, WorkSpaceRotaAngle);
                }

                for (int i = 0; i < NodesCommon.Instance.GetNodesList().Count; i++)
                {
                    NodesCommon.Instance.GetNodesList()[i].EndPos = Quaternion.AngleAxis(WorkSpaceRotaAngle, Vector3.up) * (NodesCommon.Instance.GetNodesList()[i].EndPos - RotaAngleCenter) + RotaAngleCenter;
                }

                RotaAngle += WorkSpaceRotaAngle;
            }
        }

    }
}
