﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
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

        private void Start()
        {
            AssembleFlowParser.ParseAssembleFlowFile(assembleFlow.text, (a0, a1) => {
                // TODO: The code to generate the flow manager
            });
        }

        public List<T> GetAllNodes<T>()
        {
            var list = new List<T>();
            GetAllPartObject<T>(this.transform, list);
            return list;
        }

        private void GetAllPartObject<T>(Transform transform, List<T> list)
        {
            var part = transform.GetComponent<T>();
            if (part != null)
            {
                list.Add(part);
            }
            foreach (Transform t in transform)
            {
                GetAllPartObject<T>(t, list);
            }
        }
    }
}