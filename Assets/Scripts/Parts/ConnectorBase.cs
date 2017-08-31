/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The node is the spot to install the part on. It holds the right position for the part
/// </summary>

namespace WyzLink.Parts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using WyzLink.Utils;

    public abstract class ConnectorBase : MonoBehaviour
    {
        private Node parentNode = null;

        private float connectorDetectionRadius = 0.01f; // TODO: Need adjustment
        private Coroutine connectionDetectionCoroutine = null;
        private LineRenderer lineRenderer;
        public bool isConnectorOpen = true;

        private const int MaxRaycastBufferSize = 20;

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

            // The collider is used to detect the connectors get close to each other
            this.gameObject.layer = 8;  // ConnectorLayer
            //var collider = this.gameObject.AddComponent<SphereCollider>();
            //collider.radius = 0.001f;       // TODO: GetRadius for all 
        }

        protected virtual void Start()
        {
            if (parentNode != null && parentNode.displayConnectorLabels && GlobalConfig.Instance.DisplayLabels)
            {
                if (null != UtilityCollection.Instance)
                {
                    var label = UtilityCollection.Instance.CreateLabelUI(this.transform);
                    label.SetLabelText(GetName());
                }
            }

            // TODO: Debugging only
            StartProbingConnection();
        }

        public abstract string GetName();
        public abstract ConnectorType GetConnectorType();
        public virtual bool Match(ConnectorBase connector)
        {
            Debug.Log("Invalid call to the match method");
            return false;
        }

        //
        // Test connections
        //
        public void StartProbingConnection()
        {
            if (this.isConnectorOpen)
            {
                if (this.GetConnectorType() == ConnectorType.EndConnector)
                {
                    this.connectionDetectionCoroutine = StartCoroutine(_DetectConnection());
                }
                else
                {
                    CreateConnectorColliderCollider();
                }
            }
        }

        private void CreateConnectorColliderCollider()
        {
            var collider = this.gameObject.AddComponent<SphereCollider>();
            collider.radius = 0.05f;// TODO: Use BoudingBox
        }

        private IEnumerator _DetectConnection()
        {
            if (this.lineRenderer == null)
            {
                this.lineRenderer = this.gameObject.AddComponent<LineRenderer>();
                this.lineRenderer.useWorldSpace = false;
                this.lineRenderer.material = GlobalConfig.Instance.lineMaterialBase;
                this.lineRenderer.startWidth = 0.001f;
                this.lineRenderer.endWidth = 0.001f;
            }
            yield return -1;
            // If not finished, keep detecting
            RaycastHit[] resultsBuffer = new RaycastHit[MaxRaycastBufferSize];
            float detectDistance = 0.20f;
            int layerMask = 0x100;
            while (true)
            {
                var connectedConnectors = ProbeConnectors(resultsBuffer, detectDistance, layerMask).ToList();
                float longestDistance = 0.05f;      // Default length of line
                foreach (var connector in connectedConnectors)
                {
                    //connector.SetSelected();
                    var distance = Vector3.Distance(this.transform.position, connector.transform.position) * 10;
                    if (distance > longestDistance)
                    {
                        longestDistance = distance + 0.05f;
                    }
                    if (connector.GetConnectorType() == ConnectorType.EndConnector && this.Match(connector))
                    {
                        // TODO: 
                    }
                }
                RenderRay(longestDistance);
                yield return -1;
            }
        }

        private void RenderRay(float distance)
        {
            this.lineRenderer.SetPosition(0, Vector3.zero);
            this.lineRenderer.SetPosition(1, Vector3.back * distance);
        }

        private IEnumerable<ConnectorBase> ProbeConnectors(RaycastHit[] results, float maxDistance, int layerMask)
        {
            int numOfResults = Physics.RaycastNonAlloc(this.transform.position, -this.transform.forward, results, maxDistance, layerMask);
            if (MaxRaycastBufferSize - numOfResults < 2)
            {
                Debug.LogError("The MaxRaycastBufferSize is getting filled up: " + numOfResults + "/" + MaxRaycastBufferSize);
            }
            List<ConnectorBase> connectors = new List<ConnectorBase>();
            for (int i = 0; i < numOfResults; i++)
            {
                var connector = results[i].transform.GetComponent<ConnectorBase>();
                if (connector != null)
                {
                    connectors.Add(connector);
                }
            }
            return connectors;
        }

        public void EndDetectingConnection()
        {
            RemoveConnectorColliderCollider();

            if (this.connectionDetectionCoroutine != null)
            {
                StopCoroutine(this.connectionDetectionCoroutine);
                this.connectionDetectionCoroutine = null;
            }
        }

        private void RemoveConnectorColliderCollider()
        {
            var collider = this.GetComponent<SphereCollider>();
            Destroy(collider);
        }
    }
}