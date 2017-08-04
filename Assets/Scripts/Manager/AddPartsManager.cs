using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPartsManager : MonoBehaviour
{
    private Transform[] PartsTransform;

    // Use this for initialization
    void Start()
    {
        GameObject game;

        PartsTransform = GlobalVar._PartsFatherGameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in PartsTransform)
        {
            if (null != child.GetComponent<MeshFilter>())
            {
                Parts parts = new Parts();
                parts.EndPos = child.transform.position;
                parts.LocalSize = child.transform.localScale;
                parts.Name = child.name;
                parts.PartsGameObject = child.gameObject;
                GlobalVar._PartsManager.PartsList.Add(parts);

                game = GameObject.Find(("Engine/Model/") + child.name);
                if (null != game && game.name != "NONE 106")
                {
                    game.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
