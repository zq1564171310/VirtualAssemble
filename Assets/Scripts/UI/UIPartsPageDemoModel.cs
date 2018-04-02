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

    public class UIPartsPageDemoModel : MonoBehaviour, UIPartsPage
    {
        /// <summary>
        /// 当前页面索引
        /// </summary>
        private int _PageIndex;
        public int m_PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        private int _PageCount;
        public int m_PageCount
        {
            get { return _PageCount; }
            set { _PageCount = value; }
        }

        /// <summary>
        /// 元素总个数
        /// </summary>
        private int _ItemsCount;
        public int m_ItemsCount
        {
            get { return _ItemsCount; }
            set { _ItemsCount = value; }
        }

        /// <summary>
        /// 元素列表
        /// </summary>
        private IList<Node> _ItemsList;
        public IList<Node> m_ItemsList
        {
            get { return _ItemsList; }
            set { _ItemsList = value; }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private Button _PreviousPage;
        public Button PreviousPage
        {
            get { return _PreviousPage; }
            set { _PreviousPage = value; }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private Button _NextPage;
        public Button NextPage
        {
            get { return _NextPage; }
            set { _NextPage = value; }
        }

        /// <summary>
        /// 显示当前页数的标签
        /// </summary>
        private Text _PanelText;
        public Text m_PanelText
        {
            get { return _PanelText; }
            set { _PanelText = value; }
        }

        /// <summary>
        /// 一页12个
        /// </summary>
        private int _Page_Count;
        public int Page_Count
        {
            get { return _Page_Count; }
            set { _Page_Count = value; }
        }

        private int _InitFlag;
        public int InitFlag
        {
            get { return _InitFlag; }
            set { _InitFlag = value; }
        }

        /// <summary>
        /// 零件类型
        /// </summary>
        private string _Type;
        public string m_Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private IList<Button> _BtnList;
        public IList<Button> BtnList
        {
            get { return _BtnList; }
            set { _BtnList = value; }
        }

        private IList<Node> _NodesList;
        public IList<Node> NodesList       //所有零件的集合
        {
            get { return _NodesList; }
            set { _NodesList = value; }
        }

        private string FirstNode = "底盘平台";

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

            Page_Count = 12;

            InitFlag = 0;

            BtnList = new List<Button>();          //零件按钮集合

            NodesList = new List<Node>();            //所有零件的集合
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
        public void SetIndex(Node node)
        {

        }

        public int GetIndex(Node node)
        {
            return 0;
        }

        /// <summary>
        /// 判断某个零件是否显示在零件架上
        /// </summary>
        public bool IsView(Node node)
        {
            return false;
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
        public void BindGridItem(Transform trans, Node gridItem)
        {

        }

        /// <summary>
        /// 类型变化，刷新零件界面
        /// </summary>
        /// <param name="type"></param>
        public void RefreshItems()
        {

        }

        /// <summary>
        /// 零件按钮点击事件
        /// </summary>
        public void BtnClick(GameObject go)
        {

        }

        #region  按钮效果
        /// <summary>
        /// 让按钮看起来不能被点击，仅仅只是UI显示方面
        /// </summary>
        /// <param name="go"></param>
        public void DisButton(GameObject go)
        {

        }

        /// <summary>
        /// 让按钮看起来能被点击，仅仅只是UI显示方面
        /// </summary>
        /// <param name="go"></param>
        public void AbleButton(GameObject go)
        {

        }
        #endregion

        public Vector3 GetBoxColliderSize(GameObject go)
        {
            return Vector3.zero;
        }

    }
}