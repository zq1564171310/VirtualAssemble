﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class 长孔 : ConnectorBase
    {
        public float 半径;
        public float 厚度;
        public float 长度;

        public override string GetName()
        {
            return "长孔" + " Ø " + 半径;
        }
    }
}