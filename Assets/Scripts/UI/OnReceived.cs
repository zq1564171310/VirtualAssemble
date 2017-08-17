using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WyzLink.Control;
using WyzLink.Parts;
using WyzLink.Common;
using System;
using WyzLink.ToolsAndCommonParts;

public class OnReceived : MonoBehaviour
{
    public List<Node> Curpartlist = new List<Node>();//当前零件类型的集合

    public Dictionary<MyTransform, List<Node>> maps = new Dictionary<MyTransform, List<Node>>(); //声明字典maps

    public UIPartsClassPanel partsclass;// 声明UIPartsClassPanel类变量
    public Transform parent;//就是SinglePartPanel，即该脚本所挂物体的Transform

    public Button NextPage, PreviousPage;//分别表示下一页，上一页按钮
    public GameObject ViewPage;//显示当前类别下第几页，共几页

    private int CurClassNum = 1;//当前页码 （属零件类的翻页，左右按钮，每三个类别一翻页）
    private int UI_Btn_Num = 12;//零件架每页显示零件的个顺
    private int CurPartPage = 1;//当前页数（当前类别下的多个零件翻页，上一页，下一页按钮，每12个零件一翻页）

    private int lastpage = -1;//刷新时上一页

    private List<GameObject> lastlist = new List<GameObject>();//翻页时上一页存储的零件
    private List<Node> mynodes = new List<Node>();//所有零件的集合
    private List<string> mytypes = new List<string>();//所有零件类型的集合

    // Use this for initialization
    void Start()
    {
        NodesCommon common = NodesCommon.Instance;
        mynodes = common.GetNodeList();//初始化零件集合
        mytypes = common.GetNodeTypes();//初始化零件类型集合

        List<Tool> list = ToolsCommon.Instance.GetToolList();
        List<string> listType = ToolsCommon.Instance.GetToolTypes();

        List<CommonParts> lists = CommonPartsCommon.Instance.GetCommonPartsList();
        List<string> listTypes = CommonPartsCommon.Instance.GetCommonPartsTypes();


        //将零件类型和其所包含的零件集合分别作为字典的Key和Value添加到字典maps
        InitNodes();

        partsclass.Init(maps, OnCallBack);

        //添加监听
        NextPage.onClick.AddListener(NextPage_Btn);
        PreviousPage.onClick.AddListener(PreviousPage_Btn);
    }

    /// <summary>
    /// 零件下一页按钮
    /// </summary>
    void NextPage_Btn()
    {
        int PartTotalPage = Mathf.CeilToInt(Curpartlist.Count / (float)UI_Btn_Num);

        if (CurPartPage < PartTotalPage && CurClassNum > 0)
        {
            CurPartPage++;
            RefreshPage(CurPartPage);
        }
    }

    /// <summary>
    /// 零件上一页按钮
    /// </summary>
    void PreviousPage_Btn()
    {
        int PartTotalPage = Mathf.CeilToInt(Curpartlist.Count / (float)UI_Btn_Num);
        if (CurPartPage > 1 && CurPartPage <= PartTotalPage)
        {
            CurPartPage--;
            RefreshPage(CurPartPage);
        }
    }


    //刷新
    private void OnCallBack(int obj)
    {
        //重复点击返回
        if (lastpage == obj)
        {
            return;
        }
        lastpage = obj;

        Cleared(lastlist);

        // RefreshToogles(obj);

        List<MyTransform> mys = new List<MyTransform>(maps.Keys);

        MyTransform my = null;


        for (int i = 0; i < mys.Count; i++)
        {
            if (mys[i].page == obj - 1)
            {
                my = mys[i];
                Curpartlist = maps[my];// int Obj是页数

                //默认刷新第一页
                CurPartPage = 1;

                RefreshViewCurPage(CurPartPage);

                RefreshPage(CurPartPage);
            }
        }


    }


    /// <summary>
    /// 获取当前类型下零件的总页数
    /// </summary>
    /// <returns PartTotalPage总页数></returns>
    private int GetPartsCurClassTotalPage()
    {
        int count = Curpartlist.Count;
        int PartTotalPage = Mathf.CeilToInt(count / (float)UI_Btn_Num);
        return PartTotalPage;
    }

    /// <summary>
    /// 刷新显示当前共多少页，第几页
    /// </summary>
    /// <param name="CurFirstPage"></param>
    private void RefreshViewCurPage(int CurFirstPage)
    {
        ViewPage.GetComponent<Text>().text = "第" + CurFirstPage + "页/共" + GetPartsCurClassTotalPage() + "页";
    }


