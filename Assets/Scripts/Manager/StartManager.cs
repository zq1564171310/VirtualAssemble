/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加零件到集合
/// </summary>
namespace WyzLink.Manager
{
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.Utils.ModelDataHelper;

    public class StartManager : MonoBehaviour
    {
        private GameObject RootPartGameObject;
        private GameObject BeginCanvas;
        private GameObject Canvas;

        public Button Restart;//重新开始按钮
        public Button RecordStart;//上次记录开始按钮
        public Toggle Pattern_demonstration;//演示模式开关
        public Toggle Pattern_Study;//学习模式开关
        public Toggle Pattern_Test;//考试模式开关

        // Use this for initialization
        void Start()
        {
            RootPartGameObject = FindObjectOfType<AssembleController>().gameObject;
            Canvas = GameObject.Find("Canvas");
            if (null != RootPartGameObject)
            {
                foreach(Transform tran in RootPartGameObject.transform)
                {
                    tran.gameObject.SetActive(false);
                }
                Canvas.SetActive(false);
            }
            BeginCanvas = GameObject.Find("BeginCanvas");
            Restart.GetComponent<Button>().onClick.AddListener(StartAssemble);
            //foreach (Transform tran in BeginCanvas.transform)
            //{
            //    if ("Restart" != tran.gameObject.name)
            //    {
            //        tran.gameObject.SetActive(false);
            //    }
            //    else
            //    {
            //        GameObject.Find("BeginCanvas/Restart").gameObject.GetComponent<Button>().onClick.AddListener(StartAssemble);
            //    }
            //}
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StartAssemble()
        {
            if (null != RootPartGameObject)
            {
                foreach (Transform tran in RootPartGameObject.transform)
                {
                    tran.gameObject.SetActive(true);
                }
            }
            Canvas.SetActive(true);
            BeginCanvas.SetActive(false);
            AssembleManager.Instance.Init();
        }
    }
}
