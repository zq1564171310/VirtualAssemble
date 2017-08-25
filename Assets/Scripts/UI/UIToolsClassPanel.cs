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
    using UnityEngine.UI;
    using WyzLink.Control;
    using System;

    public class UIToolsClassPanel : MonoBehaviour
    {

        private Toggle ToolsClass1, ToolsClass2, ToolsClass3;//工具类型的三个开关
        private int CurClassPage = 1;//当前类的页数，初始时为第一页
        private List<string> ToolsType = new List<string>();//工具类型集合

        [HideInInspector]
        private Button Previous, Next;//左右翻动工具类别的按钮
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