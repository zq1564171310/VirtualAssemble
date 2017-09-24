/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 模式切换
/// </summary>
namespace WyzLink.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using WyzLink.Parts;
    public class ChangeModes : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //StartScence();
            }
        }
        private void StartScence()
        {
            EntryMode.SetAssembleModel(AssembleModel.DemonstrationModel);
            SceneManager.LoadScene(0);
        }

    }
}