    void RefreshPage(int CurPartPage)
    {
        //刷新显示
        RefreshViewCurPage(CurPartPage);

        List<Node> temp = new List<Node>();
        int count = Curpartlist.Count;
        int PartTotalPage = Mathf.CeilToInt(count / (float)UI_Btn_Num);
        if (CurPartPage > 1)
        {
            Cleared(lastlist);
            //如果不是最后一页，遍历属于当前类型的的零件并记录在集合
            if (count >= (CurPartPage * UI_Btn_Num))
            {
                for (int i = UI_Btn_Num * (CurPartPage - 1); i < (CurPartPage) * UI_Btn_Num; i++)
                {
                    temp.Add(Curpartlist[i]);
                }
            }
            else
            {
                for (int i = UI_Btn_Num * (CurPartPage - 1); i < count; i++)
                {
                    temp.Add(Curpartlist[i]);
                }
            }
        }
        else
        {
            if (count / (float)UI_Btn_Num <= 1)
            {
                for (int i = 0; i < count; i++)
                {
                    temp.Add(Curpartlist[i]);
                }
            }
            else
            {
                for (int i = 0; i < UI_Btn_Num; i++)
                {
                    temp.Add(Curpartlist[i]);
                }

            }
        }

        for (int i = 0; i < temp.Count; i++)
        {
            Transform tran = parent.GetChild(i).GetChild(1);//找到SinglePartPanel的子物体Btn,再找子物体下的cube
            Node partOwnCurType = temp[i];//属于当前零件类型的第i个Node类物体
            string name = partOwnCurType.partName;//当前零件的名字
            string type = partOwnCurType.Type;//当前零件的类型的名字
            GameObject go = partOwnCurType.gameObject;//当前零件这个物体
            Text t = parent.GetChild(i).GetChild(0).GetComponent<Text>();//用来存储零件名字
            t.text = name;//将零件名字赋给t.text
            t.gameObject.SetActive(true);//让显示零件信息的Text类型游戏物体处于激活状态

            if (go.GetComponent<MeshRenderer>())//当前零件如果有MeshRenderer组件
            {
                go.GetComponent<MeshRenderer>().enabled = true;

            }
            go.transform.SetParent(tran);//设置零件位置等于cube的位置
            go.transform.localScale = Vector3.one * 0.2f;
            go.transform.localPosition = Vector3.zero;
            lastlist.Add(go);
        }
    }

    /// <summary>
    /// 翻页时隐藏前一页显示的零件
    /// </summary>
    /// <param name="temp"前一页存储的零件的集合></param>
    void Cleared(List<GameObject> temp)
    {
        //将Text全部隐藏
        for (int i = 0; i < parent.childCount; i++)
        {
            Text t = parent.GetChild(i).GetChild(0).GetComponent<Text>();
            t.gameObject.SetActive(false);
        }

        //隐藏上一页所有零件
        for (int i = 0; i < temp.Count; i++)
        {
            GameObject go = temp[i];
            if (go.GetComponent<MeshRenderer>())
            {
                go.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        temp.Clear();//清空该List集合，以便下次重新添加元素
    }

    /// <summary>
    /// 遍历零件所属的类型mystypes,得到当前类型str,此时遍历Node类型零件集合，
    /// 找到属于该类型的零件，添加到集合temp
    /// </summary>
    void InitNodes()
    {
        for (int i = 0; i < mytypes.Count; i++)
        {
            string str = mytypes[i];
            List<Node> temp = new List<Node>();
            for (int j = 0; j < mynodes.Count; j++)
            {
                Node partOwnCurType = mynodes[j];
                if (partOwnCurType.Type == str &&
                    (partOwnCurType.GetInstallationState() == InstallationState.NotInstalled
                    || partOwnCurType.GetInstallationState() == InstallationState.NextInstalling))
                {
                    temp.Add(partOwnCurType);
                }
            }
            MyTransform mt = new MyTransform(str, i);
            maps.Add(mt, temp);
        }

    }
}

/// <summary>
/// 零件的类型和页数
/// </summary>
public class MyTransform
{
    public MyTransform(string str, int page)
    {
        partclassname = str;
        this.page = page;
    }
    public string partclassname;
    public int page;
}

