/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 零件安装类
/// </summary>
namespace WyzLink.LogicManager
{
    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.Common;
    using WyzLink.Control;
    using WyzLink.Parts;
    using WyzLink.UI;

#if NETFX_CORE  //UWP下编译  
using Windows.Storage;
#endif

    public interface AssembleManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 获取当前正在被安装的零件
        /// </summary>
        Node GetCurrentnode();

        /// <summary>
        /// 获取零件的起点位置
        /// </summary>
        /// <returns></returns>
        Vector3 GetPartStartPosition();

        /// <summary>
        /// 初始化DependencyGraph类，调用底层
        /// </summary>
        /// <param name="dependencyGraph"></param>
        void SetDependencyGraph(DependencyGraph dependencyGraph);

        /// <summary>
        /// 设置当前已经安装的零件
        /// </summary>
        void AddInstalledNodeList(Node installedNode);

        /// <summary>
        /// 移除当前已经安装的零件
        /// </summary>
        /// <param name="installedNode"></param>
        void ReMoveInstalledNodeList(Node installedNode);

        /// <summary>
        /// 修改某个零件的安装状态
        /// </summary>
        /// <param name="installedNode"></param>
        void SetInstalledNodeListStatus(Node installedNode, InstallationState installationState);

        /// <summary>
        /// 拍照
        /// </summary>
        void CaptureScreens();

    }

}
