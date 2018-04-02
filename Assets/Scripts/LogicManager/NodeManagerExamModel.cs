/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 单个零件的处理逻辑，如高亮，被点击等
/// </summary>
namespace WyzLink.LogicManager
{
    using HoloToolkit.Unity.InputModule;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using WyzLink.Parts;
    using WyzLink.Control;
    using WyzLink.Common;
    using WyzLink.UI;
    using System.Collections.Generic;

    public class NodeManagerExamModel : MonoBehaviour, NodeManager
    {
        private Coroutine MaterialCoroutine;                //光标进入和失去材质改变协程
        private Dictionary<GameObject, Material> OriginalMaterial = new Dictionary<GameObject, Material>();   //零件本来的材质
        private Dictionary<GameObject, Material> HighLightMaterial = new Dictionary<GameObject, Material>();   //零件高亮的材质
        private Coroutine NodeInstallationStateManagerCorout;

        // Use this for initialization
        void Start()
        {
            if (null != GetComponent<MeshRenderer>())
            {
                OriginalMaterial.Add(gameObject, gameObject.GetComponent<MeshRenderer>().sharedMaterial);
                HighLightMaterial.Add(gameObject, GlobalVar.HighLightMate);
            }
            else
            {
                Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
                foreach (Transform tran in trans)
                {
                    if (null != tran.gameObject.GetComponent<MeshRenderer>())
                    {
                        OriginalMaterial.Add(tran.gameObject, tran.GetComponent<MeshRenderer>().sharedMaterial);
                        HighLightMaterial.Add(tran.gameObject, GlobalVar.HighLightMate);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AssembleManagerExamModel.Instance.SetPartsInfoPlane(true);
            AssembleManagerExamModel.Instance.SetPartsInfoTextValue(gameObject.GetComponent<Node>().note.Replace("&", "\n"));
            AssembleManagerExamModel.Instance.GetPartsInfoBtn().onClick.AddListener(ClosePartInfo);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TransformIntoMaterial(HighLightMaterial);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TransformIntoMaterial(OriginalMaterial);
        }

        private void TransformIntoMaterial(Dictionary<GameObject, Material> targetMat)
        {
            if (this.MaterialCoroutine != null)
            {
                StopCoroutine(this.MaterialCoroutine);
            }
            this.MaterialCoroutine = StartCoroutine(TransformMaterialCoroutine(targetMat));
        }

        private IEnumerator TransformMaterialCoroutine(Dictionary<GameObject, Material> targetMat)
        {
            if (null != GetComponent<MeshRenderer>())
            {
                this.GetComponent<MeshRenderer>().material = targetMat[gameObject];
            }
            else
            {
                Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
                foreach (Transform tran in trans)
                {
                    if (null != tran.gameObject.GetComponent<MeshRenderer>())
                    {
                        tran.GetComponent<MeshRenderer>().material = targetMat[tran.gameObject];
                    }
                }
            }
            yield return 0;
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {

        }

        //判断是否安装成功
        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (null != gameObject.GetComponent<HandDraggable>())
            {
                if (null != gameObject.GetComponent<Node>())
                {
                    if (1 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == NodesCommonExamModel.Instance.GetInstallationState(gameObject.GetComponent<Node>().nodeId) && 0.08f >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().EndPos))
                    {
                        NodesCommonExamModel.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.Installed);
                        AssembleManagerExamModel.Instance.SetInstalledNodeListStatus(gameObject.GetComponent<Node>(), InstallationState.Installed);
                        Destroy(gameObject.GetComponent<HandDraggable>());
                        bool installatFlag = false;

                        gameObject.transform.position = gameObject.GetComponent<Node>().EndPos;

                        foreach (Node node in NodesControllerExamModel.Instance.GetNodeList())
                        {
                            if (InstallationState.NextInstalling == node.GetInstallationState() || InstallationState.Step1Installed == node.GetInstallationState())
                            {
                                installatFlag = true;
                                break;
                            }
                        }

                        if (false == installatFlag)            //说明这一步所有的零件都已经被安装了，那么该下一步了
                        {
                            AssembleManagerExamModel.Instance.NextInstall(gameObject.GetComponent<Node>());
                        }

                        gameObject.GetComponent<Node>().PlayAnimations();
                        Destroy(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject);
                        Destroy(GameObject.Find("RuntimeObject/Nodes/" + gameObject.GetComponent<Node>().name + gameObject.GetComponent<Node>().nodeId));
                        PlayerPrefs.SetInt(gameObject.GetComponent<Node>().nodeId.ToString(), (int)InstallationState.Installed);

                        Destroy(gameObject.GetComponent<BoxCollider>());
                        gameObject.AddComponent<MeshCollider>();
                        if (!gameObject.GetComponent<Node>().hasAnimation)
                        {
                            AssembleManagerExamModel.Instance.Recovery();
                        }
                    }

                    else
                    {
                        GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).transform.position = gameObject.transform.position;
                    }
                }
            }
        }



        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
        {

        }

        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
        {

        }

        /// <summary>
        /// 安装错误提示信息关闭
        /// </summary>
        public void OnMovesIEnumerator()
        {
            Destroy(gameObject);
            AssembleManagerExamModel.Instance.GetTipErrBtn().onClick.RemoveAllListeners();              //移除监听
            AssembleManagerExamModel.Instance.SetTipCanvasStatus(false);                                //点击确定之后，移除监听
                                                                                                        //AssembleManagerExamModel.Instance.AbleButton(gameObject.GetComponent<Node>());
        }

        /// <summary>
        /// 关闭零件属性面板
        /// </summary>
        public void ClosePartInfo()
        {
            AssembleManagerExamModel.Instance.GetPartsInfoBtn().onClick.RemoveAllListeners();
            AssembleManagerExamModel.Instance.SetPartsInfoPlane(false);
        }
    }
}