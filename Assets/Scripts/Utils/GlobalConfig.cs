/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// Global configuration
/// </summary>

namespace WyzLink.Utils
{
    using UnityEngine;

    public class GlobalConfig : HoloToolkit.Unity.Singleton<GlobalConfig> //: Singleton<GlobalConfig>
    {
        public bool DisplayLabels = true;

        public bool DisplayConnectorRays = true;

        public Material lineMaterialBase;
    }
}