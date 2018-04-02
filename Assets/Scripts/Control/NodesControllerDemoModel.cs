﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 演示模式所有零件控制类，对所有零件做操作，而不是单一零件
/// </summary>
namespace WyzLink.Control
{
    using HoloToolkit.Unity;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Parts;

    public class NodesControllerDemoModel : Singleton<NodesControllerDemoModel>, NodesController
    {
        private List<Node> NodeList = new List<Node>();              //存放所有零件集合

        public IList<Node> GetNodeList()
        {
            return NodeList;
        }

        public void SetNodeList(List<Node> listist)
        {
            NodeList = listist;
        }

        public void AddNodeList(Node node)
        {
            NodeList.Add(node);
        }
    }
}
