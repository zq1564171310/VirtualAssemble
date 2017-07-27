using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlaneManager : MonoBehaviour
{

    public int MenuNum;        //菜单选择项
    public int TypeNum;        //类型选择项
    public bool UIRefreshFlag; //UI刷新标记，一旦菜单选择项或者类型选择项发生改变，那么将标记改为True

    // Use this for initialization
    void Start()
    {
        StartCoroutine(OnPlayIEnumerator());
    }

    IEnumerator OnPlayIEnumerator()
    {
        while (true)
        {
            if (true == UIRefreshFlag)
            {
                if ((int)UIMenu.Part == MenuNum)
                {
                    GlobalVar._PartsTypePlane.SetActive(true);
                    GlobalVar._ToolsTypePlane.SetActive(false);
                    GlobalVar._PartsGameObjects.SetActive(true);
                    GlobalVar._ToolsGameObjects.SetActive(false);
                    GlobalVar._PartsMenuBtnText.color = Color.red;
                    GlobalVar._ToolsMenuBtnText.color = Color.yellow;
                }
                else if ((int)UIMenu.Tools == MenuNum)
                {
                    GlobalVar._PartsTypePlane.SetActive(false);
                    GlobalVar._ToolsTypePlane.SetActive(true);
                    GlobalVar._PartsGameObjects.SetActive(false);
                    GlobalVar._ToolsGameObjects.SetActive(true);
                    GlobalVar._PartsMenuBtnText.color = Color.yellow;
                    GlobalVar._ToolsMenuBtnText.color = Color.red;
                }
                else
                {
                    Debug.LogError("没有该类型枚举");
                }
                UIRefreshFlag = false;
            }
            yield return new WaitForSeconds(0);
        }
    }
}
