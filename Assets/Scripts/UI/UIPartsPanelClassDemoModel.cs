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

    public class UIPartsPanelClassDemoModel : MonoBehaviour, UIPartsPanelClass
    {
        private int _PageIndex;
        /// <summary>
        /// 当前页面索引
        /// </summary>
        public int m_PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }

        private int _PageCount;
        /// <summary>
        /// 总页数
        /// </summary>
        public int m_PageCount
        {
            get { return _PageCount; }
            set { _PageCount = value; }
        }

        private int _ItemsCount;
        /// <summary>
        /// 元素总个数
        /// </summary>
        public int m_ItemsCount
        {
            get { return _ItemsCount; }
            set { _ItemsCount = value; }
        }

        private IList<string> _ItemsList;
        /// <summary>
        /// 元素列表
        /// </summary>
        public IList<string> m_ItemsList
        {
            get { return _ItemsList; }
            set { _ItemsList = value; }
        }

        private Button _BtnPrevious;
        /// <summary>
        /// 上一页
        /// </summary>
        public Button m_BtnPrevious
        {
            get { return _BtnPrevious; }
            set { _BtnPrevious = value; }
        }

        private Button _BtnNext;
        /// <summary>
        /// 下一页
        /// </summary>
        public Button m_BtnNext
        {
            get { return _BtnNext; }
            set { _BtnNext = value; }
        }

        private int _Page_Count;
        /// <summary>
        /// 一页3个
        /// </summary>
        public int Page_Count
        {
            get { return _Page_Count; }
            set { _Page_Count = value; }
        }

        /// <summary>
        /// Toggles
        /// </summary>
        public Toggle PartClass1, PartClass2, PartClass3;

        private string _Type;
        /// <summary>
        /// 当前选中类型
        /// </summary>
        public string m_Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private IList<string> _NodeTypesList;
        public IList<string> NodeTypesList             //所有零件类型的集合
        {
            get
            {
                return _NodeTypesList;
            }
            set { _NodeTypesList = value; }
        }

        public UIPartsPageDemoModel _UIPartsPage;


        // Use this for initialization
        void Start()
        {

        }

        void Update()
        {

        }

        public void Init()
        {
            m_PageIndex = 1;

            m_PageCount = 0;

            m_ItemsCount = 0;

            Page_Count = 3;

            NodeTypesList = new List<string>();
        }

        /// <summary>
        /// 初始化元素
        /// </summary>
        public void InitItems()
        {

        }

        /// <summary>
        /// 下一页
        /// </summary>
        public void Next()
        {

        }

        /// <summary>
        /// 上一页
        /// </summary>
        public void Previous()
        {

        }

        /// <summary>
        /// 指定跳转相应页数
        /// </summary>
        public int SetIndex(string type)
        {
            return 0;
        }

        /// <summary>
        /// 获取当前零件再第几页
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int GetIndex(string type)
        {

            return 0;
        }

        /// <summary>
        /// 绑定指定索引处的页面元素
        /// </summary>
        /// <param name="index">页面索引</param>
        public void BindPage(int index)
        {

        }


        /// <summary>
        /// 将一个GridItem实例绑定到指定的Transform上
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="gridItem"></param>
        public void BindGridItem(Transform trans, string gridItem)
        {

        }

        /// <summary>
        /// 类型变化，刷新零件界面
        /// </summary>
        /// <param name="type"></param>
        public void PartClass1RefreshItems(bool Ison)
        {

        }

        public void PartClass2RefreshItems(bool Ison)
        {

        }

        public void PartClass3RefreshItems(bool Ison)
        {

        }

        /// <summary>
        /// 设置零件类型
        /// </summary>
        /// <param name="type"></param>
        public void SetPartsType(Node node)
        {

        }


        /// <summary>
        /// 获取零件类型
        /// </summary>
        /// <returns></returns>
        public string GetPartsType()
        {
            return m_Type;
        }

    }
}