/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>pixiaoli</author>
/// <summary>
/// UI 逻辑
/// </summary>

namespace WyzLink.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Manager;
    using WyzLink.ToolsAndCommonParts;
    using HoloToolkit.Unity;
    using WyzLink.Common;

    public class OnReceivedTools : MonoBehaviour
    {

        private ToolsCommon ToolsCommon;
        private List<Tool> ToolsList; //工具集合
        private List<string> ToolsType = new List<string>();//工具类型集合

        // Use this for initialization
        void Start()
        {
            ToolsList = ToolsCommon.GetToolList();
            ToolsType = ToolsCommon.GetToolTypes();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}