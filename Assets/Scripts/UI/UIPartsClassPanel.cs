using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WyzLink.Parts;
using WyzLink.Common;
using System;

public class UIPartsClassPanel : MonoBehaviour
{
    [SerializeField]
    private Toggle PartClass1, PartClass2, PartClass3;

    private Action<int> call;
    private int CurClassNum = 1;//当前零件类型的页码

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
        UpdateListener(1);
    }

    void Start()
    {
        next.onClick.AddListener(Nextbut);
        previous.onClick.AddListener(Previousbut);
    }

    //右方向按钮
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

    //左方向按钮
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
            UpdateListener(CurClassNum);
        }
    }

    /// <summary>
    /// 关闭当前类页的无用类开关
    /// </summary>
    /// <param int name="CurClassNum"代表当前零件类型的页数></param>
    void UpdateListener(int CurClassNum)
    {
        //设置三个类别开关都处于激活状态
        PartClass1.gameObject.SetActive(true);
        PartClass2.gameObject.SetActive(true);
        PartClass3.gameObject.SetActive(true);
        //移除监听
        PartClass1.onValueChanged.RemoveAllListeners();
        PartClass2.onValueChanged.RemoveAllListeners();
        PartClass3.onValueChanged.RemoveAllListeners();

        //关闭无用Toggle
        if (CurClassNum % 3 == 1)
        {
            if (CurClassNum + 1 > mystyps.Count)
            {
                PartClass2.gameObject.SetActive(false);
                PartClass3.gameObject.SetActive(false);

                PartClass1.GetComponentInChildren<Text>().text = mystyps[CurClassNum - 1].partclassname;
            }
            else if (CurClassNum + 2 > mystyps.Count)
            {
                PartClass3.gameObject.SetActive(false);

                PartClass1.GetComponentInChildren<Text>().text = mystyps[CurClassNum - 1].partclassname;
                PartClass2.GetComponentInChildren<Text>().text = mystyps[CurClassNum].partclassname;
            }
            else
            {
                PartClass1.GetComponentInChildren<Text>().text = mystyps[CurClassNum - 1].partclassname;
                PartClass2.GetComponentInChildren<Text>().text = mystyps[CurClassNum].partclassname;
                PartClass3.GetComponentInChildren<Text>().text = mystyps[CurClassNum + 1].partclassname;
            }

            //更新监听
            PartClass1.onValueChanged.AddListener(
            (Ison) =>
            {
                if (call != null && Ison)
                {
                    call(CurClassNum);
                }
            }
            );
            PartClass2.onValueChanged.AddListener(
                (Ison) =>
                {
                    if (call != null && Ison)
                    {
                        call(CurClassNum + 1);
                    }
                }
                );
            PartClass3.onValueChanged.AddListener(
                (Ison) =>
                {
                    if (call != null && Ison)
                    {
                        call(CurClassNum + 2);
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
        #region
        //if (obj % 3 == 0)
        //{
        //    PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
        //    PartClass2.GetComponentInChildren<Text>().text = mystyps[obj].partclassname;
        //    PartClass3.GetComponentInChildren<Text>().text = mystyps[obj + 1].partclassname;
        //}
        //else if (obj % 3 == 1)
        //{
        //    //PartClass2.gameObject.SetActive(false);
        //    //PartClass3.gameObject.SetActive(false);
        //    //PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;

        //    PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
        //    PartClass2.GetComponentInChildren<Text>().text = mystyps[obj].partclassname;
        //    PartClass3.GetComponentInChildren<Text>().text = mystyps[obj + 1].partclassname;
        //}
        //else
        //{
        //    PartClass3.gameObject.SetActive(false);
        //    PartClass1.GetComponentInChildren<Text>().text = mystyps[obj - 1].partclassname;
        //    PartClass2.GetComponentInChildren<Text>().text = mystyps[obj].partclassname;
        //}
        #endregion
    }

}
