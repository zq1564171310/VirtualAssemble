/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI 开始界面
/// </summary>
namespace WyzLink.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.Manager;

    public class StartUI : MonoBehaviour
    {
        private GameObject _Canvas;                          //零件架等UI的画布
        private GameObject _InitCanvas;                      //初始化UI画布
        private GameObject _TipCanvas;                       //错误提示画布
        private GameObject _TipErrBtn;

        private GameObject _RootPartGameObject;              //LCD1物体

        private Button _RestartBtn;                          //重新开始按钮

        private AddPartsManager _AddPartsManager;            //零件初始化
        private AddToolsManager _AddToolsManager;            //工具初始化
        private AddCommonPartsManager _AddCommonPartsManager;  //零件架初始化

        private OnReceivedTools _OnReceivedTools;
        private UIToolsClassPanel _UIToolsClassPanel;

        private UIPartsPanelClass _UIPartsPanelClass;        //零件架初始化
        private UIPartsPage _UIPartsPage;

        private UICommonClass _UICommonClass;        //零件架初始化
        private UICommonParts _UICommonParts;

        // Use this for initialization
        void Start()
        {
            _Canvas = GameObject.Find("Canvas");
            _InitCanvas = GameObject.Find("InitCanvas");
            _TipCanvas = GameObject.Find("TipsCanvas");
            _TipErrBtn = GameObject.Find("TipsCanvas/ErrorBack");

            _RootPartGameObject = FindObjectOfType<AssembleController>().gameObject;

            _RestartBtn = GameObject.Find("InitCanvas/BG/RestartBtn").GetComponent<Button>();

            _AddPartsManager = GameObject.Find("RuntimeObject").GetComponent<AddPartsManager>();
            _AddToolsManager = GameObject.Find("RuntimeObject").GetComponent<AddToolsManager>();
            _AddCommonPartsManager = GameObject.Find("RuntimeObject").GetComponent<AddCommonPartsManager>();

            _UIPartsPanelClass = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel").GetComponent<UIPartsPanelClass>();
            _UIPartsPage = GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel").GetComponent<UIPartsPage>();

            _UIToolsClassPanel = GameObject.Find("Canvas/BG/ToolsPanel/ToolsClassPanel").GetComponent<UIToolsClassPanel>();
            _OnReceivedTools = GameObject.Find("Canvas/BG/ToolsPanel/SingleToolPanel").GetComponent<OnReceivedTools>();

            _UICommonClass = GameObject.Find("Canvas/BG/CommonPartsPanel/ClassPanel").GetComponent<UICommonClass>();
            _UICommonParts = GameObject.Find("Canvas/BG/CommonPartsPanel/PartPanel").GetComponent<UICommonParts>();

            _TipCanvas.SetActive(false);                         //初始化隐藏错误提示界面

            _Canvas.SetActive(false);                            //初始化隐藏零件架等UI

            if (null != _RootPartGameObject)                      //将整个LCD1机器隐藏
            {
                foreach (Transform tran in _RootPartGameObject.transform)
                {
                    tran.gameObject.SetActive(false);
                }
            }

            _InitCanvas.SetActive(true);                         //初始化画布显示

            _RestartBtn.onClick.AddListener(RestartBtnOnClick);
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 重新开始按钮的点击事件
        /// </summary>
        private void RestartBtnOnClick()
        {
            _InitCanvas.SetActive(false);                         //初始化画布隐藏

            if (null != _RootPartGameObject)                      //显示LCD1
            {
                foreach (Transform tran in _RootPartGameObject.transform)
                {
                    tran.gameObject.SetActive(true);
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

            AssembleManager.Instance.GetTipCanvas(_TipCanvas);    //提示框被隐藏了，所以初始化的时候，把被隐藏的物体赋值，否则后续将无法调用该物体
            AssembleManager.Instance.GetTipErrBtn(_TipErrBtn);
            AssembleManager.Instance.Init();                    //安装处理类初始化
        }
    }
}
