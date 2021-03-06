﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>
namespace WyzLink.Parts
{
    using UnityEngine;

    [AddComponentMenu("接口/螺丝")]
    public class 螺丝 : ConnectorBase
    {
        public float 直径;
        public float 长度;
        public int 螺纹号;

        public override string GetName()
        {
            return "螺丝" + " M" + 螺纹号;
        }

        public override ConnectorType GetConnectorType()
        {
            return ConnectorType.BeginConnector;
        }
    }
}