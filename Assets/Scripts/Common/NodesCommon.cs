/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 零件一些公共接口，目前已知是给UI提供零件集合，零件分类等
/// </summary>
namespace WyzLink.Common
{
    using HoloToolkit.Unity;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Control;
    using WyzLink.Parts;

    public class NodesCommon : Singleton<NodesCommon>
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
        public List<Node> GetNodeList()
        {
            if (null != NodesController.Instance)
            {
                return NodesController.Instance.GetNodeList();
            }
            else
            {
                Debug.LogError("没有实例化零件控制类（NodesController）");
                return null;
            }
        }

        /// <summary>
        /// 获取零件类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetNodeTypes()
        {
            if (null != NodesController.Instance)
            {
                if (0 < NodesController.Instance.GetNodeList().Count)
                {
                    List<string> list = new List<string>();
                    for (int i = 0; i < NodesController.Instance.GetNodeList().Count; i++)
                    {
                        if (!list.Contains(NodesController.Instance.GetNodeList()[i].Type))
                        {
                            list.Add(NodesController.Instance.GetNodeList()[i].Type);
                        }
                    }
                    return list;
                }
                else
                {
                    Debug.LogError("没有获取到零件，或者零件加上没有零件");
                    return null;
                }
            }
            else
            {
                Debug.LogError("没有实例化零件控制类（NodesController）");
                return null;
            }
        }
    }
}