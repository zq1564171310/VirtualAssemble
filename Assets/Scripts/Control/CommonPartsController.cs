/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 所有常用零件控制类，真实数据存放
/// </summary>
namespace WyzLink.Control
{
    using HoloToolkit.Unity;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.ToolsAndCommonParts;

    public class CommonPartsController : Singleton<CommonPartsController>
    {
        private List<CommonParts> CommonPartsList = new List<CommonParts>();              //存放所有零件集合

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<CommonParts> GetCommonPartsList()
        {
            return CommonPartsList;
        }

        public void SetCommonPartsList(List<CommonParts> listist)
        {
            CommonPartsList = listist;
        }

        public void AddCommonPartsList(CommonParts commonParts)
        {
            CommonPartsList.Add(commonParts);
        }
    }
}

