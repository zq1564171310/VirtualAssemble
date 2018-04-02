/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 考试模式零件一些公共接口，目前已知是给UI提供零件集合，零件分类等
/// </summary>
namespace WyzLink.Common
{
    using HoloToolkit.Unity;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Control;
    using WyzLink.Parts;

    public class NodesCommonExamModel : Singleton<NodesCommonExamModel>, NodesCommon
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
        public IList<Node> GetNodesList()
        {
            IList<Node> list = new List<Node>();
            if (null != NodesControllerExamModel.Instance)
            {
                list = NodesControllerExamModel.Instance.GetNodeList();
            }
            else
            {
                Debug.LogError("没有实例化零件控制类（NodesController）");
            }
            return list;
        }

        /// <summary>
        /// 获取零件类型
        /// </summary>
        /// <returns></returns>
        public IList<string> GetNodesTypes()
        {
            List<string> list = new List<string>();
            if (null != NodesControllerExamModel.Instance)
            {
                if (0 < NodesControllerExamModel.Instance.GetNodeList().Count)
                {
                    for (int i = 0; i < NodesControllerExamModel.Instance.GetNodeList().Count; i++)
                    {
                        if (!list.Contains(NodesControllerExamModel.Instance.GetNodeList()[i].Type))
                        {
                            list.Add(NodesControllerExamModel.Instance.GetNodeList()[i].Type);
                        }
                    }
                }
                else
                {
                    Debug.LogError("没有获取到零件，或者零件加上没有零件");
                }
            }
            else
            {
                Debug.LogError("没有实例化零件控制类（NodesControllerExamModel）");
            }
            return list;
        }

        /// <summary>
        /// 根据工作区ID，获取该ID下面所有的零件的集合
        /// </summary>
        /// <param name="WorkSpaceID"></param>
        /// <returns></returns>
        public IList<Node> GetWorkSpaceNodes(int WorkSpaceID)
        {
            List<Node> list = new List<Node>();
            if (null != NodesControllerExamModel.Instance)
            {
                if (0 < NodesControllerExamModel.Instance.GetNodeList().Count)
                {
                    for (int i = 0; i < NodesControllerExamModel.Instance.GetNodeList().Count; i++)
                    {
                        if (WorkSpaceID == NodesControllerExamModel.Instance.GetNodeList()[i].WorkSpaceID)
                        {
                            list.Add(NodesControllerExamModel.Instance.GetNodeList()[i]);
                        }
                    }
                }
                else
                {
                    Debug.LogError("没有获取到零件，或者零件加上没有零件");
                }
            }
            else
            {
                Debug.LogError("没有实例化零件控制类（NodesControllerExamModel）");
            }
            return list;
        }


        /// <summary>
        /// 设置零件集合中的某个零件的安装状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="state"></param>
        public void SetInstallationState(int nodeID, InstallationState state)
        {
            if (null != NodesControllerExamModel.Instance)
            {
                if (0 < NodesControllerExamModel.Instance.GetNodeList().Count)
                {
                    for (int i = 0; i < NodesControllerExamModel.Instance.GetNodeList().Count; i++)
                    {
                        if (nodeID == NodesControllerExamModel.Instance.GetNodeList()[i].nodeId)
                        {
                            NodesControllerExamModel.Instance.GetNodeList()[i].SetInstallationState(state);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 获取集合中的某个零件的安装状态
        /// </summary>
        /// <returns></returns>
        public InstallationState GetInstallationState(int nodeID)
        {
            InstallationState state = InstallationState.Installed;
            if (null != NodesControllerExamModel.Instance)
            {
                if (0 < NodesControllerExamModel.Instance.GetNodeList().Count)
                {
                    for (int i = 0; i < NodesControllerExamModel.Instance.GetNodeList().Count; i++)
                    {
                        if (nodeID == NodesControllerExamModel.Instance.GetNodeList()[i].nodeId)
                        {
                            state = NodesControllerExamModel.Instance.GetNodeList()[i].GetInstallationState();
                        }
                    }
                }
            }
            return state;
        }

        /// <summary>
        /// 是否存在零件已经从零件架上取下来，正准备安装
        /// </summary>
        /// <returns></returns>
        public bool IsPartInstallating()
        {
            bool flag = false;
            if (null != NodesControllerExamModel.Instance)
            {
                if (0 < NodesControllerExamModel.Instance.GetNodeList().Count)
                {
                    for (int i = 0; i < NodesControllerExamModel.Instance.GetNodeList().Count; i++)
                    {
                        if (InstallationState.Step1Installed == NodesControllerExamModel.Instance.GetNodeList()[i].GetInstallationState())
                        {
                            flag = true;
                        }
                    }
                }
            }
            return flag;
        }

    }
}