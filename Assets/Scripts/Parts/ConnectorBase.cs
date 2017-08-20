/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;

    public abstract class ConnectorBase : MonoBehaviour
    {
        private Node parentNode = null;

        private void Awake()
        {
            var transform = this.transform;
            while (transform != null)
            {
                Node node = transform.GetComponent<Node>();
                if (node != null)
                {
                    this.parentNode = node;
                    break;
                }
                transform = transform.parent;
            }
        }

        protected virtual void Start()
        {
            if (parentNode != null && parentNode.displayConnectorLabels)
            {
                var label = UtilityCollection.Instance.CreateLabelUI(this.transform);
                label.SetLabelText(GetName());
            }
        }

        public abstract string GetName();
    }
}