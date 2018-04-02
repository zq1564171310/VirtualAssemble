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

    public interface NodesController
    {

        IList<Node> GetNodeList();

        void SetNodeList(List<Node> listist);

        void AddNodeList(Node node);
    }
}
