using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WyzLink.Control;
using WyzLink.Parts;
using WyzLink.Common;
using System;

public class UIPartsClassPanel : MonoBehaviour
{
    private Action<int> call;
    private int CurClassNum = 0;

    private Dictionary<MyTransform, List<Node>> maps = new Dictionary<MyTransform, List<Node>>();

    private List<MyTransform> mystyps = new List<MyTransform>();

    public Button next, previous;

    /// <summary>
    /// UI显示类别名称
    /// </summary>
    /// <param name="map"></param>
    /// <param name="callback"></param>
    public void Init(Dictionary<MyTransform, List<Node>> map , Action<int> callback)
    {
        call = callback;
        maps = map;
        mystyps = new List<MyTransform>(map.Keys);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponentInChildren<Text>().text = mystyps[i].partclassname;
        }
    }

    void Start()
    {
        NodesCommon common = NodesCommon.Instance;

        CurClassNum = 1;
        next.onClick.AddListener(Nextbut);
        previous.onClick.AddListener(Previousbut);
    }

    //右方向
    void Nextbut()
    {
        if (CurClassNum < mystyps.Count && CurClassNum > 0)
        {
            
            CurClassNum++;
            Refresh(CurClassNum);
        }
        else
        {
            Debug.Log("currentpage 最大不能再加了" + CurClassNum);
        }
    }

    //左方向
    void Previousbut()
    {
        if (CurClassNum >1)
        {
            CurClassNum--;
            Refresh(CurClassNum);
        }
        else {
            Debug.Log("currentpage == 1不能再减了");
        }
    }


    //刷新
    void Refresh(int CurClassNum)
    {
        if (call != null)
        {
            call(CurClassNum);
        }
    }

}
