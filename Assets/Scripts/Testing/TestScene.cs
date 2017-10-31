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

        void Start()
        {
            ChangeModeBtn = GameObject.Find("Canvas/BG/WorkAreaControl/PartPanel/SwitchMode").GetComponent<Button>();
            ChangeModeBtn.onClick.AddListener(ChangeModeBtnClick);
            Init();
        }

        public void Init()
        {
            var assembleController = Object.FindObjectOfType<AssembleController>();
            if (assembleController != null)
            {
                StartCoroutine(FlowRender(assembleController));
            }
            else
            {
                Debug.LogError("Not able to find the AssembleController in the scene");
            }
        }

        private void ChangeModeBtnClick()
        {
            EntryMode.SetAssembleModel(AssembleModel.DemonstrationModel);
            SceneManager.LoadScene(0);
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
                    var coroutine = StartCoroutine(AssemblePart(node));

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

        private IEnumerator AssemblePart(Node node)
        {
            Vector3 velocity = Vector3.up;
            node.transform.position = Vector3.left * 2;
            node.gameObject.SetActive(true);
            node.SetInstallationState(InstallationState.Installed);
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
    }
}
