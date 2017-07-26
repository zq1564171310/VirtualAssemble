using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assembles : MonoBehaviour
{
    public bool IsAssembleFlag;    //是否正在安装

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //IEnumerator OnPlayIEnumerator()
    //{
    //    while (true)
    //    {
    //        if (true == IsAssembleFlag)
    //        {
    //            GlobalVar._ErrorMassage.SetActive(true);
    //        }
    //        else
    //        {
    //            GlobalVar._ErrorMassage.SetActive(false);
    //        }
    //    }
    //}


    void OnTriggerEnter(Collider collider)
    {
        if ("NONE 18" == collider.name)
        {
            IsAssembleFlag = false;
            Destroy(collider.gameObject.GetComponent<HandDraggable>());
            GameObject.Find("NONE 17").AddComponent<HandDraggable>();
        }
        else if ("NONE 17" == collider.name)
        {
            IsAssembleFlag = false;
            Destroy(collider.gameObject.GetComponent<HandDraggable>());
            GameObject.Find("NONE 21").AddComponent<HandDraggable>();
        }
    }
}
