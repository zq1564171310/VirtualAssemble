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
    using WyzLink.Common;
    using UnityEngine.UI;

    public class NodeManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IManipulationHandler
    {
        private Coroutine MaterialCoroutine;                //光标进入和失去材质改变协程
        private Material OriginalMaterial;                     //零件本来的材质
        private GameObject _TipCanvas;                       //错误提示画布
        private GameObject _TipErrBtn;

        // Use this for initialization
        void Start()
        {
            _TipCanvas = GameObject.Find("TipsCanvas");
            _TipErrBtn = GameObject.Find("TipsCanvas/ErrorBack");
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


        private IEnumerator NodeInstallationStateManagerCoroutine()
        {
            InstallationState installationState;
            bool installatFlag;
            while (true)
            {
                installatFlag = false;
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
                yield return new WaitForSeconds(0.0010f);
            }
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {
            if (null != gameObject.GetComponent<Node>())
            {
                if (InstallationState.NextInstalling == NodesCommon.Instance.GetInstallationState(gameObject.GetComponent<Node>().nodeId))
                {
                    if (null != gameObject.GetComponent<HandDraggable>())
                    {
                        NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.Step1Installed);
                    }
                }

                if (EntryMode.GeAssembleModel() == AssembleModel.ExamModel)
                {
                    if (null != gameObject.GetComponent<HandDraggable>())
                    {
                        NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.Step1Installed);
                    }
                }
            }
        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (null != gameObject.GetComponent<HandDraggable>())
            {
                if (null != gameObject.GetComponent<Node>())
                {
                    if (1 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == NodesCommon.Instance.GetInstallationState(gameObject.GetComponent<Node>().nodeId) && 0.5f >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().EndPos))
                    {
                        if (EntryMode.GeAssembleModel() != AssembleModel.ExamModel)
                        {
                            NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.Installed);
                            Destroy(gameObject.GetComponent<HandDraggable>());
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
                            GameObject.Find("RuntimeObject/" + gameObject.GetComponent<Node>().name + gameObject.GetComponent<Node>().nodeId).SetActive(false);

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
                                NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.NotInstalled);
                                _TipCanvas.SetActive(true);
                                _TipErrBtn.GetComponent<Button>().onClick.AddListener(OnMovesIEnumerator);
                                GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject.SetActive(false);
                            }
                        }
                    }
                    else if (2 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == NodesCommon.Instance.GetInstallationState(gameObject.GetComponent<Node>().nodeId) && 0.1f >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().WorSpaceRelativePos))
                    {
                        NodesCommon.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.WorkSpaceInstalled);
                        gameObject.transform.position = gameObject.GetComponent<Node>().WorSpaceRelativePos;
                        Destroy(gameObject.GetComponent<HandDraggable>());
                        Destroy(gameObject.GetComponent<BoxCollider>());
                        gameObject.transform.parent = GameObject.Find("RuntimeObject/SecondWorkSpace").transform;
                        GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject.SetActive(false);
                        GameObject.Find("RuntimeObject/" + gameObject.GetComponent<Node>().name + "Sec" + gameObject.GetComponent<Node>().nodeId).SetActive(false);

                        bool flag = false;
                        foreach (Transform trans in GameObject.Find("RuntimeObject/SecondWorkSpace").transform)
                        {
                            if (InstallationState.WorkSpaceInstalled != NodesCommon.Instance.GetInstallationState(trans.gameObject.GetComponent<Node>().nodeId))
                            {
                                flag = true;
                                break;
                            }
                        }

                        if (false == flag)                 //第二工作区全部安装完毕
                        {
                            Debug.Log("第二工作区全部安装完毕");
                            if (null == GameObject.Find("RuntimeObject/SecondWorkSpace").GetComponent<BoxCollider>())
                            {
                                GameObject.Find("RuntimeObject/SecondWorkSpace").AddComponent<BoxCollider>();
                            }
                            if (null == GameObject.Find("RuntimeObject/SecondWorkSpace").GetComponent<HandDraggable>())
                            {
                                GameObject.Find("RuntimeObject/SecondWorkSpace").AddComponent<HandDraggable>();
                            }
                            if (null == GameObject.Find("RuntimeObject/SecondWorkSpace").GetComponent<NodeManager>())
                            {
                                GameObject.Find("RuntimeObject/SecondWorkSpace").AddComponent<NodeManager>();
                            }
                        }
                    }
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
            //float f = 0;
            //while (true)
            //{
            //    if (f <= 3f)
            //    {
            //        transform.position = Vector3.Lerp(gameObject.GetComponent<Node>().EndPos, gameObject.GetComponent<Node>().EndPos + new Vector3(0, 0, 3f), f / 3f);
            //        f += Time.deltaTime;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //   // yield return new WaitForSeconds(0);
            //}
            Destroy(gameObject);
            _TipErrBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            _TipCanvas.SetActive(false);
        }
    }
}
























///// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
///// <author>zq</author>
///// <summary>
///// 单个零件的处理逻辑，如高亮，被点击等
///// </summary>
//namespace WyzLink.Manager
//{
//    using HoloToolkit.Unity.InputModule;
//    using System.Collections;
//    using UnityEngine;
//    using UnityEngine.EventSystems;
//    using WyzLink.Parts;
//    using WyzLink.Control;
//    using UnityEngine.SocialPlatforms;
//    using UnityEngine.UI;
//    using WyzLink.Common;
//    using System.Collections.Generic;

//    public class NodeManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IManipulationHandler
//    {
//        private Coroutine MaterialCoroutine;                //光标进入和失去材质改变协程
//        private Material OriginalMaterial;                     //零件本来的材质

//        private bool IsInstallationStateChangeFlag;           //是否有零件的状态发生变化，有状态变化协程执行相关操作，否则协程继续挂起

//        private Coroutine AsseablCoroutine;                //光标进入和失去材质改变协程

//        private GameObject Txt;                            //零件名字，跟随零件一起移动

//        // Use this for initialization
//        void Start()
//        {
//            IsInstallationStateChangeFlag = true;
//            if (null != gameObject.GetComponent<MeshRenderer>())
//            {
//                OriginalMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
//            }
//            //StartCoroutine(NodeInstallationStateManagerCoroutine());
//        }

//        // Update is called once per frame
//        void Update()
//        {

//        }

//        public void OnPointerClick(PointerEventData eventData)
//        {

//        }

//        public void OnPointerEnter(PointerEventData eventData)
//        {
//            if (null != gameObject.GetComponent<MeshRenderer>())
//            {
//                TransformIntoMaterial(GlobalVar.HighLightMate);
//            }
//        }

//        public void OnPointerExit(PointerEventData eventData)
//        {
//            if (null != gameObject.GetComponent<MeshRenderer>() && InstallationState.NextInstalling != gameObject.GetComponent<Node>().GetInstallationState())
//            {
//                TransformIntoMaterial(OriginalMaterial);
//            }
//            else if (null != gameObject.GetComponent<MeshRenderer>() && InstallationState.NextInstalling == gameObject.GetComponent<Node>().GetInstallationState())
//            {
//                TransformIntoMaterial(GlobalVar.NextInstallMate);
//            }
//        }

//        private void TransformIntoMaterial(Material targetMat)
//        {
//            if (this.MaterialCoroutine != null)
//            {
//                StopCoroutine(this.MaterialCoroutine);
//            }
//            this.MaterialCoroutine = StartCoroutine(TransformMaterialCoroutine(targetMat));
//        }

//        private IEnumerator TransformMaterialCoroutine(Material targetMat)
//        {
//            this.GetComponent<MeshRenderer>().material = targetMat;
//            yield return 0;
//        }


//        private IEnumerator NodeInstallationStateManagerCoroutine()
//        {
//            InstallationState installationState;
//            bool installatFlag;
//            while (true)
//            {
//                installatFlag = false;
//                if (true == IsInstallationStateChangeFlag)
//                {
//                    installationState = gameObject.GetComponent<Node>().GetInstallationState();
//                    switch (installationState)
//                    {
//                        case InstallationState.Installed:
//                            break;

//                        case InstallationState.NotInstalled:
//                            break;

//                        case InstallationState.NextInstalling:
//                            if (null != gameObject.GetComponent<MeshRenderer>())
//                            {
//                                TransformIntoMaterial(GlobalVar.NextInstallMate);
//                            }
//                            break;

//                        case InstallationState.Step1Installed:
//                            if (null != gameObject.GetComponent<MeshFilter>())
//                            {
//                                gameObject.transform.localScale = gameObject.GetComponent<Node>().LocalSize;
//                            }
//                            break;

//                        default:

//                            break;
//                    }
//                    IsInstallationStateChangeFlag = false;
//                }
//                yield return new WaitForSeconds(0.0010f);
//            }
//        }

//        public void SetIsInstallationStateChangeFlag(bool isInstallationStateChangeFlag)
//        {
//            IsInstallationStateChangeFlag = isInstallationStateChangeFlag;
//        }

