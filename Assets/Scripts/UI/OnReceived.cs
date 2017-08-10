using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WyzLink.Control;
using WyzLink.Parts;
using WyzLink.Common;
using System;

public class OnReceived : MonoBehaviour
{
    List<Node> Curpartlist = new List<Node>();

    private int CurClassNum = 0;
    private int UI_Btn_Num = 12;
    private int CurPartPage = 1;

    private List<GameObject> lastlist = new List<GameObject>();
    private List<Node> mynodes = new List<Node>();
    private List<string> mytypes = new List<string>();

    //声明字典maps
    public Dictionary<MyTransform, List<Node>> maps = new Dictionary<MyTransform, List<Node>>();

    public UIPartsClassPanel partsclass;
    public Transform parent;

    public Button NextPage, PreviousPage;

    // Use this for initialization
    void Start()
    {
        NodesCommon common = NodesCommon.Instance;
        mynodes = common.GetNodeList();
        mytypes = common.GetNodeTypes();
        
        CurClassNum = 1;

        //将零件类型和其所包含的零件集合分别作为字典的Key和Value添加到字典maps
        InitNodes();

        partsclass.Init(maps, OnCallBack);
        OnCallBack(1);

        //添加监听
        NextPage.onClick.AddListener(NextPage_Btn);
        PreviousPage.onClick.AddListener(PreviousPage_Btn);
    }


    void NextPage_Btn()
    {
        int PartTotalPage = Mathf.CeilToInt(Curpartlist.Count / (float)UI_Btn_Num);
        if (CurPartPage < PartTotalPage && CurClassNum > 0)
        {
            CurPartPage++;
            RefreshPage(CurPartPage);
        }
        else
        {
            Debug.Log("CurPartPage 最大不能再加了" + CurPartPage);
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
        else
        {
            Debug.Log("CurPartPage 最小不能再减了" + CurPartPage);
        }
    }


    //刷新
    private void OnCallBack(int obj)
    {
        Cleared(lastlist);

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
                RefreshPage(CurPartPage);

            }
        }
    }

    void RefreshPage(int CurPartPage)
    {
        List<Node> temp = new List<Node>();
        int count = Curpartlist.Count;
        int PartTotalPage = Mathf.CeilToInt(count / (float)UI_Btn_Num);
        if (CurPartPage >= 1)
        {
            Cleared(lastlist);
            //如果不是最后一页，遍历属于当前类型的的零件并记录在集合
            if (count >= (CurPartPage  * UI_Btn_Num))
            {
                for (int i = UI_Btn_Num * (CurPartPage - 1); i < (CurPartPage)* UI_Btn_Num; i++)
                {
                    temp.Add(Curpartlist[i]);
                }
            }
            else
            {
                for (int i = CurPartPage * (UI_Btn_Num-1); i < count; i++)
                {
                    temp.Add(Curpartlist[i]);
                }
            }
        }
        else
        {
            if (count / UI_Btn_Num <= 1)
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
            if (go.GetComponent<MeshRenderer>())
            {
                go.GetComponent<MeshRenderer>().enabled = true;

            }

            go.transform.SetParent(tran);//设置零件位置等于cube的位置
            go.transform.localScale = Vector3.one* 0.2f;//GlobalVar.ModelSize;
            go.transform.localPosition = Vector3.zero;
            lastlist.Add(go);
        }
    }


    void Cleared(List<GameObject> temp)
    {
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
            MyTransform mt = new MyTransform(str,i);

            maps.Add(mt, temp);
        }

    }
}


public class MyTransform
{
    public MyTransform(string str,int page)
    {
        partclassname = str;
        this.page = page;
    }
    public string partclassname;
    public int page;
}

