﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// This is the code for testing. Do not take it in the production scene
/// </summary>

namespace WyzLink.Assemble
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Parts;

    public class TestScene : MonoBehaviour
    {
        public Transform[] targets;

        private void OnGUI()
        {
            if (GUI.Button(new Rect(20, 20, 120, 40), "Run flow"))
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

            if (GUI.Button(new Rect(160, 20, 120, 40), "Run animations"))
            {
                StartCoroutine(TestAnimations());
            }
        }

        private IEnumerator FlowRender(AssembleController assembleController)
        {
            yield return new WaitForSeconds(1f);

            foreach (var node in assembleController.GetDependencyGraph().GetAllNodes())
            {
                node.SetInstallationState(InstallationState.NotInstalled);
                node.gameObject.SetActive(false);
            }
            yield return -1;

            yield return StartCoroutine(FlowRenderOneFlow(assembleController, assembleController.GetDependencyGraph().GetHeaders()));
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

                    yield return new WaitForSeconds(0.4f);

                    yield return StartCoroutine(FlowRenderOneFlow(assembleController, assembleController.GetDependencyGraph().GetNextSteps(node)));
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