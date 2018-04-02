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

    public class UIPartsPanelClassStudyModel : MonoBehaviour, UIPartsPanelClass
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

        public UIPartsPageStudyModel _UIPartsPage;


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

            if (null != NodesCommonStudyModel.Instance)
            {
                NodeTypesList = NodesCommonStudyModel.Instance.GetNodesTypes();  //初始化
            }
            else
            {
                Debug.LogError("NodesCommon没有初始化！");
            }

            _UIPartsPage = GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel").GetComponent<UIPartsPageStudyModel>();

            m_BtnNext = GameObject.Find("Canvas/BG/PartsPanel/NextIcon_Btn").GetComponent<Button>();
            m_BtnPrevious = GameObject.Find("Canvas/BG/PartsPanel/PreviousIcon_Btn").GetComponent<Button>();

            PartClass1 = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel/PartClass 1").GetComponent<Toggle>();
            PartClass2 = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel/PartClass 2").GetComponent<Toggle>();
            PartClass3 = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel/PartClass 3").GetComponent<Toggle>();

            //为上一页和下一页添加事件
            m_BtnNext.onClick.AddListener(() => { Next(); });
            m_BtnPrevious.onClick.AddListener(() => { Previous(); });

            PartClass1.onValueChanged.AddListener((Ison) => { PartClass1RefreshItems(Ison); });
            PartClass2.onValueChanged.AddListener((Ison) => { PartClass2RefreshItems(Ison); });
            PartClass3.onValueChanged.AddListener((Ison) => { PartClass3RefreshItems(Ison); });

            InitItems();
        }

        /// <summary>
        /// 初始化元素
        /// </summary>
        public void InitItems()
        {
            m_ItemsList = new List<string>();
            //初始化零件数据
            for (int i = 0; i < NodeTypesList.Count; i++)
            {
                m_ItemsList.Add(NodeTypesList[i]);
            }
            //m_ItemsList.Reverse();

            //计算元素总个数
            m_ItemsCount = m_ItemsList.Count;
            //计算总页数
            m_PageCount = (m_ItemsCount % Page_Count) == 0 ? m_ItemsCount / Page_Count : (m_ItemsCount / Page_Count) + 1;

            BindPage(m_PageIndex);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public void Next()
        {
            if (m_PageCount <= 0)
                return;
            //最后一页禁止向后翻页
            if (m_PageIndex >= m_PageCount)
                return;

            m_PageIndex += 1;
            if (m_PageIndex >= m_PageCount)
                m_PageIndex = m_PageCount;

            BindPage(m_PageIndex);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public void Previous()
        {
            if (m_PageCount <= 0)
                return;
            //第一页时禁止向前翻页
            if (m_PageIndex <= 1)
                return;
            m_PageIndex -= 1;
            if (m_PageIndex < 1)
                m_PageIndex = 1;

            BindPage(m_PageIndex);
        }

        /// <summary>
        /// 指定跳转相应页数
        /// </summary>
        public int SetIndex(string type)
        {
            int pageIndex = 1;
            int index = 0;
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                if (type == m_ItemsList[i])
                {
                    pageIndex = (i + 1) / Page_Count;
                    index = i;
                    break;
                }
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            m_PageIndex = pageIndex;
            BindPage(pageIndex);

            return index;
        }

        /// <summary>
        /// 获取当前零件再第几页
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int GetIndex(string type)
        {
            int pageIndex = 1;
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                if (type == m_ItemsList[i])
                {
                    pageIndex = i / Page_Count + 1;
                    break;
                }
            }
            m_PageIndex = pageIndex;
            return pageIndex;
        }

        /// <summary>
        /// 绑定指定索引处的页面元素
        /// </summary>
        /// <param name="index">页面索引</param>
        public void BindPage(int index)
        {
            //列表处理
            if (m_ItemsList == null || m_ItemsCount <= 0)
                return;

            //索引处理
            if (index < 0 || index > m_ItemsCount)
                return;

            PartClass1.isOn = true;

            //按照元素个数可以分为1页和1页以上两种情况
            if (m_PageCount == 1)
            {
                int canDisplay = 0;
                for (int i = Page_Count; i > 0; i--)
                {
                    if (canDisplay < m_ItemsCount)
                    {
                        BindGridItem(transform.GetChild(canDisplay), m_ItemsList[Page_Count - i]);
                        transform.GetChild(canDisplay).gameObject.SetActive(true);
                    }
                    else
                    {
                        //对超过canDispaly的物体实施隐藏
                        transform.GetChild(canDisplay).gameObject.SetActive(false);
                    }
                    canDisplay += 1;
                }
            }
            else if (m_PageCount > 1)
            {
                //1页以上需要特别处理的是最后1页
                //和1页时的情况类似判断最后一页剩下的元素数目
                //第1页时显然剩下的为Page_Count所以不用处理
                if (index == m_PageCount)
                {
                    int canDisplay = 0;
                    for (int i = Page_Count; i > 0; i--)
                    {
                        //最后一页剩下的元素数目为 m_ItemsCount - Page_Count * (index-1)
                        if (canDisplay < m_ItemsCount - Page_Count * (index - 1))
                        {
                            BindGridItem(transform.GetChild(canDisplay), m_ItemsList[Page_Count * index - i]);
                            transform.GetChild(canDisplay).gameObject.SetActive(true);
                        }
                        else
                        {
                            //对超过canDispaly的物体实施隐藏
                            transform.GetChild(canDisplay).gameObject.SetActive(false);
                        }
                        canDisplay += 1;
                    }
                }
                else
                {
                    for (int i = Page_Count; i > 0; i--)
                    {
                        BindGridItem(transform.GetChild(Page_Count - i), m_ItemsList[Page_Count * index - i]);
                        transform.GetChild(Page_Count - i).gameObject.SetActive(true);
                    }
                }
            }
        }


        /// <summary>
        /// 将一个GridItem实例绑定到指定的Transform上
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="gridItem"></param>
        public void BindGridItem(Transform trans, string gridItem)
        {
            trans.Find("Label").GetComponent<Text>().text = gridItem;
            if (m_Type != PartClass1.transform.Find("Label").GetComponent<Text>().text)     //确保只刷新一次
            {
                m_Type = PartClass1.transform.Find("Label").GetComponent<Text>().text;
                _UIPartsPage.RefreshItems();
            }
        }

        /// <summary>
        /// 类型变化，刷新零件界面
        /// </summary>
        /// <param name="type"></param>
        public void PartClass1RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = PartClass1.transform.Find("Label").GetComponent<Text>().text;
                _UIPartsPage.InitFlag = 0;
                _UIPartsPage.RefreshItems();
            }
        }

        public void PartClass2RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = PartClass2.transform.Find("Label").GetComponent<Text>().text;
                _UIPartsPage.InitFlag = 0;
                _UIPartsPage.RefreshItems();
            }
        }

        public void PartClass3RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = PartClass3.transform.Find("Label").GetComponent<Text>().text;
                _UIPartsPage.InitFlag = 0;
                _UIPartsPage.RefreshItems();
            }
        }

        /// <summary>
        /// 设置零件类型
        /// </summary>
        /// <param name="type"></param>
        public void SetPartsType(Node node)
        {
            string type = node.Type;
            m_Type = type;
            int index = SetIndex(type) + 1;
            if (index % Page_Count == 1)
            {
                PartClass1.isOn = true;
                m_Type = PartClass1.transform.Find("Label").GetComponent<Text>().text;
            }
            else if (index % Page_Count == 2)
            {
                PartClass2.isOn = true;
                m_Type = PartClass2.transform.Find("Label").GetComponent<Text>().text;
            }
            else
            {
                PartClass3.isOn = true;
                m_Type = PartClass3.transform.Find("Label").GetComponent<Text>().text;

            }

            _UIPartsPage.RefreshItems();
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