﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加工具集合
/// </summary>
namespace WyzLink.LogicManager
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Common;
    using WyzLink.Control;
    using WyzLink.ToolsAndCommonParts;

    public class AddToolsManager : MonoBehaviour
    {
        private GameObject[] Tools;                                  //Resoures目录下的工具预制体
        private GameObject _RuntimeObjectTools;                           //获取物体RuntimeObject

        // Use this for initialization
        void Start()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            _RuntimeObjectTools = GameObject.Find("RuntimeObject/Tools");
            Tools = Resources.LoadAll<GameObject>("ToolsPrefabs");

            if (null != Tools && Tools.Length > 0)
            {
                if (ToolsController.Instance == null)
                {
                    var _ToolsController = new GameObject("ToolsController", typeof(ToolsController));
                    _ToolsController.transform.parent = _RuntimeObjectTools.transform;
                }
                if (ToolsCommon.Instance == null)
                {
                    var _ToolsCommon = new GameObject("ToolsCommon", typeof(ToolsCommon));
                    _ToolsCommon.transform.parent = _RuntimeObjectTools.transform;
                }
            }

            if (Tools.Length > 0)
            {
                GameObject go;
                Tool tool;
                for (int i = 0; i < Tools.Length; i++)
                {
                    go = Instantiate(Tools[i]);
                    go.name = Tools[i].name;
                    go.transform.parent = _RuntimeObjectTools.transform;
                    go.AddComponent<Tool>();
                    tool = go.GetComponent<Tool>();
                    tool.ToolName = go.name;

                    #region Test         暂时分类
                    if (tool.name.Contains("螺丝刀"))
                    {
                        tool.Type = "螺丝刀";
                    }
                    else if (tool.name.Contains("M2.0用内六角"))
                    {
                        tool.Type = "M2.0";
                    }
                    else if (tool.name.Contains("M2.5用内六角"))
                    {
                        tool.Type = "M2.5";
                    }
                    else if (tool.name.Contains("M4用内六角"))
                    {
                        tool.Type = "M4";
                    }
                    else
                    {
                        tool.Type = "其他";
                    }
                    #endregion

                    ToolsController.Instance.AddToolList(tool);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}