using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WyzLink.Control;
using WyzLink.Parts;
using WyzLink.Common;
using System;

public class UIPartsClassPanel : MonoBehaviour
{
    [SerializeField]
    private Toggle PartClass1, PartClass2, PartClass3;

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
    public void Init(Dictionary<MyTransform, List<Node>> map, Action<int> callback)
    {

        call = callback;
        maps = map;
        mystyps = new List<MyTransform>(map.Keys);

        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).GetComponentInChildren<Text>().text = mystyps[i].partclassname;
        //}

        UpdateListener(1);

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
        int pagestotal = Mathf.CeilToInt(mystyps.Count / 3f);
        if (CurClassNum < pagestotal && CurClassNum > 0)
        {
            CurClassNum++;
            int current = ((CurClassNum - 1) * 3) + 1;
            if (current <= mystyps.Count)
            {
                Refresh(current);
            }
        }
    }

    //左方向
    void Previousbut()
    {
        int pagestotal = Mathf.CeilToInt(mystyps.Count / 3f);

        if (CurClassNum > 1)
        {
            CurClassNum--;
            int current = ((CurClassNum - 1) * 3) + 1;
            if (current <= mystyps.Count)
            {
                Refresh(current);
            }
        }

    }


    //刷新
    void Refresh(int CurClassNum)
    {
        if (call != null)
        {
            //刷1，4，7，11
            //call(CurClassNum);


            UpdateListener(CurClassNum);
        }
    }


    void UpdateListener(int obj)
    {
        PartClass1.gameObject.SetActive(true);
        PartClass2.gameObject.SetActive(true);
        PartClass3.gameObject.SetActive(true);
        //移除监听
        PartClass1.onValueChanged.RemoveAllListeners();
        PartClass2.onValueChanged.RemoveAllListeners();
        PartClass3.onValueChanged.RemoveAllListeners();

        #region  错误逻辑，暂时先定3个类别，有时间修改该bug
        //关闭无用Toggle
        //if (obj + 2 > mystyps.Count)
        //{
        //    PartClass2.gameObject.SetActive(false);
        //    PartClass3.gameObject.SetActive(false);

        //    PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
        //}
        //else if (obj + 3 > mystyps.Count)
        //{
        //    PartClass3.gameObject.SetActive(false);

        //    PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
        //    PartClass2.GetComponentInChildren<Text>().text = mystyps[obj].partclassname;
        //}
        //else
        //{
        //    PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
        //    PartClass2.GetComponentInChildren<Text>().text = mystyps[obj].partclassname;
        //    PartClass3.GetComponentInChildren<Text>().text = mystyps[obj + 1].partclassname;
        //}

        if (obj % 3 == 0)
        {
            PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
            PartClass2.GetComponentInChildren<Text>().text = mystyps[obj].partclassname;
            PartClass3.GetComponentInChildren<Text>().text = mystyps[obj + 1].partclassname;
        }
        else if (obj % 3 == 1)
        {
            //PartClass2.gameObject.SetActive(false);
            //PartClass3.gameObject.SetActive(false);
            //PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;

            PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
            PartClass2.GetComponentInChildren<Text>().text = mystyps[obj].partclassname;
            PartClass3.GetComponentInChildren<Text>().text = mystyps[obj + 1].partclassname;
        }
        else
        {
            PartClass3.gameObject.SetActive(false);
            PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
            PartClass2.GetComponentInChildren<Text>().text = mystyps[obj].partclassname;
        }
        #endregion


        //更新监听
        PartClass1.onValueChanged.AddListener(
            (Ison) =>
            {
                if (call != null && Ison)
                {
                    call(obj);
                }
            }
            );
        PartClass2.onValueChanged.AddListener(
            (Ison) =>
            {
                if (call != null && Ison)
                {
                    call(obj + 1);
                }
            }
            );
        PartClass3.onValueChanged.AddListener(
            (Ison) =>
            {
                if (call != null && Ison)
                {
                    call(obj + 2);
                }
            }
            );



        //刷新默认Toggle的IsOn状态
        if (PartClass1.isOn)
        {
            PartClass1.onValueChanged.Invoke(true);
        }
        else
        {
            PartClass1.isOn = true;
        }
        PartClass2.isOn = false;
        PartClass3.isOn = false;
    }

}
