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

            foreach (var item in dependencyGraph.GetAllNodes())
            {
                item.gameObject.SetActive(false);
            }

            yield return -1;
            List<Node> headers = dependencyGraph.GetHeaders().ToList();
            while (headers.Count > 0)
            {
                List<Node> nextRound = new List<Node>();
                foreach (var node in headers)
                {
                    node.gameObject.SetActive(true);

                    nextRound.AddRange(dependencyGraph.GetNextSteps(node).ToList());
                    yield return new WaitForSeconds(0.2f);
                }
                headers = nextRound;
            }
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