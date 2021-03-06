﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI零件界面按钮状态管理
/// </summary>

namespace WyzLink.UI
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using WyzLink.Common;
    using WyzLink.LogicManager;
    using WyzLink.Parts;

    public class UIPartBtnStateManagerExamModel : MonoBehaviour, UIPartBtnStateManager
    {
        private Coroutine BtnStateCoroutine;                        //按钮状态协程
        private Node UIPartBtnNode;                                 //当前按钮绑定的零件
        private Coroutine BtnRotaCoroutine;                         //按钮旋转协程

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 开启协程
        /// </summary>
        private void StartStatusCount()
        {
            if (true == gameObject.activeInHierarchy)
            {
                if (null != BtnStateCoroutine)
                {
                    StopCoroutine("BtnStateManagerIEnumerator");
                }
                BtnStateCoroutine = StartCoroutine("BtnStateManagerIEnumerator");
            }
        }

        /// <summary>
        /// 关闭协程
        /// </summary>
        public void StopStatusCount()
        {
            if (true == gameObject.activeInHierarchy)
            {
                if (null != BtnStateCoroutine)
                {
                    StopCoroutine("BtnStateManagerIEnumerator");
                }
            }
        }

        private IEnumerator BtnStateManagerIEnumerator()               //协程的具体实现
        {                                           //闪烁提示的标记
            InstallationState installationState;                       //声明一个安装状态的变量
            while (true)
            {

                if (null == UIPartBtnNode || null == AssembleManagerExamModel.Instance || null == NodesCommonExamModel.Instance)              //按钮上没有被绑定的零件
                {
                    continue;
                }

                installationState = NodesCommonExamModel.Instance.GetInstallationState(UIPartBtnNode.nodeId);   //获取按钮上被绑定的零件的状态

                if (InstallationState.Installed == installationState)
                {
                    AssembleManagerExamModel.Instance.DisButton(gameObject);
                }
                else if (InstallationState.NotInstalled == installationState)
                {
                    AssembleManagerExamModel.Instance.AbleButton(gameObject);
                }
                else if (InstallationState.NextInstalling == installationState)
                {

                }
                else if (InstallationState.Step1Installed == installationState)
                {
                    AssembleManagerExamModel.Instance.DisButton(gameObject);
                }
                else
                {

                }
                yield return 0;
            }
        }

        public void RefreshData(Node node)
        {
            UIPartBtnNode = node;
            StartStatusCount();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (null != BtnRotaCoroutine)
            {
                StopCoroutine("BtnRotaManagerIEnumerator");
            }
            BtnRotaCoroutine = StartCoroutine("BtnRotaManagerIEnumerator");
            UIPartBtnNode.gameObject.transform.localScale *= 2f;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            StopCoroutine("BtnRotaManagerIEnumerator");
            UIPartBtnNode.gameObject.transform.localScale /= 2f;
        }

        private IEnumerator BtnRotaManagerIEnumerator()               //协程的具体实现
        {
            if (null != UIPartBtnNode)
            {
                while (true)
                {
                    UIPartBtnNode.transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
                    yield return 0;
                }
            }
        }
    }
}