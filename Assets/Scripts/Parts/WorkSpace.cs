/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 工作区
/// </summary>

namespace WyzLink.Parts
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class WorkSpace : MonoBehaviour
    {
        /// <summary>
        /// 是否是主工作区
        /// </summary>
        public bool IsMainWorkSpace;

        /// <summary>
        /// 工作区名字
        /// </summary>
        public string WorkSpaceName;

        /// <summary>
        /// 工作区唯一标识
        /// </summary>
        public int WorkSpaceID;

        /// <summary>
        /// 工作区坐标
        /// </summary>
        public Vector3 WorkSpacePos;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}