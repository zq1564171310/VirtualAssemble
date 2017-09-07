/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI 开始界面
/// </summary>
namespace WyzLink.Manager
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;

    public class StartUI : MonoBehaviour
    {
        private GameObject _Canvas;                          //零件架等UI的画布
        private GameObject _InitCanvas;                      //初始化UI画布
        private GameObject _TipCanvas;                       //错误提示画布
        private GameObject _RootPartGameObject;              //LCD1物体
        private Button _RestartBtn;                          //重新开始按钮

        // Use this for initialization
        void Start()
        {
            _Canvas = GameObject.Find("Canvas");

            _InitCanvas = GameObject.Find("InitCanvas");

            _TipCanvas = GameObject.Find("TipsCanvas");

            _RootPartGameObject = FindObjectOfType<AssembleController>().gameObject;

            _RestartBtn = GameObject.Find("InitCanvas/BG/RestartBtn").GetComponent<Button>();

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

            //AssembleManager.Instance.Init();
        }
    }
}
