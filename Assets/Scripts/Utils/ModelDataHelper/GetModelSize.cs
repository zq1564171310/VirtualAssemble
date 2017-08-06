/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 获取零件的真实大小
/// </summary>
namespace WyzLink.Utils.ModelDataHelper
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Parts;

    public class GetModelSize : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        /// <summary>
        /// 获取零件的大小尺寸
        /// </summary>
        /// <param name="partModel"></param>
        /// <returns></returns>
        public Vector3 GetPartModelRealSize(GameObject partModel)
        {
            return partModel.GetComponent<Node>().GetDimensions();
        }

        /// <summary>
        /// 获取工具大大小尺寸
        /// </summary>
        /// <param name="toolModel"></param>
        /// <returns></returns>
        public Vector3 GetToolModelRealSize(GameObject toolModel)
        {
            Vector3 Ver;
            Transform[] Trans = toolModel.GetComponentsInChildren<Transform>();
            GameObject realToolModel = null;
            foreach (Transform child in Trans)
            {
                if (null != child.GetComponent<MeshFilter>())
                {
                    realToolModel = child.gameObject;
                }
            }
            if (null != realToolModel)
            {
                float xSize = realToolModel.GetComponent<MeshFilter>().mesh.bounds.size.x * realToolModel.transform.localScale.x;
                float ySize = (float)Math.Round(realToolModel.GetComponent<MeshFilter>().mesh.bounds.size.y * realToolModel.transform.localScale.y, 3);
                float zSize = realToolModel.GetComponent<MeshFilter>().mesh.bounds.size.z * realToolModel.transform.localScale.z;
                Ver = new Vector3(xSize, ySize, zSize);
            }
            else
            {
                Ver = new Vector3(0, 0, 0);
            }
            return Ver;
        }

        /// <summary>
        /// 获取统一规格需要缩放的大小比例
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public float Scaling(GameObject go, ModelType type)
        {
            float scalingNum = 1;
            Vector3 targeSize = GlobalVar.ModelSize;
            if (ModelType.Part == type)
            {
                Vector3 localSize = GetPartModelRealSize(go);
                List<float> list = new List<float>();
                list.Add(localSize.x);
                list.Add(localSize.y);
                list.Add(localSize.z);
                list.Sort();
                if (list[2] >= targeSize.x)
                {
                    scalingNum = list[2] / targeSize.x;                    //目前定义的x，y，z是一样大的，先把逻辑简单化，将来在做复杂逻辑
                }
                else
                {
                    scalingNum = Mathf.Max(0.5f, list[2] / targeSize.x);        // Do not make the parts too big
                }
            }
            else if (ModelType.Tools == type)
            {

            }
            return scalingNum;
        }
    }
}