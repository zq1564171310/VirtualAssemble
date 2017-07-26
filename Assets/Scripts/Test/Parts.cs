using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    public Vector3 StartPos;
    public Vector4 EndPos;

    // Use this for initialization
    void Start()
    {
        StartPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (null != gameObject.GetComponent<HandDraggable>() && gameObject.transform.position != StartPos)
        {
            GlobalVar._Assembles.IsAssembleFlag = true;
        }
    }
}
