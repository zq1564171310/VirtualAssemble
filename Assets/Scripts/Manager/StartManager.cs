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
        private GameObject CanvasUI;

        // Use this for initialization
        void Start()
        {
            RootPartGameObject = FindObjectOfType<AssembleController>().gameObject;
            if (null != RootPartGameObject)
            {
                RootPartGameObject.SetActive(false);
            }
            CanvasUI = GameObject.Find("Canvas");
            foreach (Transform tran in CanvasUI.transform)
            {
                if ("StartPanel" != tran.gameObject.name)
                {
                    tran.gameObject.SetActive(false);
                }
                else
                {
                    GameObject.Find("Canvas/StartPanel/StartBtn").gameObject.GetComponent<Button>().onClick.AddListener(StartAssemble);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StartAssemble()
        {
            RootPartGameObject.SetActive(true);

            foreach (Transform tran in CanvasUI.transform)
            {
                if ("StartPanel" != tran.gameObject.name)
                {
                    tran.gameObject.SetActive(true);
                }
                else
                {
                    tran.gameObject.SetActive(false);
                }
            }

            GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject").GetComponent<PaginationUtil>().Init();

            AssembleManager.Instance.Init();
        }
    }
}
