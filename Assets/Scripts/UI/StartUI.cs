/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI 开始界面
/// </summary>
namespace WyzLink.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.LogicManager;

    public interface StartUI
    {
        /// <summary>
        /// 重新开始
        /// </summary>
        void Restart();

        /// <summary>
        /// 读取存档
        /// </summary>
        void Record();
    }
}
