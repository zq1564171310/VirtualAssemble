/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 修改零件名字
/// </summary>
namespace WyzLink.Test
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NameHelper : MonoBehaviour
    {
        void Start()
        {
            List<ModelName> nameList = ReadData();
            GameObject gameObj;
            for (int i = 0; i < nameList.Count; i++)
            {
                gameObj = GameObject.Find("Models/LCD1/" + nameList[i].Name);
                if (null != gameObj && null != nameList[i].RealName && "" != nameList[i].RealName && "" != nameList[i].RealName.Trim())
                {
                    gameObj.name = nameList[i].RealName;
                }
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        public List<ModelName> ReadData()
        {
            List<ModelName> Name = new List<ModelName>();
            TextAsset textAsset = Resources.Load("Temp/Name") as TextAsset;
            if (null != textAsset && "" != textAsset.text)
            {
                string[] lines = textAsset.text.Split("\n"[0]);
                ModelName modelName;
                for (int i = 0; i < lines.Length; i++)
                {
                    if ("" == lines[i]) { break; }
                    modelName = new ModelName();
                    string[] lineInfo = lines[i].Split('\t');
                    modelName.Name = lineInfo[0];
                    modelName.RealName = lineInfo[1];
                    if (null == modelName.RealName)
                    {
                        modelName.RealName = "";
                    }
                    Name.Add(modelName);
                }
            }
            return Name;
        }

        public class ModelName
        {
            public string Name;
            public string RealName;
        }
    }
}
