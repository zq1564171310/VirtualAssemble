/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// 零件接驳方式
/// </summary>

namespace WyzLink.Parts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class 双层通孔 : ConnectorBase
    {
        public float 半径1;
        public float 半径2;
        public float 厚度1;
        public float 厚度2;

        public override string GetName()
        {
            return "双层通孔" + " Ø " + 半径1 + " Ø " + 半径2;
        }
    }
}