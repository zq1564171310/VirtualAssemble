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

    public class UIPartsPageStudyModel : MonoBehaviour, UIPartsPage
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

        private float ErrorTime = 0;       //错误提示时间
        private Text ErrorInfo;           //错误信息提示框内容
        private Text PartInfo;            //零件信息内容
        private Text NextParts;           //下一步该零件零件提示内容

        // Use this for initialization
        void Start()
        {
            ErrorInfo = GameObject.Find("Canvas/BG/InfoPanel/Panel/Error").GetComponent<Text>();           //错误信息提示框内容
            PartInfo = GameObject.Find("Canvas/BG/InfoPanel/Panel/Info").GetComponent<Text>();            //零件信息内容
            NextParts = GameObject.Find("Canvas/BG/InfoPanel/Panel/Tips").GetComponent<Text>();           //下一步该零件零件提示内容
        }

        void Update()
        {
            if (InitFlag <= 2)   //第二帧布局的坐标才正常，之前btn坐标都是一样的，自动布局的问题，后续想办法修改该问题
            {
                RefreshItems();
                InitFlag++;
            }

            if ("错误信息提示：" != ErrorInfo.text)
            {
                ErrorTime += Time.deltaTime;
            }
            if (ErrorTime > 3)
            {
                ErrorInfo.text = "错误信息提示：";
                ErrorTime = 0;
            }
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

            if (null != NodesCommonStudyModel.Instance)
            {
                NodesList = NodesCommonStudyModel.Instance.GetNodesList();
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

            for (int i = 0; i < NodesList.Count; i++)
            {
                if (InstallationState.Installed == NodesList[i].GetInstallationState() && NodesList[i].name != FirstNode)
                {
                    GameObject gameobj;                         //克隆一份,作为安装的零件
                    gameobj = Instantiate(NodesList[i].gameObject, NodesList[i].gameObject.transform, true);
                    gameobj.name = NodesList[i].name;
                    gameobj.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                    gameobj.transform.localScale = NodesList[i].LocalSize * AssembleManagerStudyModel.Instance.GetScale();
                    AssembleManagerStudyModel.Instance.AddInstalledNodeList(gameobj.GetComponent<Node>());
                    AssembleManagerStudyModel.Instance.SetInstalledNodeListStatus(NodesList[i], InstallationState.Installed);
                }
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
            RefreshItems();
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
        public void BindPage(int index)
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
        public void BindGridItem(Transform trans, Node gridItem)
        {
            trans.Find("Text").GetComponent<Text>().text = gridItem.partName;
            gridItem.gameObject.transform.position = trans.GetChild(1).transform.position;
            trans.GetComponent<UIPartBtnStateManagerStudyModel>().RefreshData(gridItem);
        }

        /// <summary>
        /// 类型变化，刷新零件界面
        /// </summary>
        /// <param name="type"></param>
        public void RefreshItems()
        {
            if (null != GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel"))
            {
                m_Type = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel").GetComponent<UIPartsPanelClassStudyModel>().GetPartsType();
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

            if (InitFlag < 2)
            {
                m_PageIndex = 1;  //每次刷新都必须把页面重置，防止新的页面页数不够
            }
            else
            {
                if (m_PageIndex > m_PageCount)
                {
                    m_PageIndex = 1;  //每次刷新都必须把页面重置，防止新的页面页数不够
                }
            }

            BindPage(m_PageIndex);

            m_PanelText.text = string.Format("第" + "{0}/{1}" + "页", m_PageIndex.ToString(), m_PageCount.ToString());
        }

        /// <summary>
        /// 零件按钮点击事件
        /// </summary>
        public void BtnClick(GameObject go)
        {
            Node node;                                  //被点击的零件
            IEnumerable<Node> NextInstallNode = AssembleManagerStudyModel.Instance.GetNextInstallNode();
            GameObject gameobj;                         //克隆一份,作为安装的零件
            GameObject gameOb;                          //克隆一份作为提示
            //GameObject gameObSec;                       //克隆一份作为第二工作区提示
            GameObject Txt;                             //克隆一份文本
            Transform[] trans;

            if (EntryMode.GetAssembleModel() != AssembleModel.ExamModel)         //学习模式
            {
                for (int i = 0; i < BtnList.Count; i++)
                {
                    if (BtnList[i].name == go.name)
                    {
                        node = m_ItemsList[(m_PageIndex - 1) * Page_Count + i];

                        if (InstallationState.Step1Installed == NodesCommonStudyModel.Instance.GetInstallationState(node.nodeId) || InstallationState.Installed == NodesCommonStudyModel.Instance.GetInstallationState(node.nodeId))
                        {
                            continue;
                        }

                        if (true == NodesCommonStudyModel.Instance.IsPartInstallating())        //如果有零件正在安装
                        {
                            //此处要给提示
                            ErrorInfo.text = "错误信息提示：请装完前面一个从零件架上取下的零件，再从零件架上抓取！";
                            continue;
                        }

                        if (null != NextInstallNode)
                        {
                            foreach (Node nd in NextInstallNode)
                            {
                                if (nd.nodeId == node.nodeId && InstallationState.NextInstalling == node.GetInstallationState())
                                {
                                    gameobj = Instantiate(node.gameObject, node.gameObject.transform, true);
                                    gameobj.gameObject.transform.rotation = node.TargetRotation;
                                    gameobj.name = node.name;
                                    gameobj.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                                    gameobj.transform.localScale = node.LocalSize * AssembleManagerStudyModel.Instance.GetScale();
                                    //gameobj.transform.rotation = node.LocalRotation;
                                    gameobj.transform.RotateAround(AssembleManagerStudyModel.Instance.GetRotaAngleCenter(), Vector3.up, AssembleManagerStudyModel.Instance.GetRotaAngle());

                                    gameOb = Instantiate(node.gameObject, node.gameObject.transform, true);
                                    gameOb.name = node.name + node.nodeId;

                                    gameOb.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                                    gameOb.transform.position = node.EndPos;
                                    gameOb.transform.rotation = gameobj.transform.rotation;
                                    if (null != gameOb.GetComponent<MeshRenderer>())
                                    {
                                        gameOb.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;

                                        trans = gameOb.GetComponentsInChildren<Transform>();
                                        if (null != trans)
                                        {
                                            foreach (Transform tran in trans)
                                            {
                                                if (null != tran.gameObject.GetComponent<MeshRenderer>())
                                                {
                                                    tran.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;
                                                }
                                            }
                                        }
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
                                    gameOb.transform.localScale = node.LocalSize * AssembleManagerStudyModel.Instance.GetScale();

                                    gameobj.AddComponent<NodeManagerStudyModel>();
                                    gameobj.AddComponent<BoxCollider>();
                                    if (null == gameobj.GetComponent<MeshFilter>())
                                    {
                                        gameobj.GetComponent<BoxCollider>().size /= 10;
                                    }
                                    //gameobj.GetComponent<BoxCollider>().size = GetBoxColliderSize(gameobj);
                                    gameobj.AddComponent<HandDraggable>();
                                    gameobj.GetComponent<HandDraggable>().RotationMode = HandDraggable.RotationModeEnum.LockObjectRotation;

                                    StartCoroutine(OnMovesIEnumerator(gameobj, gameobj.transform.position));
                                    PartInfo.text = "被选中的零件信息：" + gameobj.GetComponent<Node>().note.Replace("&", "\n").ToString();
                                    if (null != AssembleManagerStudyModel.Instance.GetNextInstallNode())
                                    {
                                        string tips = "";
                                        int index = 1;
                                        foreach (Node no in AssembleManagerStudyModel.Instance.GetNextInstallNode())
                                        {
                                            if (InstallationState.NextInstalling == no.GetInstallationState())
                                            {
                                                index = GetIndex(no);
                                                tips += no.name + "(第" + index + "页）" + "/";
                                            }
                                        }
                                        NextParts.text = "下一步应该安装的零件:" + tips;
                                    }

                                    Txt = Instantiate(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text"), GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1").transform, true);
                                    Txt.name = "Text" + node.nodeId;
                                    Txt.transform.position = node.gameObject.transform.position;
                                    Txt.GetComponent<Text>().text = node.gameObject.name;

                                    if (null != gameobj.GetComponent<Node>())
                                    {
                                        if (null != gameobj.GetComponent<HandDraggable>())
                                        {
                                            NodesCommonStudyModel.Instance.SetInstallationState(node.GetComponent<Node>().nodeId, InstallationState.Step1Installed);
                                            AssembleManagerStudyModel.Instance.AddInstalledNodeList(gameobj.GetComponent<Node>());
                                            AssembleManagerStudyModel.Instance.SetInstalledNodeListStatus(gameobj.GetComponent<Node>(), InstallationState.Step1Installed);

                                            AssembleManagerStudyModel.Instance.AutoScaleAndRota(gameobj.GetComponent<Node>());
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

                        if (InstallationState.Step1Installed == NodesCommonStudyModel.Instance.GetInstallationState(node.nodeId) || InstallationState.Installed == NodesCommonStudyModel.Instance.GetInstallationState(node.nodeId))
                        {
                            continue;
                        }

                        if (true == NodesCommonStudyModel.Instance.IsPartInstallating())        //如果有零件正在安装
                        {
                            continue;
                        }

                        gameobj = Instantiate(node.gameObject, node.gameObject.transform, true);
                        gameobj.name = node.name;
                        gameobj.transform.parent = GameObject.Find("RuntimeObject/Nodes").transform;
                        gameobj.transform.localScale = node.LocalSize * AssembleManagerStudyModel.Instance.GetScale();
                        gameobj.transform.RotateAround(AssembleManagerStudyModel.Instance.GetRotaAngleCenter(), Vector3.up, AssembleManagerStudyModel.Instance.GetRotaAngle());

                        gameobj.AddComponent<NodeManagerStudyModel>();
                        gameobj.AddComponent<BoxCollider>();
                        if (null == gameobj.gameObject.GetComponent<MeshFilter>())
                        {
                            gameobj.GetComponent<BoxCollider>().size /= 10;
                        }
                        //gameobj.GetComponent<BoxCollider>().size = GetBoxColliderSize(gameobj);
                        gameobj.AddComponent<HandDraggable>();
                        gameobj.GetComponent<HandDraggable>().RotationMode = HandDraggable.RotationModeEnum.LockObjectRotation;

                        StartCoroutine(OnMovesIEnumerator(gameobj, gameobj.transform.position));         //从零件架上飞出

                        Txt = Instantiate(GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1/Text"), GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel/Button 1").transform, true);
                        Txt.name = "Text" + node.nodeId;
                        Txt.transform.position = node.gameObject.transform.position;
                        Txt.GetComponent<Text>().text = node.gameObject.name;

                        if (null != gameobj.GetComponent<Node>())
                        {
                            if (null != gameobj.GetComponent<HandDraggable>())
                            {
                                NodesCommonStudyModel.Instance.SetInstallationState(node.GetComponent<Node>().nodeId, InstallationState.Step1Installed);
                                AssembleManagerStudyModel.Instance.AddInstalledNodeList(gameobj.GetComponent<Node>());
                                AssembleManagerStudyModel.Instance.SetInstalledNodeListStatus(gameobj.GetComponent<Node>(), InstallationState.Step1Installed);
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
                    _GameObject.transform.position = Vector3.Lerp(statPos, AssembleManagerStudyModel.Instance.GetPartStartPosition(), f);
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
            if (false == go.activeInHierarchy)             //有可能按钮是被隐藏的
            {
                return;
            }
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
            if (false == go.activeInHierarchy)           //有可能按钮是被隐藏的
            {
                return;
            }
            Button but = go.GetComponent<Button>();
            but.interactable = true;
            go.transform.GetChild(2).gameObject.SetActive(true);
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