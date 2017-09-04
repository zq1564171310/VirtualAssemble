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
    using UnityEngine.UI;
    using WyzLink.Control;
    using WyzLink.Common;
    using System;

    public class UIToolsClassPanel : MonoBehaviour
    {
        
        private static OnReceivedTools _OnReceivedTools = new OnReceivedTools();

        private Toggle ToolsClass1, ToolsClass2, ToolsClass3;//工具类型的三个开关

        private Button m_BtnPrevious, m_BtnNext;//左右翻动工具类别的按钮

        private List<string> ToolsTypeList = new List<string>();//工具类型集合

        private string m_Type;//当前选中的工具类型

        private int m_PageIndex = 1; //类型当前页面索引，初始时为第一页

        private int m_PageCount = 0;//类型总页数

        private int m_ItemsCount = 0;//类型总个数

        private int ToolTypeCount = 3;//每个工具类型的个数

        private bool InitFlag;
        private bool flag;

        // Use this for initialization
        void Start()
        {
            _OnReceivedTools = GameObject.Find("Canvas/BG/ToolsPanel/SingleToolPanel").GetComponent<OnReceivedTools>();
            if (flag == false)
            {
                if (null != ToolsCommon.Instance)
                {
                    ToolsTypeList = ToolsCommon.Instance.GetToolTypes();  //初始化
                }
                else
                {
                    Debug.LogError("ToolsCommon没有初始化！");
                }
                Init();
                InitItems();
                flag = true;
            }
        }

       /// <summary>
       /// 初始化界面各个按钮
       /// </summary>
        private void Init()
        {
            GameObject _m_BtnNext = GameObject.Find("Canvas/BG/ToolsPanel/PreviousIcon_Btn");
            m_BtnNext = GameObject.Find("Canvas/BG/ToolsPanel/NextIcon_Btn").GetComponent<Button>();
            m_BtnPrevious = GameObject.Find("Canvas/BG/ToolsPanel/PreviousIcon_Btn").GetComponent<Button>();
            ToolsClass1 = GameObject.Find("Canvas/BG/ToolsPanel/ToolsClassPanel/ToolsClass 1").GetComponent<Toggle>();
            ToolsClass2 = GameObject.Find("Canvas/BG/ToolsPanel/ToolsClassPanel/ToolsClass 2").GetComponent<Toggle>();
            ToolsClass3 = GameObject.Find("Canvas/BG/ToolsPanel/ToolsClassPanel/ToolsClass 3").GetComponent<Toggle>();

            //为上一页和下一页添加事件
            m_BtnNext.onClick.AddListener(() => { Next(); });
            m_BtnPrevious.onClick.AddListener(() => { Previous(); });

            //为类型开关添加监听事件
            ToolsClass1.onValueChanged.AddListener((Ison) => { ToolsClass1RefreshItems(Ison); });
            ToolsClass2.onValueChanged.AddListener((Ison) => { ToolsClass2RefreshItems(Ison); });
            ToolsClass3.onValueChanged.AddListener((Ison) => { ToolsClass3RefreshItems(Ison); });
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitItems()
        {
            //计算元素总个数
            if (ToolsTypeList.Count > 0)
                m_ItemsCount = ToolsTypeList.Count;
            else
                return;

            //计算总页数
            m_PageCount = (m_ItemsCount % ToolTypeCount) == 0 ? m_ItemsCount / ToolTypeCount : (m_ItemsCount / ToolTypeCount) + 1;

            BindPage(m_PageIndex);
        }

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
        /// 绑定指定索引处的页面元素
        /// </summary>
        /// <param name="index">页面索引</param>
        private void BindPage(int index)
        {
            //列表处理
            if (ToolsTypeList == null || m_ItemsCount <= 0)
                return;

            //索引处理
            if (index < 0 || index > m_ItemsCount)
                return;

            ToolsClass1.isOn = true;

            //按照元素个数可以分为1页和1页以上两种情况
            if (m_PageCount == 1)
            {
                int canDisplay = 0;
                for (int i = ToolTypeCount; i > 0; i--)
                {
                    if (canDisplay < m_ItemsCount)
                    {
                        BindGridItem(transform.GetChild(canDisplay), ToolsTypeList[ToolTypeCount - i]);
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
                if (index == m_PageCount)
                {
                    int canDisplay = 0;
                    for (int i = ToolTypeCount; i > 0; i--)
                    {
                        //最后一页剩下的元素数目为 m_ItemsCount - ToolTypeCount * (index-1)
                        //ToolTypeCount * index - i最后一页第一个类型在ToolsTypeList中的索引
                        if (canDisplay < m_ItemsCount - ToolTypeCount * (index - 1))
                        {
                            BindGridItem(transform.GetChild(canDisplay), ToolsTypeList[ToolTypeCount * index - i]);
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
                    for (int i = ToolTypeCount; i > 0; i--)
                    {
                        BindGridItem(transform.GetChild(ToolTypeCount - i), ToolsTypeList[ToolTypeCount * index - i]);
                        transform.GetChild(ToolTypeCount - i).gameObject.SetActive(true);
                    }
                }
            }
        }

        /// <summary>
        /// 将一个GridItem实例绑定到指定的Transform上
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="gridItem"></param>
        private void BindGridItem(Transform trans, string gridItem)
        {
            trans.Find("Label").GetComponent<Text>().text = gridItem;
            if (m_Type != ToolsClass1.transform.Find("Label").GetComponent<Text>().text)     //确保只刷新一次
            {
                m_Type = ToolsClass1.transform.Find("Label").GetComponent<Text>().text;
                _OnReceivedTools.RefreshItems();
            }
        }


        /// <summary>
        /// 类型变化，刷新工具界面
        /// </summary>
        /// <param name="type"></param>
        public void ToolsClass1RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = ToolsClass1.transform.Find("Label").GetComponent<Text>().text;
                _OnReceivedTools.RefreshItems();
            }
        }

        public void ToolsClass2RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = ToolsClass2.transform.Find("Label").GetComponent<Text>().text;
                _OnReceivedTools.RefreshItems();
            }
        }

        public void ToolsClass3RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = ToolsClass3.transform.Find("Label").GetComponent<Text>().text;
                _OnReceivedTools.RefreshItems();
            }
        }

        /// <summary>
        /// 设置工具类型
        /// </summary>
        /// <param name="type"></param>
        public void SetToolsType(string type)
        {
            //m_Type = type;
        }


        /// <summary>
        /// 获取工具类型
        /// </summary>
        /// <returns>
        /// 返回string类型
        /// </returns>
        public string GetToolsType()
        {
            return m_Type;
        }
    }
}