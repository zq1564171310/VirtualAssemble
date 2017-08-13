/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The extension for handle drawing
/// </summary>

namespace WyzLink.Parts
{
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
            DrawWireDisc(transform, center - offset, Vector3.forward, radius);
            DrawWireDisc(transform, center + offset, Vector3.forward, radius);
            for (int i = 0; i < segments; i++)
            {
                var angle = Mathf.PI * 2 * i / segments;
                var radical = new Vector3(radius * Mathf.Sin(angle), radius * Mathf.Cos(angle), 0);
                DrawLine(transform, center + radical + offset, center + radical - offset);
            }
        }

        private static void DrawWireDisc(Transform transform, Vector3 center, Vector3 normal, float radius)
        {
            Handles.DrawWireDisc(transform.TransformPoint(center), transform.TransformDirection(normal), transform.TransformScalar(radius));
        }

        private static void DrawLine(Transform transform, Vector3 p1, Vector3 p2)
        {
            Handles.DrawLine(transform.TransformPoint(p1), transform.TransformPoint(p2));
        }
    }
}