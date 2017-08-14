/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
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
    using UnityEngine.SocialPlatforms;
    using UnityEngine.UI;

    public class NodeManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IManipulationHandler
    {
        private Coroutine MaterialCoroutine;                //光标进入和失去材质改变协程
        private Material OriginalMaterial;                     //零件本来的材质

        private bool IsInstallationStateChangeFlag;           //是否有零件的状态发生变化，有状态变化协程执行相关操作，否则协程继续挂起

        private Coroutine AsseablCoroutine;                //光标进入和失去材质改变协程

        private GameObject Txt;                            //零件名字，跟随零件一起移动

        // Use this for initialization
        void Start()
        {
            IsInstallationStateChangeFlag = true;
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



        public void InstanllIntoMaterial()
        {
            if (this.AsseablCoroutine != null)
            {
                StopCoroutine(this.AsseablCoroutine);
            }
            this.AsseablCoroutine = StartCoroutine(NodeInstallationStateManagerCoroutine());
        }



        private IEnumerator NodeInstallationStateManagerCoroutine()
        {
            InstallationState installationState;
            bool installatFlag;
            while (true)
            {
                installatFlag = false;
                if (true == IsInstallationStateChangeFlag)
                {
                    installationState = gameObject.GetComponent<Node>().GetInstallationState();
                    switch (installationState)
                    {
                        case InstallationState.Installed:
                            //Destroy(gameObject.GetComponent<HandDraggable>());
                            //if (null != gameObject.GetComponent<MeshRenderer>())
                            //{
                            //    TransformIntoMaterial(OriginalMaterial);
                            //    gameObject.transform.position = gameObject.GetComponent<Node>().EndPos;

                            //    foreach (Node node in NodesController.Instance.GetNodeList())
                            //    {
                            //        if (InstallationState.NextInstalling == node.GetInstallationState() || InstallationState.Step1Installed == node.GetInstallationState())
                            //        {
                            //            installatFlag = true;
                            //            break;
                            //        }
                            //    }

                            //    if (false == installatFlag)            //说明这一步所有的零件都已经被安装了，那么该下一步了
                            //    {
                            //        AssembleManager.Instance.NextInstall(gameObject.GetComponent<Node>());
                            //        StopCoroutine(NodeInstallationStateManagerCoroutine());
                            //    }
                            //}
                            break;

                        case InstallationState.NotInstalled:
                            break;

                        case InstallationState.NextInstalling:
                            gameObject.AddComponent<HandDraggable>();
                            if (null != gameObject.GetComponent<MeshRenderer>())
                            {
                                TransformIntoMaterial(GlobalVar.NextInstallMate);
                            }
                            break;

                        case InstallationState.Step1Installed:
                            if (null != gameObject.GetComponent<MeshFilter>())
                            {
                                gameObject.transform.localScale = gameObject.GetComponent<Node>().LocalSize;
                            }
                            break;

                        default:

                            break;
                    }
                    IsInstallationStateChangeFlag = false;
                }
                yield return new WaitForSeconds(0.0010f);
            }
        }

        public void SetIsInstallationStateChangeFlag(bool isInstallationStateChangeFlag)
        {
            IsInstallationStateChangeFlag = isInstallationStateChangeFlag;
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {
            if (InstallationState.Step1Installed != gameObject.GetComponent<Node>().GetInstallationState())
            {
                gameObject.GetComponent<Node>().SetInstallationState(InstallationState.Step1Installed);
                IsInstallationStateChangeFlag = true;
                if (null != gameObject.GetComponent<HandDraggable>())
                {
                    GameObject go = Instantiate(gameObject, gameObject.transform, true);
                    go.transform.parent = GameObject.Find("RuntimeObject").transform;
                    Destroy(go.GetComponent<HandDraggable>());
                    #region Test 此处根据UI布局写活，后续需要根据UI调整
                    Txt = Instantiate(GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/ItemText"), GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject").transform, true);
                    //Txt.transform.parent = GameObject.Find("RuntimeObject").transform;
                    //Txt.transform.position = gameObject.transform.position;
                    Txt.transform.position = gameObject.transform.position;
                    Txt.transform.GetChild(0).GetComponent<Text>().text = gameObject.name;
                    #endregion
                }
            }
        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (InstallationState.Step1Installed == gameObject.GetComponent<Node>().GetInstallationState() && gameObject.GetComponent<Node>().HaulingDistance >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().EndPos))
            {
                gameObject.GetComponent<Node>().SetInstallationState(InstallationState.Installed);
                Destroy(gameObject.GetComponent<HandDraggable>());
                if (null != gameObject.GetComponent<MeshRenderer>())
                {
                    bool installatFlag = false;
                    TransformIntoMaterial(OriginalMaterial);
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
                }
                Txt.SetActive(false);
            }
            else
            {
                Txt.transform.position = gameObject.transform.position;
            }
        }

        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
        {

        }

        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
        {

        }
    }
}