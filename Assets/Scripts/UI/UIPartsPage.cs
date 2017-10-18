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
        /// 一页12个
        /// </summary>
        private int Page_Count = 12;

        private int InitFlag = 0;

        private string FirstNode = "底盘平台";

        /// <summary>
        /// 零件类型
        /// </summary>
        private string m_Type;

        private List<Button> BtnList = new List<Button>();          //零件按钮集合

        private List<Node> NodesList = new List<Node>();            //所有零件的集合

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
        /// 判断某个零件是否显示在零件架上
        /// </summary>
        public bool IsView(Node node)
        {
            bool flag = false;
            if (node.Type != m_Type)
            {
                flag = false;
            }
            else
            {
                if (m_PageIndex == GetIndex(node))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
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
                if (FirstNode != NodesList[i].partName)      //第一个零件默认是安装好的,所以第一个零件不需要隐藏，其他零件隐藏起来
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
            gridItem.gameObject.transform.position = trans.GetChild(1).transform.position;
            trans.GetComponent<UIPartBtnStateManager>().RefreshData(gridItem);
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
                if (m_Type == NodesList[i].Type && FirstNode != NodesList[i].partName)
                {
                    m_ItemsList.Add(NodesList[i]);
                }
            }

            //计算元素总个数
            m_ItemsCount = m_ItemsList.Count;

            //计算总页数
            m_PageCount = (m_ItemsCount % Page_Count) == 0 ? m_ItemsCount / Page_Count : (m_ItemsCount / Page_Count) + 1;

            // if (m_PageIndex > m_PageCount)
            {
                m_PageIndex = 1;  //每次刷新都必须把页面重置，防止新的页面页数不够
            }

            BindPage(m_PageIndex);

            m_PanelText.text = string.Format("第" + "{0}/{1}" + "页", m_PageIndex.ToString(), m_PageCount.ToString());
        }

        /// <summary>
        /// 零件按钮点击事件
        /// </summary>
        private void BtnClick(GameObject go)
        {
            Node node;                                  //被点击的零件
            IEnumerable<Node> NextInstallNode = AssembleManager.Instance.GetNextInstallNode();
            GameObject gameobj;                         //克隆一份,作为安装的零件
            GameObject gameOb;                          //克隆一份作为提示
            GameObject gameObSec;                       //克隆一份作为第二工作区提示
            GameObject Txt;                             //克隆一份文本
            Transform[] trans;

            if (EntryMode.GetAssembleModel() != AssembleModel.ExamModel)
            {
                for (int i = 0; i < BtnList.Count; i++)
                {
                    if (BtnList[i].name == go.name)
                    {
                        node = m_ItemsList[(m_PageIndex - 1) * Page_Count + i];

                        if (InstallationState.Step1Installed == NodesCommon.Instance.GetInstallationState(node.nodeId) || InstallationState.Installed == NodesCommon.Instance.GetInstallationState(node.nodeId))
                        {
                            continue;
                        }

                        if (true == NodesCommon.Instance.IsPartInstallating())        //如果有零件正在安装
                        {
                            //此处要给提示
                            continue;
                        }

                        if (null != NextInstallNode)
                        {
                            foreach (Node nd in NextInstallNode)
                            {
                                if (nd.nodeId == node.nodeId && InstallationState.NextInstalling == node.GetInstallationState())
                                {
                                    gameobj = Instantiate(node.gameObject, node.gameObject.transform, true);
                                    gameobj.name = node.name;
                                    gameobj.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                                    if (null != gameobj.GetComponent<MeshFilter>())
                                    {
                                        gameobj.transform.localScale = node.LocalSize;
                                    }

                                    gameOb = Instantiate(node.gameObject, node.gameObject.transform, true);
                                    gameOb.name = node.name + node.nodeId;
                                    gameOb.transform.localScale = node.LocalSize;
                                    gameOb.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                                    gameOb.transform.position = node.EndPos;
                                    gameOb.transform.RotateAround(AssembleManager.Instance.GetRotaAngleCenter(), Vector3.up, AssembleManager.Instance.GetRotaAngle());
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
                                    gameObSec.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
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

                                    gameobj.AddComponent<NodeManager>();
                                    gameobj.AddComponent<BoxCollider>();
                                    if (null == gameobj.GetComponent<MeshFilter>())
                                    {
                                        gameobj.GetComponent<BoxCollider>().size /= 10;
                                    }
                                    gameobj.GetComponent<BoxCollider>().size = GetBoxColliderSize(gameobj);
                                    gameobj.AddComponent<HandDraggable>();
                                    gameobj.GetComponent<HandDraggable>().RotationMode = HandDraggable.RotationModeEnum.LockObjectRotation;

                                    StartCoroutine(OnMovesIEnumerator(gameobj, gameobj.transform.position));
                                    gameobj.transform.RotateAround(AssembleManager.Instance.GetRotaAngleCenter(), Vector3.up, AssembleManager.Instance.GetRotaAngle());

                                    Txt = Instantiate(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text"), GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1").transform, true);
                                    Txt.name = "Text" + node.nodeId;
                                    Txt.transform.position = node.gameObject.transform.position;
                                    Txt.GetComponent<Text>().text = node.gameObject.name;

                                    if (null != gameobj.GetComponent<Node>())
                                    {
                                        if (null != gameobj.GetComponent<HandDraggable>())
                                        {
                                            NodesCommon.Instance.SetInstallationState(node.GetComponent<Node>().nodeId, InstallationState.Step1Installed);
                                            AssembleManager.Instance.AddInstalledNodeList(gameobj.GetComponent<Node>());
                                            AssembleManager.Instance.SetInstalledNodeListStatus(gameobj.GetComponent<Node>(), InstallationState.Step1Installed);
                                        }
                                    }
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

                        if (InstallationState.Step1Installed == NodesCommon.Instance.GetInstallationState(node.nodeId) || InstallationState.Installed == NodesCommon.Instance.GetInstallationState(node.nodeId))
                        {
                            continue;
                        }

                        if (true == NodesCommon.Instance.IsPartInstallating())        //如果有零件正在安装
                        {
                            continue;
                        }

                        gameobj = Instantiate(node.gameObject, node.gameObject.transform, true);
                        gameobj.name = node.name;
                        gameobj.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                        if (null != gameobj.GetComponent<MeshFilter>())
                        {
                            gameobj.transform.localScale = node.LocalSize;
                        }

                        gameobj.AddComponent<NodeManager>();
                        gameobj.AddComponent<BoxCollider>();
                        if (null == gameobj.gameObject.GetComponent<MeshFilter>())
                        {
                            gameobj.GetComponent<BoxCollider>().size /= 10;
                        }
                        gameobj.GetComponent<BoxCollider>().size = GetBoxColliderSize(gameobj);
                        gameobj.AddComponent<HandDraggable>();
                        gameobj.GetComponent<HandDraggable>().RotationMode = HandDraggable.RotationModeEnum.LockObjectRotation;

                        StartCoroutine(OnMovesIEnumerator(gameobj, gameobj.transform.position));         //从零件架上飞出
                        gameobj.transform.RotateAround(AssembleManager.Instance.GetRotaAngleCenter(), Vector3.up, AssembleManager.Instance.GetRotaAngle());

                        #region Test 此处根据UI布局写活，后续需要根据UI调整
                        Txt = Instantiate(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text"), GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1").transform, true);
                        Txt.name = "Text" + node.nodeId;
                        Txt.transform.position = node.gameObject.transform.position;
                        Txt.GetComponent<Text>().text = node.gameObject.name;
                        #endregion

                        if (null != gameobj.GetComponent<Node>())
                        {
                            if (null != gameobj.GetComponent<HandDraggable>())
                            {
                                NodesCommon.Instance.SetInstallationState(node.GetComponent<Node>().nodeId, InstallationState.Step1Installed);
                                AssembleManager.Instance.AddInstalledNodeList(gameobj.GetComponent<Node>());
                                AssembleManager.Instance.SetInstalledNodeListStatus(gameobj.GetComponent<Node>(), InstallationState.Step1Installed);
                            }
                        }
                        break;
                    }
                }
            }
        }

        IEnumerator OnMovesIEnumerator(GameObject _GameObject, Vector3 statPos)
        {
            float f = 0;
            while (true)
            {
                if (f <= 1)
                {
                    _GameObject.transform.position = Vector3.Lerp(statPos, AssembleManager.Instance.GetPartStartPosition(), f);
                    f += Time.deltaTime;
                }
                else
                {
                    break;
                }
                yield return new WaitForSeconds(0);
            }
        }


        #region  按钮效果
        /// <summary>
        /// 让按钮看起来不能被点击，仅仅只是UI显示方面
        /// </summary>
        /// <param name="go"></param>
        public void DisButton(GameObject go)
        {
            Button but = go.GetComponent<Button>();
            but.interactable = false;
            go.transform.GetChild(2).gameObject.SetActive(false);
        }

        /// <summary>
        /// 让按钮看起来能被点击，仅仅只是UI显示方面
        /// </summary>
        /// <param name="go"></param>
        public void AbleButton(GameObject go)
        {
            Button but = go.GetComponent<Button>();
            but.interactable = true;
            go.transform.GetChild(2).gameObject.SetActive(true);
        }

        /// <summary>
        /// 让按钮看起来能被点击，仅仅只是UI显示方面
        /// </summary>
        public void AbleButtonPart(Node node)
        {
            int index = 0;
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                if (m_ItemsList[i].nodeId == node.nodeId)
                {
                    index = ((i + 1) % Page_Count) - 1;
                    break;
                }
            }

            for (int i = 0; i < BtnList.Count; i++)
            {
                if (i == index)
                {
                    BtnList[i].GetComponent<Button>().interactable = true;
                    BtnList[i].transform.GetChild(2).gameObject.SetActive(true);
                    break;
                }
            }
        }

        /// <summary>
        /// 让按钮看起来能被点击，仅仅只是UI显示方面
        /// </summary>
        public void DisButtonPart(Node node)
        {
            int index = 0;
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                if (m_ItemsList[i].nodeId == node.nodeId)
                {
                    index = ((i + 1) % Page_Count) - 1;
                    break;
                }
            }

            for (int i = 0; i < BtnList.Count; i++)
            {
                if (i == index)
                {
                    BtnList[i].GetComponent<Button>().interactable = false;
                    BtnList[i].transform.GetChild(2).gameObject.SetActive(false);
                    break;
                }
            }
        }
        #endregion

        public Vector3 GetBoxColliderSize(GameObject go)
        {
            Vector3 vector = go.GetComponent<Node>().GetPartModelRealSize(go);
            Vector3 vect = vector;

            if (vector.x < GlobalVar.ModelSize.x / 2.0)
            {
                vect.x = 3 * go.GetComponent<BoxCollider>().size.x;
            }

            if (vector.y < GlobalVar.ModelSize.y / 2.0)
            {
                vect.y = 3 * go.GetComponent<BoxCollider>().size.y;
            }

            if (vector.z < GlobalVar.ModelSize.z / 2.0)
            {
                vect.z = 3 * go.GetComponent<BoxCollider>().size.z;
            }
            return vect;
        }

    }
}