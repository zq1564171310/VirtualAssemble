/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// UI for the connector label
/// </summary>

namespace WyzLink.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class LabelUI : MonoBehaviour
    {
        public Text text;

        public void SetLabelText(string text)
        {
            this.text.text = text;
        }
    }
}