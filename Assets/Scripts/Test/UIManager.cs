using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int MenuNum;        //菜单选择项
    public int TypeNum;        //类型选择项
    public bool UIRefreshFlag; //UI刷新标记，一旦菜单选择项或者类型选择项发生改变，那么将标记改为True

    // Use this for initialization
    void Start()
    {
        StartCoroutine(OnPlayIEnumerator());
    }

    public void RefreshUI()
    {

    }

    IEnumerator OnPlayIEnumerator()
    {
        while (true)
        {
            if (true == UIRefreshFlag)
            {
                if ((int)UIMenu.Part == MenuNum)
                {
                    GlobalVar._PartsTypeUI.SetActive(true);
                    GlobalVar._ToolsTypeUI.SetActive(false);
                    GlobalVar._PartsTypeObject.SetActive(true);
                    GlobalVar._ToolsTypeObject.SetActive(false);
                    GlobalVar._PartsTypeText.GetComponent<TextMesh>().color = Color.red;
                    GlobalVar._ToolsTypeText.GetComponent<TextMesh>().color = Color.yellow;
                }
                else if ((int)UIMenu.Tools == MenuNum)
                {
                    GlobalVar._PartsTypeUI.SetActive(false);
                    GlobalVar._ToolsTypeUI.SetActive(true);
                    GlobalVar._PartsTypeObject.SetActive(false);
                    GlobalVar._ToolsTypeObject.SetActive(true);
                    GlobalVar._PartsTypeText.GetComponent<TextMesh>().color = Color.yellow;
                    GlobalVar._ToolsTypeText.GetComponent<TextMesh>().color = Color.red;
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
