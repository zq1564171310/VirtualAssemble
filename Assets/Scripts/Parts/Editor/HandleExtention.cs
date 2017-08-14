/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The extension for handle drawing
/// </summary>

namespace WyzLink.Parts
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public static class HandleExtention
    {
        public static float TransformScalar(this Transform transform, float scalar)
        {
            while (transform != null)
            {
                scalar *= transform.localScale.x;
                transform = transform.parent;
            }
            return scalar;
        }

        public static void DrawColumn(Transform transform, Vector3 center, float radius, float thickness)
        {
            int segments = 10;
            var offset = Vector3.forward * thickness / 2;
            HandleExtention.DrawWireDisc(transform, center - offset, Vector3.forward, radius);
            HandleExtention.DrawWireDisc(transform, center + offset, Vector3.forward, radius);
            for (int i = 0; i < segments; i++)
            {
                var angle = Mathf.PI * 2 * i / segments;
                var radial = new Vector3(radius * Mathf.Sin(angle), radius * Mathf.Cos(angle), 0);
                HandleExtention.DrawLine(transform, center + radial + offset, center + radial - offset);
            }
        }

        public static void DrawDualHole(Transform transform, Vector3 center, float distanceBetweenCenters, float radius, float thickness)
        {
            var center1 = center + Vector3.left * distanceBetweenCenters / 2;
            var center2 = center - Vector3.left * distanceBetweenCenters / 2;
            HandleExtention.DrawColumn(transform, center1, radius, thickness);
            HandleExtention.DrawColumn(transform, center2, radius, thickness);

            HandleExtention.DrawBeam(transform, center1, center2, thickness, radius * 2);
        }

        public static void DrawBeam(Transform transform, Vector3 p1, Vector3 p2, float width, float height)
        {
            HandleExtention.DrawLineWithOffset(transform, p1, p2, Vector3.forward * width / 2 + Vector3.up * height / 2);
            HandleExtention.DrawLineWithOffset(transform, p1, p2, Vector3.forward * width / 2 - Vector3.up * height / 2);
            HandleExtention.DrawLineWithOffset(transform, p1, p2, -Vector3.forward * width / 2 + Vector3.up * height / 2);
            HandleExtention.DrawLineWithOffset(transform, p1, p2, -Vector3.forward * width / 2 - Vector3.up * height / 2);
        }

        private static void DrawLineWithOffset(Transform transform, Vector3 p1, Vector3 p2, Vector3 offset)
        {
            HandleExtention.DrawLine(transform, p1 + offset, p2 + offset);
        }

        public static void DrawWireDisc(Transform transform, Vector3 center, Vector3 normal, float radius)
        {
            Handles.DrawWireDisc(transform.TransformPoint(center), transform.TransformDirection(normal), transform.TransformScalar(radius));
        }

        public static void DrawEnd(Transform transform, Vector3 center, float radius, float endInSet = 0.6f)
        {
            int segments = 10;
            for (int i = 0; i < segments; i++)
            {
                var angle = Mathf.PI * 2 * i / segments;
                var radial = new Vector3(radius * Mathf.Sin(angle), radius * Mathf.Cos(angle), 0);
                HandleExtention.DrawLine(transform, center + Vector3.forward * radius * endInSet, center + radial);
            }
        }

        public static void DrawLine(Transform transform, Vector3 p1, Vector3 p2)
        {
            Handles.DrawLine(transform.TransformPoint(p1), transform.TransformPoint(p2));
        }
    }
}