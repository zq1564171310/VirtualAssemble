using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WyzLink.Control;
using WyzLink.Parts;
using WyzLink.Common;
using System;

public class OnReceived : MonoBehaviour
{
    public List<Node> Curpartlist = new List<Node>();

    //声明字典maps
    public Dictionary<MyTransform, List<Node>> maps = new Dictionary<MyTransform, List<Node>>();

    public UIPartsClassPanel partsclass;
    public Transform parent;//就是SinglePartPanel，即该脚本所挂物体

    public Button NextPage, PreviousPage;//分别表示下一页，上一页按钮
    public GameObject ViewPage;//显示当前类别下第几页，共几页

    private int CurClassNum = 1;//当前页码 （属零件类的翻页，左右按钮，每三个类别一翻页）
    private int UI_Btn_Num = 12;//零件架每页显示零件的个顺
    private int CurPartPage = 1;//当前页数（当前类别下的多个零件翻页，上一页，下一页按钮，每12个零件一翻页）


    //上一刷新的page
    private int lastpage = -1;

    private List<GameObject> lastlist = new List<GameObject>();
    private List<Node> mynodes = new List<Node>();
    private List<string> mytypes = new List<string>();

    // Use this for initialization
    void Start()
    {
        NodesCommon common = NodesCommon.Instance;
        mynodes = common.GetNodeList();
        mytypes = common.GetNodeTypes();

        //CurClassNum = 1;

        //将零件类型和其所包含的零件集合分别作为字典的Key和Value添加到字典maps
        InitNodes();

        partsclass.Init(maps, OnCallBack);
        for (int i = 0; i < 3; i++)
        {
            OnCallBack(i);
            int PartTotalPage = Mathf.CeilToInt(Curpartlist.Count / (float)UI_Btn_Num);
            if (CurPartPage < PartTotalPage && CurClassNum > 0)
            {
                CurPartPage++;
                RefreshPage(CurPartPage);
            }
        }

        //添加监听
        NextPage.onClick.AddListener(NextPage_Btn);
        PreviousPage.onClick.AddListener(PreviousPage_Btn);
    }


    void NextPage_Btn()
    {
        int PartTotalPage = Mathf.CeilToInt(Curpartlist.Count / (float)UI_Btn_Num);
        //if (PartTotalPage <= 0)
        //{
        //    return;
        //}

        //if (CurPartPage >= PartTotalPage)
        //{
        //    return;
        //}

        //CurPartPage += 1;

        //if (CurPartPage >= PartTotalPage)
        //{
        //    CurPartPage = PartTotalPage;
        //}
        if (CurPartPage < PartTotalPage && CurClassNum > 0)
        {
            CurPartPage++;
            RefreshPage(CurPartPage);
        }
    }

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

                RefreshIndex(CurPartPage);

                RefreshPage(CurPartPage);
            }
        }


    }


    //获取当前总页数（小）
    private int GetCurrentIndex()
    {
        int count = Curpartlist.Count;
        int PartTotalPage = Mathf.CeilToInt(count / (float)UI_Btn_Num);
        return PartTotalPage;
    }

    //刷新按钮
    private void RefreshIndex(int first)
    {
        ViewPage.GetComponent<Text>().text = "第" + first + "页/共" + GetCurrentIndex() + "页";
    }


    void RefreshPage(int CurPartPage)
    {
        //刷新显示
        RefreshIndex(CurPartPage);

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
            string name = partOwnCurType.partName;
            string type = partOwnCurType.Type;
            GameObject go = partOwnCurType.gameObject;
            Text t = parent.GetChild(i).GetChild(0).GetComponent<Text>();//用来存储零件名字
            t.gameObject.SetActive(true);
            t.text = name + "  " + type;
            t.text = name;

            if (go.GetComponent<MeshRenderer>())
            {
                go.GetComponent<MeshRenderer>().enabled = true;

            }
            go.transform.SetParent(tran);//设置零件位置等于cube的位置
            go.transform.localScale = Vector3.one * 0.2f;
            go.transform.localPosition = Vector3.zero;
            lastlist.Add(go);
        }
    }


    void Cleared(List<GameObject> temp)
    {
        //将Text全部隐藏
        for (int i = 0; i < parent.childCount; i++)
        {
            Text t = parent.GetChild(i).GetChild(0).GetComponent<Text>();
            t.gameObject.SetActive(false);
        }


        for (int i = 0; i < temp.Count; i++)
        {
            GameObject go = temp[i];
            if (go.GetComponent<MeshRenderer>())
            {
                go.GetComponent<MeshRenderer>().enabled = false;
                //GameObject.Destroy(go);
            }
        }
        temp.Clear();
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
                if (partOwnCurType.Type == str)
                {
                    temp.Add(partOwnCurType);
                }
            }
            MyTransform mt = new MyTransform(str, i);

            maps.Add(mt, temp);
        }

    }
}


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

