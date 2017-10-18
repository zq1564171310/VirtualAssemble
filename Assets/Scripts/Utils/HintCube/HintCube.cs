using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintCube : MonoBehaviour
{
    private GameObject Arrow;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(GameObject.FindWithTag("MainCamera").transform);
    }
}
