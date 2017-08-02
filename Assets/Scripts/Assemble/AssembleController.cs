/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble
{
    using System.Collections.Generic;
    using UnityEngine;

    public class AssembleController : MonoBehaviour
    {
        public TextAsset assembleFlow;
        private DependencyGraph dependencyGraph;

        private void Awake()
        {
            dependencyGraph = new DependencyGraph(this, assembleFlow.text);
        }

        public DependencyGraph GetDependencyGraph()
        {
            return dependencyGraph;
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