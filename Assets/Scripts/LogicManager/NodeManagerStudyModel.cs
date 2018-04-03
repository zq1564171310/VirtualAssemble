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

    public class NodeManagerStudyModel : MonoBehaviour, NodeManager
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
            AssembleManagerStudyModel.Instance.SetPartsInfoPlane(true);
            AssembleManagerStudyModel.Instance.SetPartsInfoTextValue(gameObject.GetComponent<Node>().note.Replace("&", "\n"));
            AssembleManagerStudyModel.Instance.GetPartsInfoBtn().onClick.AddListener(ClosePartInfo);
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
                if (null != gameObject.GetComponent<Node>())              //0.08f
                {
                    if (1 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == NodesCommonStudyModel.Instance.GetInstallationState(gameObject.GetComponent<Node>().nodeId) && 2f >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().EndPos))
                    {
                        bool flag = false;
                        foreach (Node no in AssembleManagerStudyModel.Instance.GetNextInstallNode())
                        {
                            if (gameObject.GetComponent<Node>().nodeId == no.nodeId)
                            {
                                flag = true;
                                NodesCommonStudyModel.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.Installed);
                                AssembleManagerStudyModel.Instance.SetInstalledNodeListStatus(gameObject.GetComponent<Node>(), InstallationState.Installed);
                                PlayerPrefs.SetInt(gameObject.GetComponent<Node>().nodeId.ToString() + "StudyModel", (int)InstallationState.Installed);
                                PlayerPrefs.SetInt("CurrentNodeIDStudyModel", gameObject.GetComponent<Node>().nodeId);
                                foreach (Node node in NodesControllerStudyModel.Instance.GetNodeList())
                                {
                                    if (InstallationState.NextInstalling == node.GetInstallationState())
                                    {
                                        PlayerPrefs.SetInt(node.nodeId.ToString() + "StudyModel", (int)InstallationState.NextInstalling);
                                    }
                                }

                                Destroy(gameObject.GetComponent<HandDraggable>());

                                bool installatFlag = false;

                                foreach (Node node in NodesControllerStudyModel.Instance.GetNodeList())
                                {
                                    if (InstallationState.NextInstalling == node.GetInstallationState() || InstallationState.Step1Installed == node.GetInstallationState())
                                    {
                                        installatFlag = true;
                                        break;
                                    }
                                }

                                Destroy(gameObject.GetComponent<BoxCollider>());

                                gameObject.transform.position = gameObject.GetComponent<Node>().EndPos;
                                gameObject.GetComponent<Node>().PlayAnimations();
                                Destroy(GameObject.Find("RuntimeObject/Nodes/" + gameObject.GetComponent<Node>().name + gameObject.GetComponent<Node>().nodeId));   //如果是准备安装状态，那么还要删掉提示的物体
                                AssembleManagerStudyModel.Instance.GetTipErrBtn().onClick.AddListener(OnMovesIEnumerator);
                                Destroy(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject);

                                if (!gameObject.GetComponent<Node>().hasAnimation)
                                {
                                    AssembleManagerStudyModel.Instance.Recovery();
                                }
                                if (false == installatFlag)            //说明这一步所有的零件都已经被安装了，那么该下一步了
                                {
                                    AssembleManagerStudyModel.Instance.NextInstall(gameObject.GetComponent<Node>());
                                }
                                break;
                            }
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
            AssembleManagerStudyModel.Instance.GetTipErrBtn().onClick.RemoveAllListeners();              //移除监听
            AssembleManagerStudyModel.Instance.SetTipCanvasStatus(false);                                //点击确定之后，移除监听
                                                                                                         //AssembleManagerStudyModel.Instance.AbleButton(gameObject.GetComponent<Node>());
        }

        /// <summary>
        /// 关闭零件属性面板
        /// </summary>
        public void ClosePartInfo()
        {
            AssembleManagerStudyModel.Instance.GetPartsInfoBtn().onClick.RemoveAllListeners();
            AssembleManagerStudyModel.Instance.SetPartsInfoPlane(false);
        }
    }
}