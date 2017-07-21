using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using HoloToolkit.Unity;

public class ObjectPool:Singleton<ObjectPool>
{
    public string ResourceDir = "";
    Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();

    public GameObject Spawn(string name) 
    {
        GameObject go = null;
        if (!m_pools.ContainsKey(name))
            Rigister(name);
        SubPool pool = m_pools[name];
        return pool.Spawn();
    }

    public void UnSpawn(GameObject go) 
    {
        foreach (SubPool item in m_pools.Values)
        {
            if (item.ContainsItem(go))
            {
                item.UnSpawn(go);
            }
        }
    }


    public void UnSpawnAll() 
    {
        foreach (SubPool item in m_pools.Values)
        {
            item.UnSpawnAll();
        }
    }


    private void Rigister(string name) 
    {
        string path = "";
        if (string.IsNullOrEmpty(ResourceDir))
        {
            path = name;
        }
        else {
            path = ResourceDir + "/" + name;
        }

        GameObject go = Resources.Load<GameObject>(path);
        m_pools.Add(go.name, new SubPool(go));

    }

}
