using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Use this for initialization  
    void Start()
    {
        EventTriggerListener.Get(GlobalVar._PartsMenuBtn.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(GlobalVar._ToolsMenuBtn.gameObject).onClick = OnButtonClick;
    }

    private void OnButtonClick(GameObject gameObj)
    {
        //在这里监听按钮的点击事件  
        if (gameObj == GlobalVar._PartsMenuBtn.gameObject)
        {
            //点击了零件按钮
            GlobalVar._UIManagerPlaneScript.MenuNum = 0;
            GlobalVar._UIManagerPlaneScript.UIRefreshFlag = true;
        }
        else if (gameObj == GlobalVar._ToolsMenuBtn.gameObject)
        {
            //点击了工具按钮
            GlobalVar._UIManagerPlaneScript.MenuNum = 1;
            GlobalVar._UIManagerPlaneScript.UIRefreshFlag = true;
        }
    }
}
