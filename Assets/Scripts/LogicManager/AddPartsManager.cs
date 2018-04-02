/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加零件到集合的接口
/// </summary>
namespace WyzLink.LogicManager
{
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Assemble;
    using WyzLink.Common;
    using WyzLink.Control;
    using WyzLink.Parts;

    public interface AddPartsManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
    }
}
