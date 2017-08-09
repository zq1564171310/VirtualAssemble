/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 文件数据读取
/// </summary>
namespace WyzLink.Cache
{
    using WyzLink.Parts;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class DataHelper
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        public static List<Node> ReadData()
        {
            List<Node> modelList = new List<Node>();
            //ModelClass models;
            //TextAsset textAsset = Resources.Load("Data") as TextAsset;
            //if (null != textAsset && "" != textAsset.text)
            //{
            //    string[] lines = textAsset.text.Split("\n"[0]);
            //    for (int i = 0; i < lines.Length; i++)
            //    {
            //        if ("" == lines[i]) { break; }
            //        models = new ModelClass();
            //        string[] lineInfo = lines[i].Split('\t');
            //        models.Name = lineInfo[0];
            //        models.PlayOrderId = Convert.ToInt32(lineInfo[1]);
            //        models.MoveModel = Convert.ToInt32(lineInfo[2]);
            //        models.NameDisplay = Convert.ToInt32(lineInfo[3]);
            //        models.GroupNum = Convert.ToInt32(lineInfo[4]);
            //        models.EndPos = StringToVector(lineInfo[5]);
            //        models.StartPos = StringToVector(lineInfo[6]);
            //        if (0 == Convert.ToInt32(lineInfo[7]))
            //        {
            //            models.IsMove = false;
            //        }
            //        else
            //        {
            //            models.IsMove = true;
            //        }
            //        models.MechanicalSoundID = Convert.ToInt32(lineInfo[8]);
            //        models.IntroductionSpeechID = Convert.ToInt32(lineInfo[9]);
            //        modelList.Add(models);
            //    }
            //}
            return modelList;
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="engines"></param>
        public static void SaveData(List<Node> engines)
        {
            //if (engines.Count <= 0)
            //{
            //    return;
            //}

            //int isMove = 0;
            //string str = null;
            //for (int i = 0; i < engines.Count; i++)
            //{
            //    if (false == engines[i].IsMove)
            //    {
            //        isMove = 0;
            //    }
            //    else
            //    {
            //        isMove = 1;
            //    }
            //    str += engines[i].Name + "\t" +
            //                          engines[i].PlayOrderId + "\t" +
            //                          engines[i].MoveModel + "\t" +
            //                          engines[i].NameDisplay + "\t" +
            //                          engines[i].GroupNum + "\t" +
            //                          VectorToString(engines[i].EndPos) + "\t" +
            //                          VectorToString(engines[i].StartPos) + "\t" +
            //                          isMove + "\t" +
            //                          engines[i].MechanicalSoundID + "\t" +
            //                          engines[i].IntroductionSpeechID + "\n";
            //}
            //        File.WriteAllText(Application.dataPath + "/Resources/Data.txt", str);
            //#if UNITY_EDITOR
            //        AssetDatabase.SaveAssets();
            //        AssetDatabase.Refresh();
            //#endif
        }


        /// <summary>
        /// 将string转vector
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Vector3 StringToVector(String source)
        {
            Vector3 ver = new Vector3();
            if (null == source)
            {
                return ver;
            }
            string[] lineInfo = source.Split(',');
            ver.x = (float)Convert.ToDouble(lineInfo[0]);
            ver.y = (float)Convert.ToDouble(lineInfo[1]);
            ver.z = (float)Convert.ToDouble(lineInfo[2]);
            return ver;
        }

        /// <summary>
        /// vector转string
        /// </summary>
        /// <param name="ver"></param>
        /// <returns></returns>
        public static string VectorToString(Vector3 ver)
        {
            return ver.x + "," + ver.y + "," + ver.z;
        }
    }
}
