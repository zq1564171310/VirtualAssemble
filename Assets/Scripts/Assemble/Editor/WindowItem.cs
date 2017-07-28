/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The item used for the flow chart windows
/// </summary>

namespace WyzLink.Assemble
{
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Parts;

    public class WindowItem
    {

        private Rect windowRect;
        private int id;
        private List<WindowItem> previousSteps = new List<WindowItem>();
        private List<WindowItem> nextSteps = new List<WindowItem>();

        private int nodeId;
        private string nodeName;
        private Node node;

        private int windowWidth = 80;
        private int windowHeight = 37;
        private int windowMarginLeft = 10;

        private Color s = new Color(0.4f, 0.4f, 0.5f);
        private Rect labelRect = new Rect(3, 15, 60, 20);

        public WindowItem(int id, Vector2 position, Node node)
        {
            this.id = id;
            this.windowRect = new Rect(position, new Vector2(windowWidth, windowHeight));
            this.nodeId = node.nodeId;
            this.nodeName = node.partName;
        }

        public void Update()
        {
            this.windowRect = GUI.Window(this.id, this.windowRect, windowFunction, nodeId.ToString());
            foreach (var w in previousSteps)
            {
                curveFromTo(w, this);
            }
        }

        public static void curveFromTo(WindowItem w1, WindowItem w2)
        {
            Color s = new Color(0.4f, 0.4f, 0.5f);
            curveFromTo(w1.windowRect, w2.windowRect, new Color(0.3f, 0.7f, 0.4f), s);
        }

        public static void curveFromTo(Rect wr, Rect wr2, Color color, Color shadow)
        {
            Drawing.DrawBezierLine(
                new Vector2(wr.x + wr.width, wr.y + 3 + wr.height / 2),
                new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + 3 + wr.height / 2),
                new Vector2(wr2.x, wr2.y + 3 + wr2.height / 2),
                new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + 3 + wr2.height / 2), shadow, 5, true, 20);
            Drawing.DrawBezierLine(
                new Vector2(wr.x + wr.width, wr.y + wr.height / 2),
                new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + wr.height / 2),
                new Vector2(wr2.x, wr2.y + wr2.height / 2),
                new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + wr2.height / 2), color, 2, true, 20);
        }

        public void AddPreviousStep(WindowItem window)
        {
            if (window != null)
            {
                this.previousSteps.Add(window);
                window.AddNextStep(this);
            }
        }

        // Never add next step from external
        private void AddNextStep(WindowItem window)
        {
            if (window != null)
            {
                this.nextSteps.Add(window);
                // Don't add back to previous
            }
        }

        private void windowFunction(int id)
        {
            GUI.Label(labelRect, this.nodeName);
            GUI.DragWindow();
        }
    }
}