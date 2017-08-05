/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 所有零件控制类，对所有零件做操作，而不是单一零件
/// </summary>
namespace WyzLink.Control
{
    using HoloToolkit.Unity;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Parts;

    public class NodesController : Singleton<NodesController>
    {
        public List<Node> NodeList = new List<Node>();              //存放所有零件

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<Node> GetNodeList()
        {
            return NodeList;
        }
    }
}
