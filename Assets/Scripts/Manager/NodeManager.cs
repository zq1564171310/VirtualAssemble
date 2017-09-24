﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 单个零件的处理逻辑，如高亮，被点击等
/// </summary>
namespace WyzLink.Manager
{
    using HoloToolkit.Unity.InputModule;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using WyzLink.Parts;
    using WyzLink.Control;
    using WyzLink.Common;
    using UnityEngine.UI;
    using WyzLink.UI;

    public class NodeManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IManipulationHandler
    {
        private Coroutine MaterialCoroutine;                //光标进入和失去材质改变协程
        private Material OriginalMaterial;                     //零件本来的材质
        private GameObject PartsInfoPlane;                       //零件信息框面板
        private GameObject PartsInfoContent;


        // Use this for initialization
        void Start()
        {
            PartsInfoPlane = GameObject.Find("Canvas/PartsInfoPlane");
            PartsInfoContent = GameObject.Find("Canvas/PartsInfoPlane/PartsInfo/InfoContent");
            if (null != gameObject.GetComponent<MeshRenderer>())
            {
                OriginalMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            }
            StartCoroutine(NodeInstallationStateManagerCoroutine());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AssembleManager.Instance.SetPartsInfoPlane(true);
            AssembleManager.Instance.SetPartsInfoTextValue(gameObject.GetComponent<Node>().note);
            AssembleManager.Instance.GetPartsInfoBtn().onClick.AddListener(ClosePartInfo);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (null != gameObject.GetComponent<MeshRenderer>())
            {
                TransformIntoMaterial(GlobalVar.HighLightMate);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (null != gameObject.GetComponent<MeshRenderer>() && InstallationState.NextInstalling != gameObject.GetComponent<Node>().GetInstallationState())
            {
                TransformIntoMaterial(OriginalMaterial);
            }
            else if (null != gameObject.GetComponent<MeshRenderer>() && InstallationState.NextInstalling == gameObject.GetComponent<Node>().GetInstallationState())
            {
                TransformIntoMaterial(GlobalVar.NextInstallMate);
            }
        }

        private void TransformIntoMaterial(Material targetMat)
        {
            if (this.MaterialCoroutine != null)
            {
                StopCoroutine(this.MaterialCoroutine);
            }
            this.MaterialCoroutine = StartCoroutine(TransformMaterialCoroutine(targetMat));
        }

        private IEnumerator TransformMaterialCoroutine(Material targetMat)
        {
            this.GetComponent<MeshRenderer>().material = targetMat;
            yield return 0;
        }


        private IEnumerator NodeInstallationStateManagerCoroutine()
        {
            InstallationState installationState;
            while (true)
            {
                if (null != gameObject.GetComponent<Node>())
                {
                    installationState = gameObject.GetComponent<Node>().GetInstallationState();
                }
                else
                {
                    installationState = InstallationState.WorkSpaceInstalled;
                }
                switch (installationState)
                {
                    case InstallationState.Installed:

                        break;

                    case InstallationState.NotInstalled:

                        break;

                    case InstallationState.NextInstalling:
                        if (null != gameObject.GetComponent<MeshRenderer>())
                        {
                            TransformIntoMaterial(GlobalVar.NextInstallMate);
                        }
                        break;

                    case InstallationState.Step1Installed:

                        break;

                    default:

                        break;
                }
                yield return new WaitForSeconds(0.001f);
            }
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {

        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (null != gameObject.GetComponent<HandDraggable>())
            {
                if (null != gameObject.GetComponent<Node>())
                {
                    if (1 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == NodesCommon.Instance.GetInstallationState(gameObject.GetComponent<Node>().nodeId) && 0.1f >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().EndPos))
                    {
                        if (EntryMode.GeAssembleModel() != AssembleModel.ExamModel)
                        {
                            NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.Installed);
                            Destroy(gameObject.GetComponent<HandDraggable>());
                            bool installatFlag = false;

                            gameObject.transform.position = gameObject.GetComponent<Node>().EndPos;
                            AssembleManager.Instance.AddInstalledNodeList(gameObject.GetComponent<Node>());

                            foreach (Node node in NodesController.Instance.GetNodeList())
                            {
                                if (InstallationState.NextInstalling == node.GetInstallationState() || InstallationState.Step1Installed == node.GetInstallationState())
                                {
                                    installatFlag = true;
                                    break;
                                }
                            }

                            if (false == installatFlag)            //说明这一步所有的零件都已经被安装了，那么该下一步了
                            {
                                AssembleManager.Instance.NextInstall(gameObject.GetComponent<Node>());
                                StopCoroutine(NodeInstallationStateManagerCoroutine());
                            }

                            gameObject.GetComponent<Node>().PlayAnimations();
                            GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject.SetActive(false);
                            GameObject.Find("RuntimeObject/Nodes/" + gameObject.GetComponent<Node>().name + gameObject.GetComponent<Node>().nodeId).SetActive(false);
                        }
                        else
                        {
                            bool flag = false;
                            foreach (Node no in AssembleManager.Instance.GetNextInstallNode())
                            {
                                if (gameObject.GetComponent<Node>().nodeId == no.nodeId)
                                {
                                    NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.Installed);
                                    Destroy(gameObject.GetComponent<HandDraggable>());
                                    AssembleManager.Instance.AddInstalledNodeList(gameObject.GetComponent<Node>());
                                    bool installatFlag = false;

                                    gameObject.transform.position = gameObject.GetComponent<Node>().EndPos;

                                    foreach (Node node in NodesController.Instance.GetNodeList())
                                    {
                                        if (InstallationState.NextInstalling == node.GetInstallationState() || InstallationState.Step1Installed == node.GetInstallationState())
                                        {
                                            installatFlag = true;
                                            break;
                                        }
                                    }

                                    if (false == installatFlag)            //说明这一步所有的零件都已经被安装了，那么该下一步了
                                    {
                                        AssembleManager.Instance.NextInstall(gameObject.GetComponent<Node>());
                                        StopCoroutine(NodeInstallationStateManagerCoroutine());
                                    }
                                    gameObject.GetComponent<Node>().PlayAnimations();
                                    GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject.SetActive(false);
                                    flag = true;
                                }
                            }

                            if (false == flag)
                            {
                                Destroy(gameObject.GetComponent<HandDraggable>());
                                Destroy(gameObject.GetComponent<BoxCollider>());
                                gameObject.transform.position = gameObject.GetComponent<Node>().EndPos;
                                AssembleManager.Instance.ReMoveInstalledNodeList(gameObject.GetComponent<Node>());
                                NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.NotInstalled);
                                AssembleManager.Instance.SetTipCanvasStatus(true);
                                AssembleManager.Instance.GetTipErrBtn().onClick.AddListener(OnMovesIEnumerator);
                                GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject.SetActive(false);
                            }
                        }
                    }
                    //else if (2 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == NodesCommon.Instance.GetInstallationState(gameObject.GetComponent<Node>().nodeId) && 0.1f >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().WorSpaceRelativePos))
                    //{
                    //    NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.WorkSpaceInstalled);
                    //    gameObject.transform.position = gameObject.GetComponent<Node>().WorSpaceRelativePos;
                    //    Destroy(gameObject.GetComponent<HandDraggable>());
                    //    Destroy(gameObject.GetComponent<BoxCollider>());
                    //    gameObject.transform.parent = GameObject.Find("RuntimeObject/SecondWorkSpace").transform;
                    //    GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject.SetActive(false);
                    //    GameObject.Find("RuntimeObject/Nodes/" + gameObject.GetComponent<Node>().name + "Sec" + gameObject.GetComponent<Node>().nodeId).SetActive(false);

                    //    bool flag = false;
                    //    foreach (Transform trans in GameObject.Find("RuntimeObject/SecondWorkSpace").transform)
                    //    {
                    //        if (InstallationState.WorkSpaceInstalled != NodesCommon.Instance.GetInstallationState(trans.gameObject.GetComponent<Node>().nodeId))
                    //        {
                    //            flag = true;
                    //            break;
                    //        }
                    //    }

                    //    if (false == flag)                 //第二工作区全部安装完毕
                    //    {
                    //        if (null == GameObject.Find("RuntimeObject/SecondWorkSpace").GetComponent<BoxCollider>())
                    //        {
                    //            GameObject.Find("RuntimeObject/SecondWorkSpace").AddComponent<BoxCollider>();
                    //        }
                    //        if (null == GameObject.Find("RuntimeObject/SecondWorkSpace").GetComponent<HandDraggable>())
                    //        {
                    //            GameObject.Find("RuntimeObject/SecondWorkSpace").AddComponent<HandDraggable>();
                    //        }
                    //        if (null == GameObject.Find("RuntimeObject/SecondWorkSpace").GetComponent<NodeManager>())
                    //        {
                    //            GameObject.Find("RuntimeObject/SecondWorkSpace").AddComponent<NodeManager>();
                    //        }
                    //    }
                    //}
                    else
                    {
                        GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).transform.position = gameObject.transform.position;
                    }
                }

