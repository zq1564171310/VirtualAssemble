/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 零件一些公共接口类，目前已知是给UI提供零件集合，零件分类等
/// </summary>
namespace WyzLink.Common
{
    using HoloToolkit.Unity;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Control;
    using WyzLink.Parts;

    public interface NodesCommon
    {
        /// <summary>
        /// 获取零件类的集合
        /// </summary>
        /// <returns></returns>
        IList<Node> GetNodesList();

        /// <summary>
        /// 获取零件类型
        /// </summary>
        /// <returns></returns>
        IList<string> GetNodesTypes();

        /// <summary>
        /// 根据工作区ID，获取该ID下面所有的零件的集合
        /// </summary>
        /// <param name="WorkSpaceID"></param>
        /// <returns></returns>
        IList<Node> GetWorkSpaceNodes(int WorkSpaceID);

        /// <summary>
        /// 设置零件集合中的某个零件的安装状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="state"></param>
        void SetInstallationState(int nodeID, InstallationState state);


        /// <summary>
        /// 获取集合中的某个零件的安装状态
        /// </summary>
        /// <returns></returns>
        InstallationState GetInstallationState(int nodeID);

        /// <summary>
        /// 是否存在零件已经从零件架上取下来，正准备安装
        /// </summary>
        /// <returns></returns>
        bool IsPartInstallating();
    }
}