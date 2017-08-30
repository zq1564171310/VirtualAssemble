/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;

    [AddComponentMenu("接口/螺纹通孔")]
    public class 螺纹通孔 : ConnectorBase
    {
        public float 直径;
        public float 厚度;

        public override string GetName()
        {
            return "螺纹通孔" + " Ø " + 直径;
        }

        public override ConnectorType GetConnectorType()
        {
            return ConnectorType.MiddleConnector;
        }
    }
}