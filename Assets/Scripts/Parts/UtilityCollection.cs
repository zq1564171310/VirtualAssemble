/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The factory pattern for creating utilities
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;
    using WyzLink.UI;
    using WyzLink.Utils;

    public class UtilityCollection : Singleton<UtilityCollection>
    {
        public LabelUI labelUI;

        public LabelUI CreateLabelUI(Transform parentTransform)
        {
            return GameObject.Instantiate<LabelUI>(this.labelUI, parentTransform);
        }
    }
}