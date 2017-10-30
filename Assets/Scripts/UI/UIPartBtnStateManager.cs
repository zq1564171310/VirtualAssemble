/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI零件界面按钮状态管理
/// </summary>

namespace WyzLink.UI
{
    using System.Collections;
    using UnityEngine;
    using WyzLink.Common;
    using WyzLink.Manager;
    using WyzLink.Parts;

    public class UIPartBtnStateManager : MonoBehaviour
    {
        private Coroutine BtnStateCoroutine;                        //按钮状态协程
        private Node UIPartBtnNode;                                 //当前按钮绑定的零件

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StartCount()       //开启协程
        {
            if (null != BtnStateCoroutine)
            {
                StopCoroutine(BtnStateCoroutine);
            }
            BtnStateCoroutine = StartCoroutine(BtnStateManagerIEnumerator());
        }

        private IEnumerator BtnStateManagerIEnumerator()               //协程的具体实现
        {
            int flag = 0;                                              //闪烁提示的标记
            InstallationState installationState;                       //声明一个安装状态的变量
            while (true)
            {
                if (AssembleModel.StudyModel == EntryMode.GetAssembleModel() || AssembleModel.ExamModel == EntryMode.GetAssembleModel())                      //需要管按钮的状态
                {
                    if (null == UIPartBtnNode || null == AssembleManager.Instance || null == NodesCommon.Instance)              //按钮上没有被绑定的零件
                    {
                        continue;
                    }

                    installationState = NodesCommon.Instance.GetInstallationState(UIPartBtnNode.nodeId);   //获取按钮上被绑定的零件的状态

                    if (InstallationState.Installed == installationState)
                    {
                        AssembleManager.Instance.DisButton(gameObject);
                    }
                    else if (InstallationState.NotInstalled == installationState)
                    {
                        AssembleManager.Instance.AbleButton(gameObject);
                    }
                    else if (InstallationState.NextInstalling == installationState)
                    {
                        if (AssembleModel.StudyModel == EntryMode.GetAssembleModel())                      //学习模式提示待安装的按钮的状态
                        {
                            if (flag == 0)
                            {
                                AssembleManager.Instance.AbleButton(gameObject);
                                flag = 1;
                                yield return new WaitForSeconds(0.6f);
                            }
                            else
                            {
                                AssembleManager.Instance.DisButton(gameObject);
                                flag = 0;
                                yield return new WaitForSeconds(0.6f);
                            }
                        }
                    }
                    else if (InstallationState.Step1Installed == installationState)
                    {
                        AssembleManager.Instance.DisButton(gameObject);
                    }
                    else
                    {

                    }
                }
                yield return 0;
            }
        }

        public void RefreshData(Node node)
        {
            UIPartBtnNode = node;
            StartCount();
        }
    }
}