/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 分页功能
/// </summary>
namespace WyzLink.Utils.ModelDataHelper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Test;

    public class PaginationUtil : MonoBehaviour
    {
        /// <summary>
        /// 当前页面索引
        /// </summary>
        private int m_PageIndex = 1;

        /// <summary>
        /// 总页数
        /// </summary>
        private int m_PageCount = 0;

        /// <summary>
        /// 元素总个数
        /// </summary>
        private int m_ItemsCount = 0;

        /// <summary>
        /// 元素列表
        /// </summary>
        private List<Parts> m_ItemsList;

        /// <summary>
        /// 上一页
        /// </summary>
        private Button m_BtnPrevious;

        /// <summary>
        /// 下一页
        /// </summary>
        private Button m_BtnNext;

        /// <summary>
        /// 显示当前页数的标签
        /// </summary>
        private Text m_PanelText;

        void Start()
        {
            InitGUI();
            InitItems();
        }

        /// <summary>
        /// 初始化GUI
        /// </summary>
        private void InitGUI()
        {
            m_BtnNext = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/NextBtn").GetComponent<Button>();
            m_BtnPrevious = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PreviousBtn").GetComponent<Button>();
            m_PanelText = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PageText").GetComponent<Text>();

            //为上一页和下一页添加事件
            m_BtnNext.onClick.AddListener(() => { Next(); });
            m_BtnPrevious.onClick.AddListener(() => { Previous(); });

        }

        /// <summary>
        /// 初始化元素
        /// </summary>
        private void InitItems()
        {
            m_ItemsList = new List<Parts>();
            for (int i = 0; i < GlobalVar._PartsManager.PartsList.Count; i++)
            {
                m_ItemsList.Add(GlobalVar._PartsManager.PartsList[i]);
            }

            //计算元素总个数
            m_ItemsCount = m_ItemsList.Count;
            //计算总页数
            m_PageCount = (m_ItemsCount % 6) == 0 ? m_ItemsCount / 6 : (m_ItemsCount / 6) + 1;

            BindPage(m_PageIndex);
            //更新界面页数
            m_PanelText.text = string.Format("第" + "{0}/{1}" + "页", m_PageIndex.ToString(), m_PageCount.ToString());
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

            //更新界面页数
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
            if (m_PageIndex <= 1)
                return;
            m_PageIndex -= 1;
            if (m_PageIndex < 1)
                m_PageIndex = 1;

            BindPage(m_PageIndex);

            //更新界面页数
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

            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                if (0 == m_ItemsList[i].AssebleStaus)
                {
                    m_ItemsList[i].PartsGameObject.SetActive(false);
                }
            }

            //按照元素个数可以分为1页和1页以上两种情况
            if (m_PageCount == 1)
            {
                int canDisplay = 0;
                for (int i = 6; i > 0; i--)
                {
                    if (canDisplay < 6)
                    {
                        BindGridItem(transform.GetChild(canDisplay), m_ItemsList[6 - i]);
                        transform.GetChild(canDisplay).gameObject.SetActive(true);
                        m_ItemsList[6 - i].PartsGameObject.SetActive(true);
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
                //第1页时显然剩下的为6所以不用处理
                if (index == m_PageCount)
                {
                    int canDisplay = 0;
                    for (int i = 6; i > 0; i--)
                    {
                        //最后一页剩下的元素数目为 m_ItemsCount - 6 * (index-1)
                        if (canDisplay < m_ItemsCount - 6 * (index - 1))
                        {
                            BindGridItem(transform.GetChild(canDisplay), m_ItemsList[6 * index - i]);
                            transform.GetChild(canDisplay).gameObject.SetActive(true);
                            m_ItemsList[6 * index - i].PartsGameObject.SetActive(true);
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
                    for (int i = 6; i > 0; i--)
                    {
                        BindGridItem(transform.GetChild(6 - i), m_ItemsList[6 * index - i]);
                        transform.GetChild(6 - i).gameObject.SetActive(true);
                        m_ItemsList[6 * index - i].PartsGameObject.SetActive(true);
                    }
                }
            }
        }
        int a = 0;
        /// <summary>
        /// 将一个GridItem实例绑定到指定的Transform上
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="gridItem"></param>
        private void BindGridItem(Transform trans, Parts gridItem)
        {
            //trans.GetComponent<Image>().sprite = LoadSprite(gridItem.ItemSprite);
            trans.Find("Text").GetComponent<Text>().text = gridItem.Name;

            //if (4 == a)
            //{
            //    GameObject.Find("Part__Feature004").transform.position = trans.GetChild(1).transform.position + (GameObject.Find("Part__Feature004").transform.position - new Vector3(GameObject.Find("Part__Feature004").GetComponent<MeshFilter>().mesh.bounds.center.x / 10, GameObject.Find("Part__Feature004").GetComponent<MeshFilter>().mesh.bounds.center.y / 10, GameObject.Find("Part__Feature").GetComponent<MeshFilter>().mesh.bounds.center.z / 10));
            //}
            //a++;

            // gridItem.PartsGameObject.transform.position = trans.GetChild(1).transform.position + (gridItem.PartsGameObject.transform.position - new Vector3(gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.x / 10, gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.y / 10, gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.z / 10));
            //gridItem.PartsGameObject.transform.position = gridItem.PartsGameObject.transform.position + new Vector3(gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.x / 10, gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.y / 10, gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.z / 10);
            //GameObject.Find("Part__Feature").GetComponent<MeshFilter>().mesh.bounds.center = trans.GetChild(1).transform.position;
            //GameObject.Find("Part__Feature").transform.position = trans.GetChild(1).transform.position;

            gridItem.PartsGameObject.transform.position = trans.GetChild(1).transform.position + (gridItem.PartsGameObject.transform.position - new Vector3(gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.x / 10, gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.y / 10, gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.z / 10));
            //gridItem.PartsGameObject.transform.position = gridItem.PartsGameObject.transform.position - new Vector3(gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.x / 10, gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.y / 10, gridItem.PartsGameObject.GetComponent<MeshFilter>().mesh.bounds.center.z / 10);
        }
    }
}