/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using WyzLink.Parts;

    public class AssembleController : MonoBehaviour
    {
        public TextAsset assembleFlow;
        private DependencyGraph dependencyGraph;
        public bool DoFlowRender;

        private void Awake()
        {
            dependencyGraph = new DependencyGraph(this, assembleFlow.text);
        }

        public DependencyGraph GetDependencyGraph()
        {
            return dependencyGraph;
        }

        public void Start()
        {
            if (DoFlowRender)
            {
                StartCoroutine(FlowRender());
            }
        }

        private IEnumerator FlowRender()
        {
            yield return new WaitForSeconds(1f);

            foreach (var node in dependencyGraph.GetAllNodes())
            {
                node.SetInstallationState(InstallationState.NotInstalled);
                node.gameObject.SetActive(false);
            }
            yield return -1;

            yield return StartCoroutine(FlowRenderOneFlow(dependencyGraph.GetHeaders()));
            Debug.Log("All items are installed");
        }

        private IEnumerator FlowRenderOneFlow(IEnumerable<Node> headers)
        {
            foreach (var node in headers)
            {
                if (dependencyGraph.IsNodeValidToInstall(node))
                {
                    StartCoroutine(AssemblePart(node));

                    yield return new WaitForSeconds(0.4f);

                    yield return StartCoroutine(FlowRenderOneFlow(dependencyGraph.GetNextSteps(node)));
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

        public IEnumerable<T> GetAllNodes<T>(Transform transform = null) where T : MonoBehaviour
        {
            if (transform == null)
            {
                transform = this.transform;
            }

            var part = transform.GetComponent<T>();
            if (part != null && part.enabled)
            {
                // We only count if the component is enabled
                yield return part;
            }
            else
            {
                foreach (Transform t in transform)
                {
                    foreach (var p in GetAllNodes<T>(t))
                    {
                        yield return p;
                    }
                }
            }
        }
    }
}