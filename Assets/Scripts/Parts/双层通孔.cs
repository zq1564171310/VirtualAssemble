/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// 零件接驳方式
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;

    [AddComponentMenu("接口/双层通孔")]
    public class 双层通孔 : ConnectorBase
    {
        public float 直径1;
        public float 直径2;
        public float 厚度1;
        public float 厚度2;

        public override string GetName()
        {
            return "双层通孔" + " Ø " + 直径1 + " Ø " + 直径2;
        }
    }
}