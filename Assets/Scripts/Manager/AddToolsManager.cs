/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加工具集合
/// </summary>
namespace WyzLink.Manager
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AddToolsManager : MonoBehaviour
    {
        [HideInInspector]
        public GameObject[] Tools;    //Resoures目录下的工具预制体

        [HideInInspector]
        public static List<GameObject> ToolsList = new List<GameObject>();

        [HideInInspector]
        public static List<string> ToolsType = new List<string>();
        // Use this for initialization
        void Start()
        {
            Tools = Resources.LoadAll<GameObject>("ToolsPrefabs");

            if (Tools.Length > 0)
            {
                for (int i = 0; i < Tools.Length; i++)
                {
                    ToolsList.Add(Instantiate(Tools[i]));
                    if (ToolsList[i].name.Contains("一字螺丝刀"))
                    {
                        if (!ToolsType.Contains("一字螺丝刀"))
                        {
                            ToolsType.Add("一字螺丝刀");
                        }
                    }
                    else if (ToolsList[i].name.Contains("十字螺丝刀"))
                    {
                        if (!ToolsType.Contains("十字螺丝刀"))
                        {
                            ToolsType.Add("十字螺丝刀");
                        }
                    }
                    else if (ToolsList[i].name.Contains("内六角"))
                    {
                        if (!ToolsType.Contains("内六角"))
                        {
                            ToolsType.Add("内六角");
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}