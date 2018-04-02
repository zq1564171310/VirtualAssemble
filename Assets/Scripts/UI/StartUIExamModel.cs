/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI 开始界面
/// </summary>
namespace WyzLink.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.LogicManager;

    public class StartUIExamModel : MonoBehaviour, StartUI
    {
        private GameObject _Canvas;                          //零件架等UI的画布
        private GameObject _InitCanvas;                      //初始化UI画布

        private GameObject _TipCanvas;                       //提示画布
        private GameObject _TipErrorPlane;                      //错误提示画布
        private GameObject _TipErrBtn;                       //错误提示的确认按钮
        private GameObject _PartsInfoPlane;                  //零件信息提示画布
        private GameObject _PartsInfoText;                   //零件信息提示文本
        private GameObject _PartsInfoBtn;

        private GameObject _RootPartGameObject;              //LCD1物体

        private Button _RestartBtn;                          //重新开始按钮
        private Button _RecordStart;                         //读取存档按钮

        private AddPartsManagerExamModel _AddPartsManager;            //零件初始化
        private AddToolsManager _AddToolsManager;            //工具初始化
        private AddCommonPartsManager _AddCommonPartsManager;  //零件架初始化

        private OnReceivedTools _OnReceivedTools;
        private UIToolsClassPanel _UIToolsClassPanel;

        private UIPartsPanelClassExamModel _UIPartsPanelClass;        //零件架初始化
        private UIPartsPageExamModel _UIPartsPage;

        private UICommonClass _UICommonClass;        //零件架初始化
        private UICommonParts _UICommonParts;

        // Use this for initialization
        void Start()
        {
            _Canvas = GameObject.Find("Canvas");
            _InitCanvas = GameObject.Find("InitCanvas");
            _TipCanvas = GameObject.Find("TipsCanvas");
            _TipErrorPlane = GameObject.Find("TipsCanvas/ErrorPlane");
            _TipErrBtn = GameObject.Find("TipsCanvas/ErrorPlane/ErrorBack");
            _PartsInfoPlane = GameObject.Find("TipsCanvas/PartsInfoPlane");
            _PartsInfoText = GameObject.Find("TipsCanvas/PartsInfoPlane/InfoContent");
            _PartsInfoBtn = GameObject.Find("TipsCanvas/PartsInfoPlane/PartsBtn");

            _RootPartGameObject = FindObjectOfType<AssembleController>().gameObject;

            _RestartBtn = GameObject.Find("InitCanvas/BG/RestartBtn").GetComponent<Button>();
            _RecordStart = GameObject.Find("InitCanvas/BG/RecordStart").GetComponent<Button>();

            _AddPartsManager = GameObject.Find("RuntimeObject").GetComponent<AddPartsManagerExamModel>();
            _AddToolsManager = GameObject.Find("RuntimeObject").GetComponent<AddToolsManager>();
            _AddCommonPartsManager = GameObject.Find("RuntimeObject").GetComponent<AddCommonPartsManager>();

            _UIPartsPanelClass = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel").GetComponent<UIPartsPanelClassExamModel>();
            _UIPartsPage = GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel").GetComponent<UIPartsPageExamModel>();

            _UIToolsClassPanel = GameObject.Find("Canvas/BG/ToolsPanel/ToolsClassPanel").GetComponent<UIToolsClassPanel>();
            _OnReceivedTools = GameObject.Find("Canvas/BG/ToolsPanel/SingleToolPanel").GetComponent<OnReceivedTools>();

            _UICommonClass = GameObject.Find("Canvas/BG/CommonPartsPanel/ClassPanel").GetComponent<UICommonClass>();
            _UICommonParts = GameObject.Find("Canvas/BG/CommonPartsPanel/PartPanel").GetComponent<UICommonParts>();

            foreach (Transform tran in _TipCanvas.transform)                      //初始化隐藏所有提示（错误提示和信息提示）
            {
                tran.gameObject.SetActive(false);
            }

            _Canvas.SetActive(false);                            //初始化隐藏零件架等UI

            if (null != _RootPartGameObject)                      //将整个LCD1机器隐藏
            {
                foreach (Transform tran in _RootPartGameObject.transform)
                {
                    tran.gameObject.SetActive(false);
                }
            }

            _InitCanvas.SetActive(true);                         //初始化画布显示

            _RestartBtn.onClick.AddListener(Restart);
            _RecordStart.onClick.AddListener(Record);
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 重新开始按钮的点击事件
        /// </summary>
        public void Restart()
        {
            PlayerPrefs.DeleteAll();
            _InitCanvas.SetActive(false);                         //初始化画布隐藏

            if (null != _RootPartGameObject)                      //显示LCD1
            {
                foreach (Transform tran in _RootPartGameObject.transform)
                {
                    if ("NoSee" != tran.gameObject.tag)
                    {
                        tran.gameObject.SetActive(true);
                    }
                }
            }
            _Canvas.SetActive(true);                            //显示零件架等UI

            _AddPartsManager.Init();                            //零件集合初始化
            _AddToolsManager.Init();                            //工具集合初始化
            _AddCommonPartsManager.Init();                      //常用零件集合

            _UIPartsPage.Init();
            _UIPartsPanelClass.Init();                          //零件架数据初始化

            _OnReceivedTools.Init();
            _UIToolsClassPanel.Init();

            _UICommonParts.Init();
            _UICommonClass.Init();

            AssembleManagerExamModel.Instance.GetTipCanvas(_TipErrorPlane);    //提示框被隐藏了，所以初始化的时候，把被隐藏的物体赋值，否则后续将无法调用该物体
            AssembleManagerExamModel.Instance.GetTipErrBtn(_TipErrBtn);

            AssembleManagerExamModel.Instance.GetPartsInfoPlane(_PartsInfoPlane);
            AssembleManagerExamModel.Instance.GetPartsInfoText(_PartsInfoText);
            AssembleManagerExamModel.Instance.GetPartsInfoBtn(_PartsInfoBtn);

            AssembleManagerExamModel.Instance.Init();                    //安装处理类初始化
        }

        /// <summary>
        /// 读取存档点击按钮
        /// </summary>
        public void Record()
        {
            _InitCanvas.SetActive(false);                         //初始化画布隐藏

            if (null != _RootPartGameObject)                      //显示LCD1
            {
                foreach (Transform tran in _RootPartGameObject.transform)
                {
                    if ("NoSee" != tran.gameObject.tag)
                    {
                        tran.gameObject.SetActive(true);
                    }
                }
            }
            _Canvas.SetActive(true);                            //显示零件架等UI

            _AddPartsManager.Init();                            //零件集合初始化
            _AddToolsManager.Init();                            //工具集合初始化
            _AddCommonPartsManager.Init();                      //常用零件集合

            _UIPartsPage.Init();
            _UIPartsPanelClass.Init();                          //零件架数据初始化

            _OnReceivedTools.Init();
            _UIToolsClassPanel.Init();

            _UICommonParts.Init();
            _UICommonClass.Init();

            AssembleManagerExamModel.Instance.GetTipCanvas(_TipErrorPlane);    //提示框被隐藏了，所以初始化的时候，把被隐藏的物体赋值，否则后续将无法调用该物体
            AssembleManagerExamModel.Instance.GetTipErrBtn(_TipErrBtn);

            AssembleManagerExamModel.Instance.GetPartsInfoPlane(_PartsInfoPlane);
            AssembleManagerExamModel.Instance.GetPartsInfoText(_PartsInfoText);
            AssembleManagerExamModel.Instance.GetPartsInfoBtn(_PartsInfoBtn);

            AssembleManagerExamModel.Instance.Init();                    //安装处理类初始化
        }
    }
}
