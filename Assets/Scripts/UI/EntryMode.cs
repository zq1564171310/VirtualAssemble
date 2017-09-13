/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>pixiaoli</author>
/// <summary>
/// UI 选择模式
/// </summary>
namespace WyzLink.Manager
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using WyzLink.Parts;

    public class EntryMode : MonoBehaviour
    {
        private static AssembleModel _AssembleModel;

        private Button _DemonstrationBtn;                                  //演示模式按钮
        private Button _StudyBtn;                                          //学习模式按钮
        private Button _ExamBtn;                                           //考试模式按钮

        // Use this for initialization
        void Start()
        {
            _DemonstrationBtn = GameObject.Find("StartCanvas/StartPanel/DemonstrationBtn").GetComponent<Button>();
            _StudyBtn = GameObject.Find("StartCanvas/StartPanel/StudyBtn").GetComponent<Button>();
            _ExamBtn = GameObject.Find("StartCanvas/StartPanel/ExamBtn").GetComponent<Button>();

            _StudyBtn.onClick.AddListener(StudyComein);
            _DemonstrationBtn.onClick.AddListener(DemonstrationComein);
            _ExamBtn.onClick.AddListener(ExamComein);
        }

        // Update is called once per frame
        void Update()
        {

        }


        /// <summary>
        /// 获取当前选择的安装模式
        /// </summary>
        /// <returns></returns>
        public static AssembleModel GeAssembleModel()
        {
            return _AssembleModel;
        }

        /// <summary>
        /// 设置当前安装模式
        /// </summary>
        /// <param name="assembleModel"></param>
        public static void SetAssembleModel(AssembleModel assembleModel)
        {
            _AssembleModel = assembleModel;
        }

        //点击演示模式，进入演示模式
        private void DemonstrationComein()
        {
            SetAssembleModel(AssembleModel.DemonstrationModel);
            SceneManager.LoadScene(1);
        }

        //点击学习模式，进入学习模式界面
        private void StudyComein()
        {
            SetAssembleModel(AssembleModel.StudyModel);
            SceneManager.LoadScene(2);
        }

        //点击考试模式，进入考试模式界面
        private void ExamComein()
        {
            SetAssembleModel(AssembleModel.ExamModel);
            SceneManager.LoadScene(3);
        }
    }
}
