/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI零件分页界面
/// </summary>

namespace WyzLink.UI
{
    using HoloToolkit.Unity.InputModule;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Common;
    using WyzLink.LogicManager;
    using WyzLink.Parts;
    using WyzLink.Utils.Overriding;

    public interface UIPartsPage
    {
        /// <summary>
        /// 当前页面索引
        /// </summary>
        int m_PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        int m_PageCount
        {
            get;
            set;
        }

        /// <summary>
        /// 元素总个数
        /// </summary>
        int m_ItemsCount
        {
            get;
            set;
        }

        /// <summary>
        /// 元素列表
        /// </summary>
        IList<Node> m_ItemsList
        {
            get;
            set;
        }

        /// <summary>
        /// 上一页
        /// </summary>
        Button PreviousPage
        {
            get;
            set;
        }

        /// <summary>
        /// 下一页
        /// </summary>
        Button NextPage
        {
            get;
            set;
        }

        /// <summary>
        /// 显示当前页数的标签
        /// </summary>
        Text m_PanelText
        {
            get;
            set;
        }

        /// <summary>
        /// 一页12个
        /// </summary>
        int Page_Count
        {
            get;
            set;
        }

        int InitFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 零件类型
        /// </summary>
        string m_Type
        {
            get;
            set;
        }

        IList<Button> BtnList           //零件按钮集合
        {
            get;
            set;
        }

        IList<Node> NodesList        //所有零件的集合
        {
            get;
            set;
        }

        void Init();

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
        void SetIndex(Node node);

        int GetIndex(Node node);

        /// <summary>
        /// 判断某个零件是否显示在零件架上
        /// </summary>
        bool IsView(Node node);

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
        void BindGridItem(Transform trans, Node gridItem);

        /// <summary>
        /// 类型变化，刷新零件界面
        /// </summary>
        /// <param name="type"></param>
        void RefreshItems();

        /// <summary>
        /// 零件按钮点击事件
        /// </summary>
        void BtnClick(GameObject go);

        /// <summary>
        /// 让按钮看起来不能被点击，仅仅只是UI显示方面
        /// </summary>
        /// <param name="go"></param>
        void DisButton(GameObject go);

        /// <summary>
        /// 让按钮看起来能被点击，仅仅只是UI显示方面
        /// </summary>
        /// <param name="go"></param>
        void AbleButton(GameObject go);

        Vector3 GetBoxColliderSize(GameObject go);
    }
}