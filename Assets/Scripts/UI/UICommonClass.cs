﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
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
    using WyzLink.Common;

    public class UICommonClass : MonoBehaviour
    {
        private static UICommonParts _UICommonParts;

        private Toggle PartClass1, PartClass2, PartClass3;//类型的三个开关

        private Button m_BtnPrevious, m_BtnNext;//左右翻动类别的按钮

        private List<string> ComPartTypeList = new List<string>();//类型集合

        private string m_Type;//当前选中的类型

        private int m_PageIndex = 1; //类型当前页面索引，初始时为第一页

        private int m_PageCount = 0;//类型总页数

        private int m_ItemsCount = 0;//类型总个数

        private int ComPartTypeCount = 3;//每页类型的个数

        private bool InitFlag;

        // Use this for initialization
        void Start()
        {

        }

        /// <summary>
        /// 初始化界面各个按钮
        /// </summary>
        public void Init()
        {
            if (null != CommonPartsCommon.Instance)
            {
                ComPartTypeList = CommonPartsCommon.Instance.GetCommonPartsTypes();  //初始化
            }
            else
            {
                Debug.LogError("CommonPartsCommon没有初始化！");
            }

            _UICommonParts = GameObject.Find("Canvas/BG/CommonPartsPanel/PartPanel").GetComponent<UICommonParts>();

            m_BtnNext = GameObject.Find("Canvas/BG/CommonPartsPanel/NextIcon").GetComponent<Button>();
            m_BtnPrevious = GameObject.Find("Canvas/BG/CommonPartsPanel/PreviousIcon").GetComponent<Button>();

            PartClass1 = GameObject.Find("Canvas/BG/CommonPartsPanel/ClassPanel/PartClass 1").GetComponent<Toggle>();
            PartClass2 = GameObject.Find("Canvas/BG/CommonPartsPanel/ClassPanel/PartClass 2").GetComponent<Toggle>();
            PartClass3 = GameObject.Find("Canvas/BG/CommonPartsPanel/ClassPanel/PartClass 3").GetComponent<Toggle>();

            //为上一页和下一页添加事件
            m_BtnNext.onClick.AddListener(() => { Next(); });
            m_BtnPrevious.onClick.AddListener(() => { Previous(); });

            //为类型开关添加监听事件
            PartClass1.onValueChanged.AddListener((Ison) => { PartClass1RefreshItems(Ison); });
            PartClass2.onValueChanged.AddListener((Ison) => { PartClass2RefreshItems(Ison); });
            PartClass3.onValueChanged.AddListener((Ison) => { PartClass3RefreshItems(Ison); });

            InitItems();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitItems()
        {
            //计算元素总个数
            if (ComPartTypeList.Count > 0)
                m_ItemsCount = ComPartTypeList.Count;
            else
                return;

            //计算总页数
            m_PageCount = (m_ItemsCount % ComPartTypeCount) == 0 ? m_ItemsCount / ComPartTypeCount : (m_ItemsCount / ComPartTypeCount) + 1;

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
            if (ComPartTypeList == null || m_ItemsCount <= 0)
                return;

            //索引处理
            if (index < 0 || index > m_ItemsCount)
                return;

            PartClass1.isOn = true;

            //按照元素个数可以分为1页和1页以上两种情况
            if (m_PageCount == 1)
            {
                int canDisplay = 0;
                for (int i = ComPartTypeCount; i > 0; i--)
                {
                    if (canDisplay < m_ItemsCount)
                    {
                        BindGridItem(transform.GetChild(canDisplay), ComPartTypeList[ComPartTypeCount - i]);
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
                    for (int i = ComPartTypeCount; i > 0; i--)
                    {
                        //最后一页剩下的元素数目为 m_ItemsCount - ToolTypeCount * (index-1)
                        //ComPartTypeCount * index - i最后一页第一个类型在ComPartTypeList中的索引
                        if (canDisplay < m_ItemsCount - ComPartTypeCount * (index - 1))
                        {
                            BindGridItem(transform.GetChild(canDisplay), ComPartTypeList[ComPartTypeCount * index - i]);
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
                    for (int i = ComPartTypeCount; i > 0; i--)
                    {
                        BindGridItem(transform.GetChild(ComPartTypeCount - i), ComPartTypeList[ComPartTypeCount * index - i]);
                        transform.GetChild(ComPartTypeCount - i).gameObject.SetActive(true);
                    }
                }
            }
        }

        /// <summary>
        /// 将一个类型实例绑定到指定的Transform上
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="gridItem"></param>
        private void BindGridItem(Transform trans, string gridItem)
        {
            trans.Find("Label").GetComponent<Text>().text = gridItem;
            if (m_Type != PartClass1.transform.Find("Label").GetComponent<Text>().text)     //确保只刷新一次
            {
                m_Type = PartClass1.transform.Find("Label").GetComponent<Text>().text;
                _UICommonParts.RefreshItems();
            }
        }


        /// <summary>
        /// 类型变化，刷新常用零件界面
        /// </summary>
        /// <param name="type"></param>
        public void PartClass1RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = PartClass1.transform.Find("Label").GetComponent<Text>().text;
                _UICommonParts.RefreshItems();
            }
        }

        public void PartClass2RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = PartClass2.transform.Find("Label").GetComponent<Text>().text;
                _UICommonParts.RefreshItems();
            }
        }

        public void PartClass3RefreshItems(bool Ison)
        {
            if (true == Ison)
            {
                m_Type = PartClass3.transform.Find("Label").GetComponent<Text>().text;
                _UICommonParts.RefreshItems();
            }
        }

        /// <summary>
        /// 设置常用零件类型
        /// </summary>
        /// <param name="type"></param>
        public void SetComPartType(string type)
        {
            //m_Type = type;
        }


        /// <summary>
        /// 获取常用零件类型
        /// </summary>
        /// <returns>
        /// 返回string类型
        /// </returns>
        public string GetComPartType()
        {
            return m_Type;
        }
    }
}