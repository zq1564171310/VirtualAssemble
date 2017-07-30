/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The item used for the flow chart windows
/// </summary>

namespace WyzLink.Assemble
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
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

        private const float windowWidth = 80;
        private const float windowHeight = 37;
        private const float windowMarginLeft = 10;
        private const float connectionAreaMargin = 20;

        public const float LayoutGapVertically = 5;
        public const float LayoutGapHorizontally = 30;

        private Color s = new Color(0.4f, 0.4f, 0.5f);
        private Rect labelRect = new Rect(3, 18, 60, 20);
        private Rect buttonRect = new Rect(80 - 15, 17, 12, 21);
        private Rect dragRect = new Rect(0, 0, 80 - connectionAreaMargin, 37);

        // Layout properties
        private float childHeight;

        public enum UIState
        {
            normalState,
            connectingState,
        };

        public UIState uiState = UIState.normalState;

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

        public bool HitTest(Vector2 point, bool hitConnectingArea)
        {
            return hitConnectingArea ? this.windowRect.Contains(point) && (windowRect.width - (point.x - windowRect.x) <= connectionAreaMargin) : this.windowRect.Contains(point);
        }

        public GenericMenu CreateMenu()
        {
            var menu = new GenericMenu();
            foreach (var window in this.nextSteps)
            {
                menu.AddItem(new GUIContent("Remove " + window.nodeName), false, () => this.RemoveNextSteps(window));
            }
            return menu;
        }

        public static void curveFromTo(WindowItem w1, WindowItem w2)
        {
            Color s = new Color(0.4f, 0.4f, 0.5f);
            Drawing.curveFromTo(w1.windowRect, w2.windowRect, new Color(0.3f, 0.7f, 0.4f), s);
        }

        public static void curveFromTo(WindowItem w1, Vector2 point)
        {
            Color s = new Color(0.4f, 0.4f, 0.5f);
            Drawing.curveFromTo(w1.windowRect, new Rect(point, Vector2.zero), new Color(0.3f, 0.7f, 0.4f), s);
        }

        public void AddPreviousStep(WindowItem window)
        {
            if (window != null && !this.previousSteps.Contains(window))
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

        public void RemoveNextSteps(WindowItem window)
        {
            if (this.nextSteps.Contains(window))
            {
                this.nextSteps.Remove(window);
                window.previousSteps.Remove(this);
            }
        }

        internal List<WindowItem> GetNextSteps()
        {
            return this.nextSteps;
        }

        private void windowFunction(int id)
        {
            GUI.Label(labelRect, this.nodeName);
            GUI.Label(buttonRect, ">");
            GUI.DragWindow(dragRect);
        }

        public int GetWindowWidth()
        {
            return (int)this.windowRect.width;
        }

        public void MoveTo(Vector2 point)
        {
            this.windowRect.position = point;
        }

        public float CalculateChildrenHeight()
        {
            this.childHeight = 0;
            foreach (var w in this.nextSteps)
            {
                this.childHeight += w.CalculateChildrenHeight();
            }
            if (childHeight == 0)
            {
                childHeight = windowHeight + LayoutGapVertically;
            }
            return this.childHeight;
        }

        public float GetChildrenHeight()
        {
            return this.childHeight;
        }
    }
}