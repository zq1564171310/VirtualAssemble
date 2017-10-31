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

        private float WorkSpaceScalingNum = 1;                               //工作区缩放倍数
        private float WorkSpaceRotaAngle = 90;                               //工作区旋转角度
        private float RotaAngle = 0;                                         //工作区已经旋转了多少度
        private Vector3 RotaAngleCenter = new Vector3();                     //工作区旋转的中心点
        private Vector3 ScaleCenter = new Vector3();                         //物体缩放的中心点

        private Vector3 PartStartPosition = new Vector3(1.7f, -0.4f, 4.5f);     //零件从零件架上飞下来的初始化位置

        private UIPartsPanelClass _UIPartsPanelClass;
        private UIPartsPage _UIPartsPage;

        private Button RotaLeftBut;              //向左转
        private Button RotaRightBut;             //向右转
        private Button CaptureScreensBtn;        //拍照
        private Slider _Slider;                  //放大缩小
        private Text _SliderText;                //放大缩小的文本框
        private Button _ChangeModeBtn;           //切换模式按钮
        private Button _LastNode;                //回退
        private Button _Skip;                    //跳过

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
            ScaleCenter = FindObjectOfType<AssembleController>().transform.position;
            RotaAngleCenter = FindObjectOfType<AssembleController>().transform.position;

            RotaLeftBut = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/Contrarotate").GetComponent<Button>();
            RotaRightBut = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/ClockwiseRotation").GetComponent<Button>();
            _Slider = GameObject.Find("Canvas/BG/WorkAreaControl/SliderPlane/Slider").GetComponent<Slider>();
            _SliderText = GameObject.Find("Canvas/BG/WorkAreaControl/SliderPlane/SliderText").GetComponent<Text>();
            _ChangeModeBtn = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/SwitchMode").GetComponent<Button>();
            _LastNode = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/Backspace").GetComponent<Button>();
            if (AssembleModel.StudyModel == EntryMode.GetAssembleModel())
            {
                _Skip = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/Skip").GetComponent<Button>();
            }

            _UIPartsPanelClass = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel").GetComponent<UIPartsPanelClass>();
            _UIPartsPage = GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel").GetComponent<UIPartsPage>();

            RotaLeftBut.onClick.AddListener(RotaLeftBtnClick);
            RotaRightBut.onClick.AddListener(RotaRightBtnClick);
            _ChangeModeBtn.onClick.AddListener(ChangeModeClick);
            _LastNode.onClick.AddListener(LastNodeClick);
            if (AssembleModel.StudyModel == EntryMode.GetAssembleModel())
            {
                _Skip.onClick.AddListener(SkipClick);
            }
            #region Test
            InstalledNodeList.Add(NodesController.Instance.GetNodeList()[0]);

            NextInstallNode = _DependencyGraph.GetNextSteps(InstalledNodeList[InstalledNodeList.Count - 1]).Cast<Node>();
            if (null != NextInstallNode && EntryMode.GetAssembleModel() == AssembleModel.StudyModel)   //学习模式文字提示
            {
                string err = "";
                int index = 1;
                foreach (Node node in NextInstallNode)
                {
                    node.SetInstallationState(InstallationState.NextInstalling);
                    //跳转页面
                    //_UIPartsPage.SetIndex(node);
                    index = _UIPartsPage.GetIndex(node);
                    err += node.name + "(第" + index + "页）" + "/";
                    NextInstallNodeList.Add(node);
                }
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

        /// <summary>
        /// 设置零件信息框的内容
        /// </summary>
        /// <param name="value"></param>
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
        public void AbleButton(GameObject go)
        {
            if (null != _UIPartsPage)
            {
                _UIPartsPage.AbleButton(go);
            }
        }

        public void DisButton(GameObject go)
        {
            if (null != _UIPartsPage)
            {
                _UIPartsPage.DisButton(go);
            }
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
                    #region Test 跳转页面
                    //_UIPartsPage.SetIndex(nodes);
                    index = _UIPartsPage.GetIndex(nodes);
                    #endregion
                    err += nodes.name + "(第" + index + "页）" + "/";
                    NextInstallNodeList.Add(node);
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
                NextInstallNodeList.Add(InstalledNodeList[InstalledNodeList.Count - 1]);
                ReMoveInstalledNodeList(InstalledNodeList[InstalledNodeList.Count - 1]);    //将已经安装列表更新
            }
            else if (InstallationState.Installed == InstalledNodeList[InstalledNodeList.Count - 1].GetInstallationState())
            {
                NodesCommon.Instance.SetInstallationState(InstalledNodeList[InstalledNodeList.Count - 1].nodeId, InstallationState.Step1Installed);                //回退之前设置一下安装状态
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
                gameOb.transform.localScale = InstalledNodeList[InstalledNodeList.Count - 1].LocalSize * GetScale();
            }
        }

        /// <summary>
        /// 跳过
        /// </summary>
        public void SkipClick()
        {
            if (InstallationState.Step1Installed == InstalledNodeList[InstalledNodeList.Count - 1].GetInstallationState())   //已安装序列中有零件是从零件架上取下来，但是没安装的，这时候，应该让这个零件被安装Step1Installed-->Installed，即跳过手动拖拽安装
            {
                NodesCommon.Instance.SetInstallationState(InstalledNodeList[InstalledNodeList.Count - 1].nodeId, InstallationState.Installed);              //跳过之前设置一下安装状态
                SetInstalledNodeListStatus(InstalledNodeList[InstalledNodeList.Count - 1], InstallationState.Installed);  //集合中元素的状态也要跟着改变

                InstalledNodeList[InstalledNodeList.Count - 1].gameObject.transform.position = InstalledNodeList[InstalledNodeList.Count - 1].EndPos;                                    //移动到安装位置
                Destroy(InstalledNodeList[InstalledNodeList.Count - 1].gameObject.GetComponent<HandDraggable>());
                InstalledNodeList[InstalledNodeList.Count - 1].gameObject.GetComponent<Node>().PlayAnimations();
                Destroy(InstalledNodeList[InstalledNodeList.Count - 1].gameObject.GetComponent<BoxCollider>());
                InstalledNodeList[InstalledNodeList.Count - 1].gameObject.AddComponent<MeshCollider>();

                Destroy(GameObject.Find("RuntimeObject/Nodes/" + InstalledNodeList[InstalledNodeList.Count - 1].name + InstalledNodeList[InstalledNodeList.Count - 1].nodeId));   //如果是准备安装状态，那么还要删掉提示的物体
                Destroy(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + InstalledNodeList[InstalledNodeList.Count - 1].nodeId).gameObject);                   //删掉提示的文字

                bool installatFlag = false;
                foreach (Node node in NodesController.Instance.GetNodeList())
                {
                    if (InstallationState.NextInstalling == node.GetInstallationState() || InstallationState.Step1Installed == node.GetInstallationState())
                    {
                        installatFlag = true;
                        break;
                    }
                }

                if (false == installatFlag)            //说明这一步所有的零件都已经被安装了，那么该下一步了
                {
                    NextInstall(InstalledNodeList[InstalledNodeList.Count - 1].gameObject.GetComponent<Node>());
                }
            }
            else       //说明没有零件从零件架上取下来，那么应该判断下一步安装的零件列表中是否存在零件，如果存在自动取下一个零件，让该零件进入已安装序列，并且安装状态置为待安装NextInstalling-->Step1Installed
            {
                foreach (Node nodeContr in NodesController.Instance.GetNodeList())
                {
                    if (InstallationState.NextInstalling == nodeContr.GetInstallationState())
                    {
                        NodesCommon.Instance.SetInstallationState(nodeContr.nodeId, InstallationState.Step1Installed);                //跳过之前设置一下安装状态
                        GameObject gameobj;                         //克隆一份,作为安装的零件
                        gameobj = Instantiate(nodeContr.gameObject, nodeContr.gameObject.transform, true);
                        gameobj.name = nodeContr.name;
                        gameobj.SetActive(true);                       //可能没有翻到那一页，需要显示一下
                        gameobj.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                        InstalledNodeList.Add(gameobj.GetComponent<Node>());
                        SetInstalledNodeListStatus(InstalledNodeList[InstalledNodeList.Count - 1], InstallationState.Step1Installed);  //集合中元素的状态也要跟着改变
                        gameobj.transform.localScale = gameobj.GetComponent<Node>().LocalSize * GetScale();
                        gameobj.transform.RotateAround(GetRotaAngleCenter(), Vector3.up, GetRotaAngle());
                        gameobj.AddComponent<NodeManager>();
                        gameobj.AddComponent<BoxCollider>();
                        if (null == gameobj.gameObject.GetComponent<MeshFilter>())
                        {
                            gameobj.GetComponent<BoxCollider>().size /= 10;
                        }
                        InstalledNodeList[InstalledNodeList.Count - 1].gameObject.transform.position = PartStartPosition;
                        InstalledNodeList[InstalledNodeList.Count - 1].gameObject.AddComponent<HandDraggable>();
                        InstalledNodeList[InstalledNodeList.Count - 1].gameObject.GetComponent<HandDraggable>().RotationMode = HandDraggable.RotationModeEnum.LockObjectRotation;

                        GameObject Txt = Instantiate(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text"), GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1").transform, true);                             //克隆一份文本
                        Txt.name = "Text" + InstalledNodeList[InstalledNodeList.Count - 1].nodeId;
                        Txt.transform.position = InstalledNodeList[InstalledNodeList.Count - 1].gameObject.transform.position;
                        Txt.GetComponent<Text>().text = InstalledNodeList[InstalledNodeList.Count - 1].gameObject.name;

                        GameObject gameOb = Instantiate(gameobj, gameobj.transform, true);              //克隆一份作为提示
                        gameOb.name = gameobj.GetComponent<Node>().name + gameobj.GetComponent<Node>().nodeId;
                        gameOb.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                        gameOb.transform.position = gameobj.GetComponent<Node>().EndPos;

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
                        gameOb.transform.localScale = InstalledNodeList[InstalledNodeList.Count - 1].LocalSize * GetScale();
                        //gameOb.transform.Rotate(Vector3.up, -GetRotaAngle(), Space.Self);  //拷贝的时候，已经自转过了
                        break;
                    }
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
        /// 返回缩放比例
        /// </summary>
        /// <returns></returns>
        public float GetScale()
        {
            float scalNum = 1;
            scalNum = WorkSpaceScalingNum;
            return scalNum;
        }

        /// <summary>
        /// 工作区缩放
        /// </summary>
        public void WorkSpaceScal(float scalNum)
        {
            for (int i = 1; i < NodesCommon.Instance.GetNodesList().Count; i++)
            {
                NodesCommon.Instance.GetNodesList()[i].EndPos = scalNum * (NodesCommon.Instance.GetNodesList()[i].EndPosForScale - ScaleCenter) + ScaleCenter;
            }

            for (int i = 0; i < InstalledNodeList.Count; i++)
            {
                InstalledNodeList[i].gameObject.transform.localScale = InstalledNodeList[i].LocalSize * scalNum;
                InstalledNodeList[i].EndPos = scalNum * (InstalledNodeList[i].EndPosForScale - ScaleCenter) + ScaleCenter;

                if (InstallationState.Installed != InstalledNodeList[i].GetInstallationState())
                {
                    GameObject.Find(InstalledNodeList[i].name + InstalledNodeList[i].nodeId).transform.localScale = InstalledNodeList[i].LocalSize * scalNum;
                    GameObject.Find(InstalledNodeList[i].name + InstalledNodeList[i].nodeId).transform.position = InstalledNodeList[i].EndPos;
                    InstalledNodeList[i].gameObject.transform.position = PartStartPosition;
                }
                else
                {
                    InstalledNodeList[i].gameObject.transform.position = InstalledNodeList[i].EndPos;
                }
            }
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
                for (int i = 1; i < NodesCommon.Instance.GetNodesList().Count; i++)   //因为第一个物体是底座，所以他不在零件架上，直接出现在安装区域，所以应该pas
                {
                    NodesCommon.Instance.GetNodesList()[i].EndPos = Quaternion.AngleAxis(-WorkSpaceRotaAngle, Vector3.up) * (NodesCommon.Instance.GetNodesList()[i].EndPos - RotaAngleCenter) + RotaAngleCenter;
                    NodesCommon.Instance.GetNodesList()[i].EndPosForScale = Quaternion.AngleAxis(-WorkSpaceRotaAngle, Vector3.up) * (NodesCommon.Instance.GetNodesList()[i].EndPosForScale - RotaAngleCenter) + RotaAngleCenter;
                }

                RotaAngle -= WorkSpaceRotaAngle;

                for (int i = 0; i < InstalledNodeList.Count; i++)
                {
                    InstalledNodeList[i].EndPos = Quaternion.AngleAxis(-WorkSpaceRotaAngle, Vector3.up) * (InstalledNodeList[i].EndPos - RotaAngleCenter) + RotaAngleCenter;
                    InstalledNodeList[i].EndPosForScale = Quaternion.AngleAxis(-WorkSpaceRotaAngle, Vector3.up) * (InstalledNodeList[i].EndPosForScale - RotaAngleCenter) + RotaAngleCenter;

                    InstalledNodeList[i].gameObject.transform.RotateAround(RotaAngleCenter, Vector3.up, -WorkSpaceRotaAngle);

                    if (InstallationState.Installed != InstalledNodeList[i].GetInstallationState())   //待安装的零件提示位置和终点位置跟着转
                    {
                        GameObject.Find(InstalledNodeList[i].name + InstalledNodeList[i].nodeId).transform.RotateAround(RotaAngleCenter, Vector3.up, -WorkSpaceRotaAngle);
                        InstalledNodeList[i].transform.position = PartStartPosition;
                    }
                }
            }
            else                //右旋转
            {
                for (int i = 1; i < NodesCommon.Instance.GetNodesList().Count; i++)
                {
                    NodesCommon.Instance.GetNodesList()[i].EndPos = Quaternion.AngleAxis(WorkSpaceRotaAngle, Vector3.up) * (NodesCommon.Instance.GetNodesList()[i].EndPos - RotaAngleCenter) + RotaAngleCenter;
                    NodesCommon.Instance.GetNodesList()[i].EndPosForScale = Quaternion.AngleAxis(WorkSpaceRotaAngle, Vector3.up) * (NodesCommon.Instance.GetNodesList()[i].EndPosForScale - RotaAngleCenter) + RotaAngleCenter;
                }

                RotaAngle += WorkSpaceRotaAngle;

                for (int i = 0; i < InstalledNodeList.Count; i++)
                {
                    InstalledNodeList[i].EndPos = Quaternion.AngleAxis(WorkSpaceRotaAngle, Vector3.up) * (InstalledNodeList[i].EndPos - RotaAngleCenter) + RotaAngleCenter;
                    InstalledNodeList[i].EndPosForScale = Quaternion.AngleAxis(WorkSpaceRotaAngle, Vector3.up) * (InstalledNodeList[i].EndPosForScale - RotaAngleCenter) + RotaAngleCenter;

                    InstalledNodeList[i].gameObject.transform.RotateAround(RotaAngleCenter, Vector3.up, WorkSpaceRotaAngle);

                    if (InstallationState.Installed != InstalledNodeList[i].GetInstallationState()) //待安装的零件提示位置和终点位置跟着转
                    {
                        GameObject.Find(InstalledNodeList[i].gameObject.GetComponent<Node>().name + InstalledNodeList[i].gameObject.GetComponent<Node>().nodeId).transform.RotateAround(RotaAngleCenter, Vector3.up, WorkSpaceRotaAngle);
                        InstalledNodeList[i].transform.position = PartStartPosition;
                    }
                }
            }
        }

    }
}
