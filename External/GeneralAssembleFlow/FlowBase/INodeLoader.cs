/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Assemble
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public interface INodeLoader
    {
        IEnumerable<T> GetAllNodes<T>(Transform transform = null);
        TextAsset GetAssembleFlowAsset();
        void SetAssembleFlowAsset(TextAsset asset);
        void ParseAssembleFlowFile(string text, Action<int, int> connect);
    }
}