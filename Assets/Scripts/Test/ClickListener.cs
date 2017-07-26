using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickListener : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool IsMouseDwonFlag;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (GlobalVar._PartsTypeBtn == gameObject)
        {
            GlobalVar._UIManagerScript.MenuNum = 0;
            GlobalVar._UIManagerScript.UIRefreshFlag = true;
        }
        else
        {
            GlobalVar._UIManagerScript.MenuNum = 1;
            GlobalVar._UIManagerScript.UIRefreshFlag = true;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (true == GlobalVar._Assembles.IsAssembleFlag)
        {
            GlobalVar._ErrorMassage.SetActive(true);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GlobalVar._ErrorMassage.SetActive(false);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        IsMouseDwonFlag = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        IsMouseDwonFlag = false;
    }
}
