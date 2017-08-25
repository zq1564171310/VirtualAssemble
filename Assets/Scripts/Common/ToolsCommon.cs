/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 工具一些公共接口
/// </summary>
namespace WyzLink.Common
{
    using HoloToolkit.Unity;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Control;
    using WyzLink.ToolsAndCommonParts;

    public class ToolsCommon : Singleton<ToolsCommon>
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
        /// 获取零件类的集合
        /// </summary>
        /// <returns></returns>
        public List<Tool> GetToolList()
        {
            if (null != ToolsController.Instance)
            {
                return ToolsController.Instance.GetToolList();
            }
            else
            {
                Debug.LogError("没有实例化零件控制类（ToolsController）");
                return null;
            }
        }

        public List<string> GetToolTypes()
        {
            if (null != ToolsController.Instance)
            {
                if (0 < ToolsController.Instance.GetToolList().Count)
                {
                    List<string> list = new List<string>();
                    for (int i = 0; i < ToolsController.Instance.GetToolList().Count; i++)
                    {
                        if (!list.Contains(ToolsController.Instance.GetToolList()[i].Type))
                        {
                            list.Add(ToolsController.Instance.GetToolList()[i].Type);
                        }
                    }
                    return list;
                }
                else
                {
                    Debug.LogError("没有获取到工具，或者工具库没有工具");
                    return null;
                }
            }
            else
            {
                Debug.LogError("没有实例化工具控制类（ToolsController）");
                return null;
            }
        }

    }
}
