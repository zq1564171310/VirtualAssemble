/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI零件界面按钮状态管理
/// </summary>

namespace WyzLink.UI
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using WyzLink.Common;
    using WyzLink.LogicManager;
    using WyzLink.Parts;

    public interface UIPartBtnStateManager : IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="node"></param>
        void RefreshData(Node node);
    }
}