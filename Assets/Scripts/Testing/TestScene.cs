/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// This is the code for testing. Do not take it in the production scene
/// </summary>

namespace WyzLink.Assemble
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using WyzLink.Parts;
    using WyzLink.UI;

    public class TestScene : MonoBehaviour
    {
        public Transform[] targets;

        private Button ChangeModeBtn;
        private Button PlayModeBtn;
        private Button PauseModeBtn;

        private Vector3 AssembleCenter = new Vector3();                     //工作区旋转的中心点

        List<Node> list = new List<Node>();

        void Start()
        {
            Time.timeScale = 0;

            ChangeModeBtn = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/SwitchMode").GetComponent<Button>();
            PlayModeBtn = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/AddWordArea").GetComponent<Button>();
            PauseModeBtn = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/DelWordArea").GetComponent<Button>();

            ChangeModeBtn.onClick.AddListener(ChangeModeBtnClick);
            PlayModeBtn.onClick.AddListener(PlayFun);
            PauseModeBtn.onClick.AddListener(PauseFun);

            AssembleCenter = FindObjectOfType<AssembleController>().transform.position;

            Init();
        }

        void Update()
        {

        }

        public void Init()
        {
            var assembleController = FindObjectOfType<AssembleController>();
            if (assembleController != null)
            {
                StartCoroutine(FlowRender(assembleController));

                foreach (Node no in assembleController.GetDependencyGraph().GetHeaders().Cast<Node>())
                {
                    list.Add(no);
                }
            }
            else
            {
                Debug.LogError("Not able to find the AssembleController in the scene");
            }
        }

        /// <summary>
        /// 切换模式
        /// </summary>
        private void ChangeModeBtnClick()
        {
            EntryMode.SetAssembleModel(AssembleModel.DemonstrationModel);
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// 播放
        /// </summary>
        private void PlayFun()
        {
            Time.timeScale = 1;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        private void PauseFun()
        {
            Time.timeScale = 0;
        }

        private void OnGUI()
        {

        }

        private IEnumerator FlowRender(AssembleController assembleController)
        {
            yield return new WaitForSeconds(1f);

            foreach (var node in assembleController.GetDependencyGraph().GetAllNodes().Cast<Node>())
            {
                node.SetInstallationState(InstallationState.NotInstalled);
                node.gameObject.SetActive(false);
            }
            yield return -1;

            yield return StartCoroutine(FlowRenderOneFlow(assembleController, assembleController.GetDependencyGraph().GetHeaders().Cast<Node>()));
            Debug.Log("All items are installed");
        }

        private IEnumerator FlowRenderOneFlow(AssembleController assembleController, IEnumerable<Node> headers)
        {
            foreach (var node in headers)
            {
                if (assembleController.GetDependencyGraph().IsNodeValidToInstall(node))
                {
                    var coroutine = StartCoroutine(AssemblePart(headers, node));

                    if (node.hasAnimation)
                    {
                        yield return coroutine;
                        yield return node.PlayAnimations();
                    }

                    yield return new WaitForSeconds(1.4f);

                    yield return StartCoroutine(FlowRenderOneFlow(assembleController, assembleController.GetDependencyGraph().GetNextSteps(node).Cast<Node>()));
                }
            }
        }

        private IEnumerator AssemblePart(IEnumerable<Node> headers, Node node)
        {
            Vector3 velocity = Vector3.up;
            node.transform.position = Vector3.left * 2;
            node.gameObject.SetActive(true);
            node.SetInstallationState(InstallationState.Installed);

            //AutoScaleAndRota(node);

            float deltaTime = 0;
            while (deltaTime < 0.6f)
            {
                node.transform.position = Vector3.SmoothDamp(node.transform.position, node.GetTargetPosition(), ref velocity, 0.3f);
                deltaTime += Time.deltaTime;
                yield return 1;
            }
            node.transform.position = node.GetTargetPosition();
        }

        // To invoke an animation, call PlayAnimation with animation name and animation target, 
        // The animation target is a transform, with Z forward as the forward
        private IEnumerator TestAnimations()
        {
            foreach (var t in targets)
            {
                if (t == null)
                {
                    continue;
                }
                yield return AnimationCollection.Instance.PlayAnimation("十字螺丝刀旋入", t);
                yield return new WaitForSeconds(0.1f);

                yield return AnimationCollection.Instance.PlayAnimation("一字螺丝刀旋入", t);
                yield return new WaitForSeconds(0.1f);

                yield return AnimationCollection.Instance.PlayAnimation("M12内六角旋入", t);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void AutoScaleAndRota(Node node)
        {
            Vector3 mainCamVertor = GameObject.FindWithTag("MainCamera").transform.position;   //照相机坐标
            Vector3 nodeVector = node.EndPos;

            float angle = Vector3.Angle(AssembleCenter - mainCamVertor, AssembleCenter - nodeVector); //求出两向量之间的夹角  
            Vector3 normal = Vector3.Cross(AssembleCenter - mainCamVertor, AssembleCenter - nodeVector);//叉乘求出法线向量  
            angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.up));  //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向

            foreach (Node no in list)
            {
                //no.EndPos = Quaternion.AngleAxis(-angle, Vector3.up) * (no.EndPos - AssembleCenter) + AssembleCenter;
                //no.EndPosForScale = Quaternion.AngleAxis(-angle, Vector3.up) * (no.EndPosForScale - AssembleCenter) + AssembleCenter;
                //if (InstallationState.Installed == no.GetInstallationState())
                //{
                //    no.EndPos = Quaternion.AngleAxis(-angle, Vector3.up) * (no.EndPos - AssembleCenter) + AssembleCenter;
                //    no.EndPosForScale = Quaternion.AngleAxis(-angle, Vector3.up) * (no.EndPosForScale - AssembleCenter) + AssembleCenter;

                //    no.gameObject.transform.RotateAround(AssembleCenter, Vector3.up, -angle);
                //}
            }

            Vector3 nodeSize = node.GetPartModelRealSize(node.gameObject);

            float scaleX = 1;
            float scaleY = 1;
            float scaleZ = 1;
            float scale = 1;
            if (nodeSize.x < GlobalVar.AutoScalVector.x)
            {
                scaleX = GlobalVar.AutoScalVector.x / nodeSize.x;
                if (nodeSize.y < GlobalVar.AutoScalVector.y)
                {
                    scaleY = GlobalVar.AutoScalVector.y / nodeSize.y;
                    if (nodeSize.z < GlobalVar.AutoScalVector.z)
                    {
                        scaleZ = GlobalVar.AutoScalVector.y / nodeSize.y;
                    }
                }
            }

            if (scaleX > 1 || scaleY > 1 || scaleZ > 1)
            {
                scale = (scaleX > scaleY && scaleX > scaleZ) ? scaleX : (scaleY > scaleZ ? scaleY : scaleZ);
            }

            if (scale > 3)     //防止太大
            {
                scale = 3;
            }

            foreach (Node no in list)
            {
                no.EndPos = scale * (no.EndPosForScale - AssembleCenter) + AssembleCenter;
                if (InstallationState.Installed == no.GetInstallationState())
                {
                    no.gameObject.transform.localScale = no.LocalSize * scale;
                    no.EndPos = scale * (no.EndPosForScale - AssembleCenter) + AssembleCenter;
                    no.gameObject.transform.position = no.EndPos;
                }
            }

            //FindObjectOfType<AssembleController>().transform.localScale *= scale;

        }
    }
}