                if ("SecondWorkSpace" == gameObject.name && 0.1f >= Vector3.Distance(gameObject.transform.position, GameObject.Find("Canvas/Floor/MainWorkSpace").transform.position))
                {
                    for (int i = 0; i < NodesCommon.Instance.GetWorkSpaceNodes(2).Count; i++)
                    {
                        NodesCommon.Instance.SetInstallationState(NodesCommon.Instance.GetWorkSpaceNodes(2)[i].GetComponent<Node>().nodeId, InstallationState.Installed);
                    }
                    gameObject.transform.position = GameObject.Find("Canvas/Floor/MainWorkSpace").transform.position;
                    Destroy(gameObject.GetComponent<HandDraggable>());
                    Destroy(gameObject.GetComponent<BoxCollider>());
                    Destroy(gameObject.GetComponent<NodeManager>());
                }

            }
        }



        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
        {

        }

        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
        {

        }


        void OnMovesIEnumerator()
        {
            Destroy(gameObject);
            AssembleManager.Instance.GetTipErrBtn().onClick.RemoveAllListeners();              //移除监听
            AssembleManager.Instance.SetTipCanvasStatus(false);                                //点击确定之后，移除监听
            AssembleManager.Instance.AbleButton(gameObject.GetComponent<Node>());
        }

        void ClosePartInfo()
        {
            AssembleManager.Instance.GetPartsInfoBtn().onClick.RemoveAllListeners();
            AssembleManager.Instance.SetPartsInfoPlane(false);
        }
    }
}