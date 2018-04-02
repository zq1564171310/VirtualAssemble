﻿/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// This class contains all the flow information like whether the part is ready to be installed
/// </summary>

namespace WyzLink.Assemble
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using WyzLink.Parts;

    public class DependencyGraph
    {
        private class GraphNode
        {
            public IFlowNode node;
            public IList<GraphNode> previousNodes = new List<GraphNode>();
            public IList<GraphNode> nextNodes = new List<GraphNode>();

            public GraphNode(IFlowNode node)
            {
                this.node = node;
            }
        }

        private IDictionary<int, GraphNode> nodeList;
        private IList<GraphNode> headers;

        public DependencyGraph(AssembleController assembleController, string flowString)
        {
            InitializeDependencyGraph(assembleController, flowString);
        }

        private void InitializeDependencyGraph(AssembleController assembleController, string flowString)
        {
            this.nodeList = LoadAllToDictionary(assembleController.GetAllNodes<IFlowNode>());
            assembleController.ParseAssembleFlowFile(flowString, (a0, a1) =>
            {
                GraphNode n0;
                GraphNode n1;
                if (!this.nodeList.TryGetValue(a0, out n0))
                {
                    Debug.LogError("Can't find the item " + a0 + " from the hierarcy.");
                    return;
                }
                if (!this.nodeList.TryGetValue(a1, out n1))
                {
                    Debug.LogError("Can't find the item " + a1 + " from the hierarcy.");
                    return;
                }
                n0.nextNodes.Add(n1);
                n1.previousNodes.Add(n0);
            });
            // TODO: Check for loops
            this.headers = new List<GraphNode>();
            foreach (var node in nodeList.Values)
            {
                if (node.previousNodes.Count == 0)
                {
                    this.headers.Add(node);
                }
            }
        }

        private IDictionary<int, GraphNode> LoadAllToDictionary(IEnumerable<IFlowNode> nodes)
        {
            IDictionary<int, GraphNode> nodeList = new Dictionary<int, GraphNode>();
            foreach (var node in nodes)
            {
                nodeList.Add(node.GetID(), new GraphNode(node));
            }
            return nodeList;
        }

        public IEnumerable<IFlowNode> GetHeaders()
        {
            Debug.Assert(headers != null);
            return headers.Select((graphNode) => graphNode.node);
        }

        public IEnumerable<IFlowNode> GetAllNodes()
        {
            return this.nodeList.Values.Select((n) => n.node);
        }

        public IEnumerable<IFlowNode> GetNextSteps(IFlowNode node)
        {
            GraphNode graphNode = GetGraphNode(node);
            if (graphNode != null)
            {
                return graphNode.nextNodes.Select((n) => n.node);
            }
            else
            {
                throw new System.InvalidOperationException("The node " + node.GetID() + " could not be found in the dpendency graph");
            }
        }

        public IEnumerable<IFlowNode> GetPreviousSteps(IFlowNode node)
        {
            GraphNode graphNode = GetGraphNode(node);
            if (graphNode != null)
            {
                return graphNode.previousNodes.Select((n) => n.node);
            }
            else
            {
                throw new System.InvalidOperationException("The node " + node.GetID() + " could not be found in the dpendency graph");
            }
        }

        private GraphNode GetGraphNode(IFlowNode node)
        {
            GraphNode graphNode;
            nodeList.TryGetValue(node.GetID(), out graphNode);
            return graphNode;
        }

        public bool IsNodeValidToInstall(IFlowNode node)
        {
            bool validToInstall = true;
            GraphNode graphNode = GetGraphNode(node);
            if (graphNode != null)
            {
                foreach (var pnode in graphNode.previousNodes)
                {
                    if (((Node)pnode.node).GetInstallationState() == InstallationState.NotInstalled)
                    {
                        validToInstall = false;
                        break;
                    }
                }
            }
            else
            {
                throw new System.InvalidOperationException("The node " + node.GetID() + " could not be found in the dpendency graph");
            }
            return validToInstall;
        }
    }
}
