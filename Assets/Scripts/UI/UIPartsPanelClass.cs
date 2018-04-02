/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI零件类型分页界面
/// </summary>

namespace WyzLink.UI
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Common;
    using WyzLink.Parts;

    public interface UIPartsPanelClass
    {
        /// <summary>
        /// 当前页面索引
        /// </summary>
        int m_PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        int m_PageCount { get; set; }

        /// <summary>
        /// 元素总个数
        /// </summary>
        int m_ItemsCount { get; set; }

        /// <summary>
        /// 元素列表
        /// </summary>
        IList<string> m_ItemsList { get; set; }

        /// <summary>
        /// 上一页
        /// </summary>
        Button m_BtnPrevious { get; set; }

        /// <summary>
        /// 下一页
        /// </summary>
        Button m_BtnNext { get; set; }

        /// <summary>
        /// 一页3个
        /// </summary>
        int Page_Count { get; set; }

        /// <summary>
        /// 当前选中类型
        /// </summary>
        string m_Type { get; set; }

        IList<string> NodeTypesList { get; set; }           //所有零件类型的集合



        void Init();

        /// <summary>
        /// 初始化元素
        /// </summary>
        void InitItems();

        /// <summary>
        /// 下一页
        /// </summary>
        void Next();

        /// <summary>
        /// 上一页
        /// </summary>
        void Previous();

        /// <summary>
        /// 指定跳转相应页数
        /// </summary>
        int SetIndex(string type);

        /// <summary>
        /// 获取当前零件再第几页
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        int GetIndex(string type);


        /// <summary>
        /// 绑定指定索引处的页面元素
        /// </summary>
        /// <param name="index">页面索引</param>
        void BindPage(int index);

        /// <summary>
        /// 将一个GridItem实例绑定到指定的Transform上
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="gridItem"></param>
        void BindGridItem(Transform trans, string gridItem);

        /// <summary>
        /// 类型变化，刷新零件界面
        /// </summary>
        /// <param name="type"></param>
        void PartClass1RefreshItems(bool Ison);

        void PartClass2RefreshItems(bool Ison);

        void PartClass3RefreshItems(bool Ison);

        /// <summary>
        /// 设置零件类型
        /// </summary>
        /// <param name="type"></param>
        void SetPartsType(Node node);

        /// <summary>
        /// 获取零件类型
        /// </summary>
        /// <returns></returns>
        string GetPartsType();
    }
}