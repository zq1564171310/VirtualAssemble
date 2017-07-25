using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllChildPos : MonoBehaviour 
{
    #region 字段
    private int childcount = 0;
    #endregion



    #region Unity 回调
    void Start()
    {
        //Debug.Log(GetComponent<MeshRenderer>().bounds.size.x);
        //Debug.Log(GetComponent<MeshRenderer>().bounds.size.y);
    }


    void Update()
    {
        if (transform.childCount != childcount)
        {
            childcount = transform.childCount;
            for (int i = 0; i < childcount; i++)
            {
                if (i % 4 <= 1)
                {

                }
            }
        }
    }
    #endregion

}
