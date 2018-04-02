/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 单个零件的处理逻辑，如高亮，被点击等
/// </summary>
namespace WyzLink.LogicManager
{
    using HoloToolkit.Unity.InputModule;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using WyzLink.Parts;
    using WyzLink.Control;
    using WyzLink.Common;
    using WyzLink.UI;
    using System.Collections.Generic;

    public interface NodeManager : IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IManipulationHandler
    {
        /// <summary>
        /// 安装错误提示信息关闭
        /// </summary>
        void OnMovesIEnumerator();

        /// <summary>
        /// 关闭零件属性面板
        /// </summary>
        void ClosePartInfo();
    }
}