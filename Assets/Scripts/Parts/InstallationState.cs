/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The state of the installation
/// </summary>

namespace WyzLink.Parts
{
    public enum InstallationState
    {
        NotInstalled,                             //还没安装
        NextInstalling,                           //还没安装，但是轮到该零件安装了
        Step1Installed,                           //正在安装
        WorkSpaceInstalled,                       //已经安装到了其他工作区
        Installed,                                //已经安装了
    }
}