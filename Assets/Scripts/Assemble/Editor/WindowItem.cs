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
        //
        // Window structure
        //
        private Rect windowRect;
        private int id;
        private List<WindowItem> previousSteps = new List<WindowItem>();
        private List<WindowItem> nextSteps = new List<WindowItem>();
        private static int windowIndex = 0;

        //
        // Window properties
        //
        private int nodeId;
        private string nodeName;
        private Node node;

        //
        // Const parameters
        //
        private const float windowWidth = 80;
        private const float windowHeight = 37;
        private const float windowMarginLeft = 10;
        private const float connectionAreaMargin = 20;

        public const float LayoutGapVertically = 5;
        public const float LayoutGapHorizontally = 30;

        //
        // Window areas
        //
        private Rect labelRect = new Rect(3, 18, 60, 20);
        private Rect buttonRect = new Rect(80 - 15, 17, 12, 21);
        private Rect dragRect = new Rect(0, 0, 80 - connectionAreaMargin, 37);

        //
        // Layout properties
        //
        private float childHeight;
        public int LayoutToken { get; set; }        // Used to label the rounds of layout
        public int Layer { get; set; }

        //
        // Static properties
        //
        private static GUIStyle styleWindowNormal = null;
        private static GUIStyle styleWindowDeleted = null;
        private static GUIStyle styleLabelNormal = null;
        private static GUIStyle styleLabelDeleted = null;

        //
        // Window states for connection
        //
        public enum UIState
        {
            normalState,
            connectingState,
        };
        public UIState uiState = UIState.normalState;
        private bool isDeleted = false;


        public WindowItem(Node node)
        {
            this.id = WindowItem.windowIndex++;
            this.windowRect = new Rect(Vector2.zero, new Vector2(windowWidth, windowHeight));
            this.nodeId = node.nodeId;
            this.nodeName = node.partName;
            this.node = node;
        }

        public int GetNodeId()
        {
            return this.nodeId;
        }

        public void Update()
        {
            if (styleWindowDeleted == null)
            {
                styleWindowNormal = new GUIStyle(GUI.skin.window);
                styleWindowDeleted = new GUIStyle(GUI.skin.window);
                styleWindowDeleted.normal.textColor = Color.red;
                styleLabelNormal = new GUIStyle(GUI.skin.label);
                styleLabelDeleted = new GUIStyle(GUI.skin.label);
                styleLabelDeleted.normal.textColor = Color.red;
            }

            this.windowRect = GUI.Window(this.id, this.windowRect, windowFunction, nodeId.ToString(), this.isDeleted ? styleWindowDeleted : styleWindowNormal);
            foreach (var w in previousSteps)
            {
                curveFromTo(w, this);
            }
        }

        public bool HitTest(Vector2 point, bool hitConnectingArea)
        {
            return hitConnectingArea ? this.windowRect.Contains(point) && (windowRect.width - (point.x - windowRect.x) <= connectionAreaMargin) : this.windowRect.Contains(point);
        }

        public GenericMenu CreateMenu(WindowManager windowManager)
        {
            var menu = new GenericMenu();
            foreach (var window in this.nextSteps)
            {
                menu.AddItem(new GUIContent("Remove " + window.nodeName), false, 
                    () => {
                        this.RemoveNextSteps(window);
                        windowManager.SetDirty();
                    });
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

        internal bool HasNextSteps()
        {
            return this.nextSteps.Count > 0;
        }

        internal bool HasPreviousSteps()
        {
            return this.previousSteps.Count > 0;
        }

        private void windowFunction(int id)
        {
            GUI.Label(labelRect, this.nodeName, this.isDeleted ? styleLabelDeleted : styleLabelNormal);
            GUI.Label(buttonRect, ">", this.isDeleted ? styleLabelDeleted : styleLabelNormal);
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

        internal Rect GetWindowRect()
        {
            return this.windowRect;
        }

        public void MarkDeleted(bool isDeleted)
        {
            this.isDeleted = isDeleted;
        }

        public bool IsDeleted()
        {
            return this.isDeleted;
        }

        //
        // Game object related
        //
        internal void ShowObject(bool display)
        {
            if (this.node != null)
            {
                this.node.gameObject.SetActive(display);
            }
        }

        public void SelectAndFrameObject()
        {
            if (this.node != null)
            {
                Selection.activeGameObject = this.node.gameObject;
                SceneView.FrameLastActiveSceneView();
            }
        }
    }
}