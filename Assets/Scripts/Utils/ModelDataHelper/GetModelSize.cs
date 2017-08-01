using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetModelSize : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        // Debug.Log(GetModelRealSize(GlobalVar._Parts86).ToString("f4"));
    }

    public Vector3 GetPartModelRealSize(GameObject partModel)
    {
        Vector3 Ver;
        float xSize = partModel.GetComponent<MeshFilter>().mesh.bounds.size.x * partModel.transform.localScale.x;
        float ySize = (float)Math.Round(partModel.GetComponent<MeshFilter>().mesh.bounds.size.y * partModel.transform.localScale.y, 3);
        float zSize = partModel.GetComponent<MeshFilter>().mesh.bounds.size.z * partModel.transform.localScale.z;
        Ver = new Vector3(xSize, ySize, zSize);
        return Ver;
    }

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
}
