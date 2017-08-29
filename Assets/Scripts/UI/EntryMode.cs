namespace WyzLink.Manager
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.Control;

    public class EntryMode : MonoBehaviour
    {
        public static string Mode = "Study";
        private GameObject RootPartGameObject;//LCD1
        private GameObject StartCanvas;//开始界面
        private GameObject Canvas;//工作UI
        private GameObject ComeCanvas;//进入某个模式后的选择UI

        public Button Restart;
        public Button RecordStart;
        public Button demonstration;//演示模式
        public Button Study;//学习模式
        public Button Test;//考试模式

        AssembleManager assemblemanager;

        // Use this for initialization
        void Start()
        {
            assemblemanager = AssembleManager.Instance;
            RootPartGameObject = FindObjectOfType<AssembleController>().gameObject;
            Canvas = GameObject.Find("Canvas");
            ComeCanvas = GameObject.Find("ComeCanvas");
            //Restart =  GameObject.Find("Restart").GetComponent<Button>();
            if (null != RootPartGameObject)
            {
                foreach (Transform tran in RootPartGameObject.transform)
                {
                    tran.gameObject.SetActive(false);
                }
                Canvas.SetActive(false);
                ComeCanvas.SetActive(false);
            }
            StartCanvas = GameObject.Find("StartCanvas");
            // Study.GetComponent<Button>().onClick.AddListener(StartAssemble);
            Study.GetComponent<Button>().onClick.AddListener(StudyComein);
            demonstration.GetComponent<Button>().onClick.AddListener(demonstrationComein);
            Test.GetComponent<Button>().onClick.AddListener(TestComein);
            //Debug.Log("assemblemanager == null" + assemblemanager == null);
            Restart.GetComponent<Button>().onClick.AddListener(StartAssemble);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StudyComein()
        {
            StartCanvas.SetActive(false);
            ComeCanvas.SetActive(true);
            Mode = "Study";
        }

        private void demonstrationComein()
        {
            StartCanvas.SetActive(false);
            ComeCanvas.SetActive(false);
            Mode = "demonstration";
            SceneManager.LoadScene(1);
        }

        private void TestComein()
        {
            StartCanvas.SetActive(false);
            ComeCanvas.SetActive(true);
            Mode = "Test";

            //SceneManager.LoadScene(2);
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
            Debug.Log("assemblemanager == null" + AssembleManager.Instance.InstalledNode == null);

            AssembleManager.Instance.Init();
        }
    }
}
