using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SubPool
{
    List<GameObject> m_objects = new List<GameObject>();
    private GameObject prefab;


    public SubPool(GameObject go) 
    {
        this.prefab = go;
    }

    public GameObject Spawn()
    {
        GameObject go = null;
        foreach (GameObject item in m_objects)
        {
            if (!item.activeSelf)
            {
                go = item;
                break;
            }
        }
        if (go == null)
        {
            go = GameObject.Instantiate<GameObject>(prefab);
            m_objects.Add(go);
        }
        go.SetActive(true);
        go.SendMessage("Spawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }

    public void UnSpawn(GameObject go) 
    {
        if (ContainsItem(go))
        {
            go.SendMessage("UnSpawn", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }

    public void UnSpawnAll() 
    {
        foreach (GameObject item in m_objects)
        {
            if (item.activeSelf)
            {
                UnSpawn(item);
            }
        }
    }


    public bool ContainsItem(GameObject go) 
    {
        return m_objects.Contains(go);
    }
}
