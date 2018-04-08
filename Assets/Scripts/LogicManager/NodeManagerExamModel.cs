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
            //if (null != GetComponent<MeshRenderer>())
            //{
            //    OriginalMaterial.Add(gameObject, gameObject.GetComponent<MeshRenderer>().sharedMaterial);
            //    HighLightMaterial.Add(gameObject, GlobalVar.HighLightMate);
            //}
            //else
            //{
            //    Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
            //    foreach (Transform tran in trans)
            //    {
            //        if (null != tran.gameObject.GetComponent<MeshRenderer>())
            //        {
            //            OriginalMaterial.Add(tran.gameObject, tran.GetComponent<MeshRenderer>().sharedMaterial);
            //            HighLightMaterial.Add(tran.gameObject, GlobalVar.HighLightMate);
            //        }
            //    }
            //}

            FindGame(transform);

        }




        public void FindGame(Transform t)
        {
            Transform[] trans = FindChild(t);
            if (trans == null)
                return;
            for (int i = 0; i < trans.Length; i++)
            {
                FindGame(trans[i]);
            }

        }
        public int a;
        public Transform[] FindChild(Transform t)
        {
            Transform temp = t;
            MeshRenderer mr = temp.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                OriginalMaterial.Add(temp.gameObject, mr.sharedMaterial);
                HighLightMaterial.Add(temp.gameObject, GlobalVar.HighLightMate);
            }

            Transform[] trans = new Transform[t.childCount];
            if (t.childCount > 0)
            {
                for (int i = 0; i < t.childCount; i++)
                {
                    a++;
                    trans[i] = t.GetChild(i);
                }
                return trans;
            }
            else
                return null;
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
            foreach (KeyValuePair<GameObject, Material> pair in targetMat)
            {
                GameObject go = pair.Key;
                go.GetComponent<MeshRenderer>().material = pair.Value;
            }

            //if (null != GetComponent<MeshRenderer>())
            //{
            //    this.GetComponent<MeshRenderer>().material = targetMat[gameObject];
            //}
            //else
            //{
            //    Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
            //    foreach (Transform tran in trans)
            //    {
            //        if (null != tran.gameObject.GetComponent<MeshRenderer>())
            //        {
            //            tran.GetComponent<MeshRenderer>().material = targetMat[tran.gameObject];
            //        }
            //    }
            //}
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
                    if (1 == gameObject.GetComponent<Node>().WorkSpaceID && InstallationState.Step1Installed == NodesCommonExamModel.Instance.GetInstallationState(gameObject.GetComponent<Node>().nodeId) && 8f >= Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Node>().EndPos))
                    {
                        bool flag = false;
                        NodesCommonExamModel.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.Installed);
                        AssembleManagerExamModel.Instance.SetInstalledNodeListStatus(gameObject.GetComponent<Node>(), InstallationState.Installed);
                        foreach (Node no in AssembleManagerExamModel.Instance.GetNextInstallNode())
                        {
                            if (gameObject.GetComponent<Node>().nodeId == no.nodeId)
                            {
                                flag = true;
                                PlayerPrefs.SetInt(gameObject.GetComponent<Node>().nodeId.ToString() + "ExamModel", (int)InstallationState.Installed);
                                PlayerPrefs.SetInt("CurrentNodeIDExamModel", gameObject.GetComponent<Node>().nodeId);
                                foreach (Node node in NodesControllerExamModel.Instance.GetNodeList())
                                {
                                    if (InstallationState.NextInstalling == node.GetInstallationState())
                                    {
                                        PlayerPrefs.SetInt(node.nodeId.ToString() + "ExamModel", (int)InstallationState.NextInstalling);
                                    }
                                }
                                break;
                            }
                        }

                        Destroy(gameObject.GetComponent<HandDraggable>());

                        Destroy(gameObject.GetComponent<BoxCollider>());

                        gameObject.transform.position = gameObject.GetComponent<Node>().EndPos;
                        gameObject.GetComponent<Node>().PlayAnimations();
                        Destroy(GameObject.Find("RuntimeObject/Nodes/" + gameObject.GetComponent<Node>().name + gameObject.GetComponent<Node>().nodeId));   //如果是准备安装状态，那么还要删掉提示的物体
                        Destroy(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text" + gameObject.GetComponent<Node>().nodeId).gameObject);

                        if (!gameObject.GetComponent<Node>().hasAnimation)
                        {
                            AssembleManagerExamModel.Instance.Recovery();
                        }

                        if (false == flag)   //说明装错了
                        {
                            NodesCommonExamModel.Instance.SetInstallationState(gameObject.GetComponent<Node>().nodeId, InstallationState.NotInstalled);
                            AssembleManagerExamModel.Instance.SetInstalledNodeListStatus(gameObject.GetComponent<Node>(), InstallationState.NotInstalled);
                            AssembleManagerExamModel.Instance.SetTipCanvasStatus(true);
                            AssembleManagerExamModel.Instance.GetTipErrBtn().onClick.AddListener(OnMovesIEnumerator);
                            AssembleManagerExamModel.Instance.ReMoveInstalledNodeList(gameObject.GetComponent<Node>());
                        }
                        else
                        {
                            //安装对了，那么该下一步了
                            AssembleManagerExamModel.Instance.NextInstall(gameObject.GetComponent<Node>());
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