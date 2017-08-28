namespace WyzLink.Manager
{
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;

    public class StartMode : MonoBehaviour
    {

        private GameObject ComeCanvas;//进入某个模式后的选择UI
        public Button Restart;
        public Button RecordStart;
        private GameObject RootPartGameObject;//LCD1
        private GameObject Canvas;//工作UI

        // Use this for initialization
        void Start()
        {
            ComeCanvas = GameObject.Find("ComeCanvas");
            Canvas = GameObject.Find("Canvas");
            RootPartGameObject = FindObjectOfType<AssembleController>().gameObject;
            Restart.GetComponent<Button>().onClick.AddListener(StartAssemble);
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
            ComeCanvas.SetActive(false);
            AssembleManager.Instance.Init();
        }
    }
}