//        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
//        {
//            if (InstallationState.Step1Installed != gameObject.GetComponent<Node>().GetInstallationState())
//            {
//                if (null != gameObject.GetComponent<HandDraggable>())
//                {
//                    gameObject.GetComponent<Node>().SetInstallationState(InstallationState.Step1Installed);
//                    IsInstallationStateChangeFlag = true;
//                }
//            }
//        }

//        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
//        {
//            if (null != gameObject.GetComponent<HandDraggable>())
//            {
//                if (1 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == gameObject.GetComponent<Node>().GetInstallationState() && 10 >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().EndPos))
//                {
//                    gameObject.GetComponent<Node>().SetInstallationState(InstallationState.Installed);
//                    Destroy(gameObject.GetComponent<HandDraggable>());
//                    bool installatFlag = false;

//                    gameObject.transform.position = gameObject.GetComponent<Node>().EndPos;

//                    foreach (Node node in NodesController.Instance.GetNodeList())
//                    {
//                        if (InstallationState.NextInstalling == node.GetInstallationState() || InstallationState.Step1Installed == node.GetInstallationState())
//                        {
//                            installatFlag = true;
//                            break;
//                        }
//                    }

//                    if (false == installatFlag)            //说明这一步所有的零件都已经被安装了，那么该下一步了
//                    {
//                        AssembleManager.Instance.NextInstall(gameObject.GetComponent<Node>());
//                        StopCoroutine(NodeInstallationStateManagerCoroutine());
//                    }

//                    GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject.SetActive(false);
//                    GameObject.Find("RuntimeObject/" + gameObject.GetComponent<Node>().name + gameObject.GetComponent<Node>().nodeId).SetActive(false);
//                }
//                else if (2 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == gameObject.GetComponent<Node>().GetInstallationState() && 10 >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().WorSpaceRelativePos))
//                {
//                    gameObject.GetComponent<Node>().SetInstallationState(InstallationState.WorkSpaceInstalled);
//                    gameObject.transform.position = gameObject.GetComponent<Node>().WorSpaceRelativePos;
//                    Destroy(gameObject.GetComponent<HandDraggable>());
//                    Destroy(gameObject.GetComponent<BoxCollider>());
//                    gameObject.transform.parent = GameObject.Find("RuntimeObject/SecondWorkSpace").transform;
//                    GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject.SetActive(false);
//                    GameObject.Find("RuntimeObject/" + gameObject.GetComponent<Node>().name + "Sec" + gameObject.GetComponent<Node>().nodeId).SetActive(false);

//                    bool flag = false;
//                    List<Node> list = new List<Node>();
//                    foreach (Transform trans in GameObject.Find("RuntimeObject/SecondWorkSpace").transform)
//                    {
//                        if (InstallationState.WorkSpaceInstalled != trans.gameObject.GetComponent<Node>().GetInstallationState())
//                        {
//                            flag = true;
//                            break;
//                        }
//                        list.Add(trans.gameObject.GetComponent<Node>());
//                    }

//                    if (false == flag)                 //第二工作区全部安装完毕
//                    {
//                        Debug.Log("第二工作区全部安装完毕");
//                        GameObject.Find("RuntimeObject/SecondWorkSpace").AddComponent<BoxCollider>();
//                        GameObject.Find("RuntimeObject/SecondWorkSpace").AddComponent<HandDraggable>();
//                    }
//                }
//                else if (2 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.WorkSpaceInstalled == gameObject.GetComponent<Node>().GetInstallationState() && 0.1f >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().WorSpaceRelativePos))
//                {
//                    for (int i = 0; i < NodesCommon.Instance.GetWorkSpaceNodes(2).Count; i++)
//                    {
//                        NodesCommon.Instance.GetWorkSpaceNodes(2)[i].GetComponent<Node>().SetInstallationState(InstallationState.Installed);
//                    }
//                    GameObject.Find("RuntimeObject/SecondWorkSpace").transform.position = WorkSpaceManager.GetPartsInMainWorkSpacePosition(new Vector3(0, 0, 0), GameObject.Find("Canvas/Floor/MainWorkSpace").transform.position, GameObject.Find("Canvas/Floor/MainWorkSpace_Second").transform.position);
//                    Destroy(GameObject.Find("RuntimeObject/SecondWorkSpace").GetComponent<HandDraggable>());
//                    Destroy(GameObject.Find("RuntimeObject/SecondWorkSpace").GetComponent<BoxCollider>());
//                }
//                else if (2 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.WorkSpaceInstalled == gameObject.GetComponent<Node>().GetInstallationState() && 10 <= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().WorSpaceRelativePos))
//                {

//                }
//                else
//                {
//                    GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).transform.position = gameObject.transform.position;
//                }
//            }
//        }



//        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
//        {

//        }

//        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
//        {

//        }
//    }
//}