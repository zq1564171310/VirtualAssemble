using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToolsManager : MonoBehaviour
{
    private GameObject[] Tools;    //Resoures目录下的工具预制体

    private List<GameObject> ToolsList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        Tools = Resources.LoadAll<GameObject>("ToolsPrefabs");

        if (Tools.Length > 0)
        {
            for (int i = 0; i < Tools.Length; i++)
            {
                ToolsList.Add(Instantiate(Tools[i]));
                ToolsList[i].transform.parent = GlobalVar._ToolsGameObjects.transform;

                //Debug.Log(GlobalVar._GetModelSize.GetToolModelRealSize(ToolsList[i]).ToString("f4"));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
