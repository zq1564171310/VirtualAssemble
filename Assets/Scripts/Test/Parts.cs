using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Parts : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 StartPos;
    public Vector3 EndPos;
    public int AssebleStaus;                //0是再零件架上未安装，1安装正确
    public GameObject PartsGameObject;
    public Vector3 LocalSize;                //原本尺寸
    public string Name;

    // Use this for initialization
    void Start()
    {
        StartPos = gameObject.transform.position;
        EndPos = GameObject.Find("Engine/Model/" + gameObject.name).transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (null != gameObject.GetComponent<HandDraggable>() && gameObject.transform.position != StartPos)
        {
            GlobalVar._PartsManager.AssembleFlag = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == GameObject.Find("Engine/Model/" + gameObject.name))
        {
            Destroy(gameObject.GetComponent<HandDraggable>());
            GameObject.Find(gameObject.name + "/Text").SetActive(false);
            gameObject.transform.position = EndPos;
            GlobalVar._PartsManager.AssembleFlag = false;
            if (gameObject.name == "NONE 208")
            {
                GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 196").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 196")
            {
                GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 106").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 106")
            {
                GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 86").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 86")
            {
                GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 101").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 101")
            {
                GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 22").AddComponent<HandDraggable>().enabled = true;
            }

        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {

    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (true == GlobalVar._PartsManager.AssembleFlag && gameObject.name != "PartsBtn" && gameObject.name != "ToolsBtn")
        {
            GlobalVar._ErrorMassage.SetActive(true);
        }

        if (null != gameObject.GetComponent<MeshFilter>())
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HighLightMate;
            GameObject.Find("Engine/Model/" + gameObject.name).GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HighLightMate;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GlobalVar._ErrorMassage.SetActive(false);
        if (null != gameObject.GetComponent<MeshFilter>())
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.NomalMate;
            GameObject.Find("Engine/Model/" + gameObject.name).GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HideLightMate;
        }
    }
}
