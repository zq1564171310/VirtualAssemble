using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WyzLink.Manager;
using WyzLink.Parts;
using WyzLink.UI;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartScence();
            //DemonstrationComein();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            //StudyComein();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            //ExamComein();
        }
    }
    private void StartScence()
    {
        EntryMode.SetAssembleModel(AssembleModel.DemonstrationModel);
        SceneManager.LoadScene(0);
    }

    private void DemonstrationComein()
    {
        EntryMode.SetAssembleModel(AssembleModel.DemonstrationModel);
        SceneManager.LoadScene(1);
    }

    //点击学习模式，进入学习模式界面
    private void StudyComein()
    {
        EntryMode.SetAssembleModel(AssembleModel.StudyModel);
        SceneManager.LoadScene(2);
    }

    //点击考试模式，进入考试模式界面
    private void ExamComein()
    {
        EntryMode.SetAssembleModel(AssembleModel.ExamModel);
        SceneManager.LoadScene(3);
    }
}
