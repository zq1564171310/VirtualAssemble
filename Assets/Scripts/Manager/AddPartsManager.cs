using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPartsManager : MonoBehaviour
{
    private Transform[] PartsTransform;

    // Use this for initialization
    void Start()
    {
        PartsTransform = GlobalVar._PartsFatherGameObject.GetComponentsInChildren<Transform>();
        Parts parts;
        foreach (Transform child in PartsTransform)
        {
            if (null != child.GetComponent<MeshFilter>())
            {
                child.gameObject.AddComponent<Parts>();
                parts = child.GetComponent<Parts>();
                parts.EndPos = child.transform.position;
                parts.LocalSize = child.transform.localScale;
                parts.Name = child.name;
                parts.PartsGameObject = child.gameObject;
                GlobalVar._PartsManager.PartsList.Add(parts);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
