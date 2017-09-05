/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>pixiaoli</author>
/// <summary>
/// UI 逻辑
/// </summary>

namespace WyzLink.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Manager;
    using WyzLink.ToolsAndCommonParts;
    using HoloToolkit.Unity;
    using WyzLink.Common;
    using UnityEngine.UI;

    public class UICommonParts : MonoBehaviour
    {
        private UICommonClass _UICommonClass = new UICommonClass();

        private List<CommonParts> CommonPartsList = new List<CommonParts>();//所有常用零件集合
        private List<string> CommonPartsType = new List<string>();//所有常用零件类型集合
        private List<Button> BtnList = new List<Button>();//常用零件按钮集合
        private List<CommonParts> m_ItemsList = new List<CommonParts>();//属于当前类型的所有常用零件集合
        private string m_Type;//当前常用零件类型

        private int m_PageIndex = 1;//属于当前类型的常用零件页面索引
        private int m_PageCount = 0;//属于当前类型的常用零件总页数
        private int m_ItemsCount = 0;//属于当前类型的常用零件元素总个数

        private Button PreviousPage;//上一页
        private Button NextPage;// 下一页
        private Text m_PanelText;//用来显示当前页数
        private int Page_Count = 12;//每页最多可放常用零件个数

        private int InitFlag = 0;
        private bool Init;

        // Use this for initialization
        void Awake()
        {
            _UICommonClass = GameObject.Find("Canvas/BG/CommonPartsPanel/PartPanel").GetComponent<UICommonClass>();
            //RefreshItems();
        }

        void Start()
        {
            RefreshItems();
        }


        void Update()
        {
            if (InitFlag <= 2)   //第二帧布局的坐标才正常，之前btn坐标都是一样的，自动布局的问题，后续想办法修改该问题
            {
                RefreshItems();
                InitFlag++;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public void Next()
        {
            if (m_PageCount <= 0)
                return;
            //最后一页禁止向后翻页
            if (m_PageIndex > m_PageCount)
                return;

            m_PageIndex += 1;
            if (m_PageIndex >= m_PageCount)
                m_PageIndex = m_PageCount;

            BindPage(m_PageIndex);
            m_PanelText.text = string.Format("第" + "{0}/{1}" + "页", m_PageIndex.ToString(), m_PageCount.ToString());
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public void Previous()
        {
            if (m_PageCount <= 0)
                return;
            //第一页时禁止向前翻页
            if (m_PageIndex < 1)
                return;
            m_PageIndex -= 1;
            if (m_PageIndex <= 1)
                m_PageIndex = 1;

            BindPage(m_PageIndex);
            m_PanelText.text = string.Format("第" + "{0}/{1}" + "页", m_PageIndex.ToString(), m_PageCount.ToString());
        }


        /// <summary>
        /// 绑定指定索引处的页面元素
        /// </summary>
        /// <param name="index">页面索引</param>
        private void BindPage(int index)
        {

            //列表处理
            if (m_ItemsList == null || m_ItemsCount <= 0)
                return;

            //索引处理
            if (index < 0 || index > m_ItemsCount)
                return;

            //先将工具全部隐藏
            for (int i = 0; i < CommonPartsList.Count; i++)
            {
                CommonPartsList[i].gameObject.SetActive(false);
            }

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
                        m_ItemsList[Page_Count - i].gameObject.SetActive(true);
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
                            m_ItemsList[Page_Count * index - i].gameObject.SetActive(true);
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
                        m_ItemsList[Page_Count * index - i].gameObject.SetActive(true);
                    }
                }
            }
        }

        /// <summary>
        /// 将一个GridItem实例绑定到指定的Transform上
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="gridItem"></param>
        private void BindGridItem(Transform trans, CommonParts gridItem)
        {
            trans.Find("Text").GetComponent<Text>().text = gridItem.CommonPartsName;

            gridItem.gameObject.transform.position = trans.GetChild(1).transform.position;

        }

        /// <summary>
        /// 类型变化，刷新常用零件界面
        /// </summary>
        /// <param name="type"></param>
        public void RefreshItems()
        {
            if (false == Init)                   //初始化
            {
                if (null != CommonPartsCommon.Instance)
                {
                    CommonPartsList = CommonPartsCommon.Instance.GetCommonPartsList();
                    CommonPartsType = CommonPartsCommon.Instance.GetCommonPartsTypes();
                }
                else
                {
                    Debug.LogError("CommonPartsCommon没有初始化！");
                }

                NextPage = GameObject.Find("Canvas/BG/CommonPartsPanel/NexPage").GetComponent<Button>();
                PreviousPage = GameObject.Find("Canvas/BG/CommonPartsPanel/PrePage").GetComponent<Button>();
                m_PanelText = GameObject.Find("Canvas/BG/CommonPartsPanel/ViewPage").GetComponent<Text>();

                //为上一页和下一页添加事件
                NextPage.onClick.AddListener(() => { Next(); });
                PreviousPage.onClick.AddListener(() => { Previous(); });

                foreach (Transform tran in transform)
                {
                    BtnList.Add(tran.gameObject.GetComponent<Button>());
                    //EventTriggerListener.Get(tran.gameObject).onClick = BtnClick;
                }
                Init = true;
            }

            if (null != GameObject.Find("Canvas/BG/CommonPartsPanel/ClassPanel"))
            {
                m_Type = GameObject.Find("Canvas/BG/CommonPartsPanel/ClassPanel").GetComponent<UICommonClass>().GetComPartType();
            }

            if (null == m_ItemsList)
            {
                m_ItemsList = new List<CommonParts>();
            }
            else
            {
                m_ItemsList.Clear();           //清空前一类型的数据
            }

            //把属于当前类型的常用零件加入m_ItemsList集合
            for (int i = 0; i < CommonPartsList.Count; i++)
            {
                if (m_Type == CommonPartsList[i].Type)
                {
                    m_ItemsList.Add(CommonPartsList[i]);
                }

            }
            //计算元素总个数
            m_ItemsCount = m_ItemsList.Count;
            //计算总页数
            m_PageCount = (m_ItemsCount % Page_Count) == 0 ? m_ItemsCount / Page_Count : (m_ItemsCount / Page_Count) + 1;

            m_PageIndex = 1;  //每次刷新都必须把页面重置

            BindPage(m_PageIndex);
            m_PanelText.text = string.Format("第" + "{0}/{1}" + "页", m_PageIndex.ToString(), m_PageCount.ToString());

        }

    }
}