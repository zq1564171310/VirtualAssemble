/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// This class contains all the flow information like whether the part is ready to be installed
/// </summary>

namespace WyzLink.Assemble
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class MenuUtils : MonoBehaviour
    {
        [MenuItem("Edit/修正父节点坐标")]
        static void FixParentAccordinate()
        {
            if (Selection.activeTransform != null)
            {
                FixParentAccordinate(Selection.activeTransform);
            }
        }

        private static void FixParentAccordinate(UnityEngine.Transform transform)
        {
            var diff = GetBestFitParentPosition(transform);

            // Capture the undo list
            List<Transform> undoList = new List<Transform>();
            undoList.Add(transform);
            foreach (Transform t in transform)
            {
                undoList.Add(t);
            }
            Undo.RecordObjects(undoList.ToArray(), "Fix parent position");

            foreach (Transform t in transform)
            {
                t.localPosition -= diff;
            }
            transform.position += transform.TransformVector(diff);
        }

        private static Vector3 GetBestFitParentPosition(Transform transform)
        {
            Vector3 totalVector = Vector3.zero;
            int count = 0;
            foreach (Transform t in transform)
            {
                totalVector += t.localPosition;
                count++;
            }
            var targetVector = totalVector / count;
            return targetVector;
        }
    }
}