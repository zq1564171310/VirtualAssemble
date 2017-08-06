using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PartPageTurning : MonoBehaviour {

    private int CurrentPage = 1;
    private int PartTotalPage= 0;
    private GameObject ViewPage;

    //private int PartsCount = 0;

    private GameObject PreviousPage_Btn;
    private GameObject NextPage_Btn;
    // Use this for initialization
    void Start ()
    {
        ViewPage = GameObject.Find("Canvas/PanelPart/Text");
        PreviousPage_Btn = GameObject.Find("Canvas/PanelPart/PreviousPage_Btn");
        NextPage_Btn = GameObject.Find("Canvas/PanelPart/PreviousPage_Btn");
        PartTotalPage = OnReceivedList.Instance.PartsCount1;

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void PartsUI()
    {

    }

    public void Next()
    {
        if (PartTotalPage <= 0)
        {
            return;
        }

        if (CurrentPage >= PartTotalPage)
        {
            return;
        }

        CurrentPage += 1;

        if (CurrentPage >= PartTotalPage)
        {
            CurrentPage = PartTotalPage;
        }
            
        BindPage(CurrentPage);

        //更新界面页数
        ViewPage.GetComponent<Text>().text = string.Format("第" + "{0}"+"页"+"/ "+"共" +"{1}" + "页", ViewPage.ToString(), PartTotalPage.ToString());
    }

    public void Previous()
    {
        if (CurrentPage <= 0)
            return;
        //第一页时禁止向前翻页
        if (CurrentPage <= 1)
            return;
        CurrentPage -= 1;
        if (CurrentPage < 1)
            CurrentPage = 1;

        BindPage(CurrentPage);

        //更新界面页数
        ViewPage.GetComponent<Text>().text = string.Format("第" + "{0}" + "页" + "/ " + "共" + "{1}" + "页", ViewPage.ToString(), PartTotalPage.ToString());
    }


    private void BindPage(int index)
    {
        if (OnReceivedList.PartsList1 == null || OnReceivedList.PartsList1.Count<= 0)
        {
            return;
        }
        //索引处理
        if (index < 0 || index > OnReceivedList.PartsList1.Count)
        {
            return;
        }

        for (int i = 0; i < OnReceivedList.PartsList1.Count; i++)
        {
            OnReceivedList.PartsImage1[i].gameObject.SetActive(false);
        }
        List<GameObject> CurList = OnReceivedList.SetChild(PartTotalPage);
    }
}
