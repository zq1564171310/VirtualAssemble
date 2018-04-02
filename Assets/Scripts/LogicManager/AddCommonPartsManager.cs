﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加常用零件到集合
/// </summary>
namespace WyzLink.LogicManager
{
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Common;
    using WyzLink.Control;
    using WyzLink.ToolsAndCommonParts;

    public class AddCommonPartsManager : MonoBehaviour
    {
        private GameObject[] CommonParts;                            //Resoures目录下的工具预制体
        private GameObject _RuntimeObjectCommonParts;                           //获取物体RuntimeObject

        // Use this for initialization
        void Start()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            _RuntimeObjectCommonParts = GameObject.Find("RuntimeObject/CommonParts");
            CommonParts = Resources.LoadAll<GameObject>("CommonPartsPrefabs");

            if (null != CommonParts && CommonParts.Length > 0)
            {
                if (CommonPartsController.Instance == null)
                {
                    var _CommonPartsController = new GameObject("CommonPartsController", typeof(CommonPartsController));
                    _CommonPartsController.transform.parent = _RuntimeObjectCommonParts.transform;
                }
                if (CommonPartsCommon.Instance == null)
                {
                    var _CommonPartsCommon = new GameObject("CommonPartsCommon", typeof(CommonPartsCommon));
                    _CommonPartsCommon.transform.parent = _RuntimeObjectCommonParts.transform;
                }
            }

            if (CommonParts.Length > 0)
            {
                GameObject go;
                CommonParts commonPart;
                for (int i = 0; i < CommonParts.Length; i++)
                {
                    go = Instantiate(CommonParts[i]);
                    go.name = CommonParts[i].name;
                    go.transform.parent = _RuntimeObjectCommonParts.transform;
                    go.AddComponent<CommonParts>();
                    commonPart = go.GetComponent<CommonParts>();
                    commonPart.CommonPartsName = go.name;

                    #region Test  暂时分类，后期需要添加设置界面，做成可配置的
                    if (commonPart.name.Contains("半圆头"))
                    {
                        commonPart.Type = "半圆头";
                    }
                    else if (commonPart.name.Contains("内六角"))
                    {
                        commonPart.Type = "内六角";
                    }
                    else if (commonPart.name.Contains("垫片"))
                    {
                        commonPart.Type = "垫片";
                    }
                    else if (commonPart.name.Contains("平头"))
                    {
                        commonPart.Type = "平头";
                    }
                    else
                    {
                        commonPart.Type = "其他";
                    }
                    #endregion

                    CommonPartsController.Instance.AddCommonPartsList(commonPart);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}
