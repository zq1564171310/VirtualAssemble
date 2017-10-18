/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 常用零件一些公共方法，对外提供数据
/// </summary>
namespace WyzLink.Common
{
    using HoloToolkit.Unity;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Control;
    using WyzLink.ToolsAndCommonParts;

    public class CommonPartsCommon : Singleton<CommonPartsCommon>
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 获取常用零件类的集合
        /// </summary>
        /// <returns></returns>
        public List<CommonParts> GetCommonPartsList()
        {
            if (null != CommonPartsController.Instance)
            {
                return CommonPartsController.Instance.GetCommonPartsList();
            }
            else
            {
                Debug.LogError("没有实例化常用零件控制类（CommonPartsController）");
                return null;
            }
        }

        /// <summary>
        /// 获取常用零件类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetCommonPartsTypes()
        {
            if (null != CommonPartsController.Instance)
            {
                if (0 < CommonPartsController.Instance.GetCommonPartsList().Count)
                {
                    List<string> list = new List<string>();
                    for (int i = 0; i < CommonPartsController.Instance.GetCommonPartsList().Count; i++)
                    {
                        if (!list.Contains(CommonPartsController.Instance.GetCommonPartsList()[i].Type))
                        {
                            list.Add(CommonPartsController.Instance.GetCommonPartsList()[i].Type);
                        }
                    }
                    return list;
                }
                else
                {
                    Debug.LogError("没有获取到常用零件，或者常用零件库没有");
                    return null;
                }
            }
            else
            {
                Debug.LogError("没有实例化常用零件控制类（CommonPartsController）");
                return null;
            }
        }
    }
}
