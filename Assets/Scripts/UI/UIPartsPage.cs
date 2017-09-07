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
    using WyzLink.Manager;
    using WyzLink.Parts;
    using WyzLink.Utils.Overriding;

    public class UIPartsPage : MonoBehaviour
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
        private List<Node> m_ItemsList;

        /// <summary>
        /// 上一页
        /// </summary>
        private Button PreviousPage;

        /// <summary>
        /// 下一页
        /// </summary>
        private Button NextPage;

        /// <summary>
        /// 显示当前页数的标签
        /// </summary>
        private Text m_PanelText;

        /// <summary>
        /// 一页3个
        /// </summary>
        private int Page_Count = 12;

        private int InitFlag = 0;

        /// <summary>
        /// 零件类型
        /// </summary>
        private string m_Type;

        private List<Button> BtnList = new List<Button>();          //零件按钮集合

        private List<Node> NodesList = new List<Node>();            //所有零件类型的集合

        // Use this for initialization
        void Start()
        {

        }

        void Update()
        {
            if (InitFlag <= 2)   //第二帧布局的坐标才正常，之前btn坐标都是一样的，自动布局的问题，后续想办法修改该问题
            {
                RefreshItems();
                InitFlag++;
            }
        }

        public void Init()
        {
            if (null != NodesCommon.Instance)
            {
                NodesList = NodesCommon.Instance.GetNodesList();
            }
            else
            {
                Debug.LogError("NodesCommon没有初始化！");
            }

            NextPage = GameObject.Find("Canvas/BG/PartsPanel/NextPage").GetComponent<Button>();
            PreviousPage = GameObject.Find("Canvas/BG/PartsPanel/PreviousPage").GetComponent<Button>();
            m_PanelText = GameObject.Find("Canvas/BG/PartsPanel/ViewPage_Text").GetComponent<Text>();

            //为上一页和下一页添加事件
            NextPage.onClick.AddListener(() => { Next(); });
            PreviousPage.onClick.AddListener(() => { Previous(); });

            foreach (Transform tran in transform)
            {
                BtnList.Add(tran.gameObject.GetComponent<Button>());
                EventTriggerListener.Get(tran.gameObject).onClick = BtnClick;
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
        /// 指定跳转相应页数
        /// </summary>
        public void SetIndex(Node node)
        {
            int pageIndex = 1;
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                if (m_ItemsList[i].nodeId == node.nodeId)
                {
                    pageIndex = i / Page_Count + 1;
                    break;
                }
            }
            m_PageIndex = pageIndex;
            BindPage(m_PageIndex);

            //更新界面页数
            m_PanelText.text = string.Format("第" + "{0}/{1}" + "页", m_PageIndex.ToString(), m_PageCount.ToString());
        }

        public int GetIndex(Node node)
        {
            int pageIndex = 1;
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                if (m_ItemsList[i].nodeId == node.nodeId)
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
        private void BindPage(int index)
        {

            //列表处理
            if (m_ItemsList == null || m_ItemsCount <= 0)
                return;

            //索引处理
            if (index < 0 || index > m_ItemsCount)
                return;

            for (int i = 0; i < NodesList.Count; i++)
            {
                //if (InstallationState.NotInstalled == NodesList[i].GetInstallationState() || InstallationState.NextInstalling == NodesList[i].GetInstallationState())
                if (NodesList[i].partName != "底盘平台")
                {
                    NodesList[i].gameObject.SetActive(false);
                }
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
        private void BindGridItem(Transform trans, Node gridItem)
        {
            trans.Find("Text").GetComponent<Text>().text = gridItem.partName;
            if (InstallationState.NextInstalling == gridItem.gameObject.GetComponent<Node>().GetInstallationState() || InstallationState.NotInstalled == gridItem.gameObject.GetComponent<Node>().GetInstallationState())
            {
                gridItem.gameObject.transform.position = trans.GetChild(1).transform.position;
            }
        }

        /// <summary>
        /// 类型变化，刷新零件界面
        /// </summary>
        /// <param name="type"></param>
        public void RefreshItems()
        {
            if (null != GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel"))
            {
                m_Type = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel").GetComponent<UIPartsPanelClass>().GetPartsType();
            }

            if (null == m_ItemsList)
            {
                m_ItemsList = new List<Node>();
            }
            else
            {
                m_ItemsList.Clear();           //清空前一类型的数据
            }
            for (int i = 0; i < NodesList.Count; i++)
            {
                if ((InstallationState.NextInstalling == NodesList[i].GetInstallationState() || InstallationState.NotInstalled == NodesList[i].GetInstallationState()) && m_Type == NodesList[i].Type)
                {
                    m_ItemsList.Add(NodesList[i]);
                }
            }
            //计算元素总个数
            m_ItemsCount = m_ItemsList.Count;
            //计算总页数
            m_PageCount = (m_ItemsCount % Page_Count) == 0 ? m_ItemsCount / Page_Count : (m_ItemsCount / Page_Count) + 1;

            m_PageIndex = 1;  //每次刷新都必须把页面重置，防止新的页面页数不够

            BindPage(m_PageIndex);
            m_PanelText.text = string.Format("第" + "{0}/{1}" + "页", m_PageIndex.ToString(), m_PageCount.ToString());


        }

        /// <summary>
        /// 零件按钮点击事件
        /// </summary>
        private void BtnClick(GameObject go)
        {
            Node node;            //被点击的零件
            IEnumerable<Node> NextInstallNode = AssembleManager.Instance.GetNextInstallNode();
            GameObject gameobj;                         //克隆一份,作为安装的零件
            GameObject gameOb;                          //克隆一份作为提示
            GameObject gameObSec;                       //克隆一份作为第二工作区提示
            GameObject Txt;                             //克隆一份文本
            Transform[] trans;

            if (EntryMode.GeAssembleModel() != AssembleModel.ExamModel)
            {
                for (int i = 0; i < BtnList.Count; i++)
                {
                    if (BtnList[i].name == go.name)
                    {
                        node = m_ItemsList[(m_PageIndex - 1) * Page_Count + i];
                        if (null != NextInstallNode)
                        {
                            foreach (Node nd in NextInstallNode)
                            {
                                if (nd.nodeId == node.nodeId && InstallationState.NextInstalling == node.GetInstallationState())  //此处点击多下会重复生成，bug后续修改
                                {
                                    gameobj = Instantiate(node.gameObject, node.gameObject.transform, true);
                                    gameobj.name = node.name;
                                    gameobj.transform.parent = GameObject.Find("RuntimeObject").transform;
                                    if (null != gameobj.GetComponent<MeshFilter>())
                                    {
                                        gameobj.transform.localScale = node.LocalSize;
                                    }

                                    gameOb = Instantiate(node.gameObject, node.gameObject.transform, true);
                                    gameOb.name = node.name + node.nodeId;
                                    gameOb.transform.localScale = node.LocalSize;
                                    gameOb.transform.parent = GameObject.Find("RuntimeObject").transform;
                                    gameOb.transform.position = node.EndPos;
                                    if (null != gameOb.GetComponent<MeshRenderer>())
                                    {
                                        gameOb.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;
                                    }
                                    else
                                    {
                                        trans = gameOb.GetComponentsInChildren<Transform>();
                                        foreach (Transform tran in trans)
                                        {
                                            if (null != tran.gameObject.GetComponent<MeshRenderer>())
                                            {
                                                tran.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;
                                            }
                                        }
                                    }
                                    if (null != gameOb.GetComponent<MeshFilter>())
                                    {
                                        gameOb.transform.localScale = node.LocalSize;
                                    }

                                    gameObSec = Instantiate(node.gameObject, node.gameObject.transform, true);
                                    gameObSec.name = node.name + "Sec" + node.nodeId;
                                    gameObSec.transform.localScale = node.LocalSize;
                                    gameObSec.transform.parent = GameObject.Find("RuntimeObject").transform;
                                    gameObSec.transform.position = node.WorSpaceRelativePos;
                                    if (null != gameObSec.GetComponent<MeshRenderer>())
                                    {
                                        gameObSec.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;
                                    }
                                    else
                                    {
                                        trans = gameObSec.GetComponentsInChildren<Transform>();
                                        foreach (Transform tran in trans)
                                        {
                                            if (null != tran.gameObject.GetComponent<MeshRenderer>())
                                            {
                                                tran.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;
                                            }
                                        }
                                    }
                                    if (null != gameObSec.GetComponent<MeshFilter>())
                                    {
                                        gameObSec.transform.localScale = node.LocalSize;
                                    }

                                    gameobj.gameObject.AddComponent<BoxCollider>();
                                    if (null == gameobj.gameObject.GetComponent<MeshFilter>())
                                    {
                                        gameobj.gameObject.GetComponent<BoxCollider>().size /= 10;
                                    }
                                    gameobj.gameObject.AddComponent<HandDraggable>();

                                    #region Test
                                    Vector3 var3 = gameobj.transform.position;
                                    StartCoroutine(OnMovesIEnumerator(gameobj, var3));
                                    #endregion

                                    #region Test 此处根据UI布局写活，后续需要根据UI调整
                                    Txt = Instantiate(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text"), GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1").transform, true);
                                    Txt.name = "Text" + node.nodeId;
                                    Txt.transform.position = node.gameObject.transform.position;
                                    Txt.GetComponent<Text>().text = node.gameObject.name;
                                    #endregion
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("当前可安装列表为空！");
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < BtnList.Count; i++)
                {
                    if (BtnList[i].name == go.name)
                    {
                        node = m_ItemsList[(m_PageIndex - 1) * Page_Count + i];

                        gameobj = Instantiate(node.gameObject, node.gameObject.transform, true);
                        gameobj.name = node.name;
                        gameobj.transform.parent = GameObject.Find("RuntimeObject").transform;
                        if (null != gameobj.GetComponent<MeshFilter>())
                        {
                            gameobj.transform.localScale = node.LocalSize;
                        }

                        gameobj.gameObject.AddComponent<BoxCollider>();
                        if (null == gameobj.gameObject.GetComponent<MeshFilter>())
                        {
                            gameobj.gameObject.GetComponent<BoxCollider>().size /= 10;
                        }
                        gameobj.gameObject.AddComponent<HandDraggable>();

                        #region Test
                        Vector3 var3 = gameobj.transform.position;
                        StartCoroutine(OnMovesIEnumerator(gameobj, var3));
                        #endregion

                        #region Test 此处根据UI布局写活，后续需要根据UI调整
                        Txt = Instantiate(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text"), GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1").transform, true);
                        Txt.name = "Text" + node.nodeId;
                        Txt.transform.position = node.gameObject.transform.position;
                        Txt.GetComponent<Text>().text = node.gameObject.name;
                        #endregion
                        break;
                    }
                }
            }
            //GlobalVar._Tips.text = "没有轮到该零件安装！";
        }

        IEnumerator OnMovesIEnumerator(GameObject _GameObject, Vector3 statPos)
        {
            float f = 0;
            while (true)
            {
                if (f <= 1)
                {
                    _GameObject.transform.position = Vector3.Lerp(statPos, new Vector3(1.7f, -0.4f, 4.5f), f);
                    f += Time.deltaTime;
                }
                else
                {
                    break;
                }
                yield return new WaitForSeconds(0);
            }
        }

    }
}