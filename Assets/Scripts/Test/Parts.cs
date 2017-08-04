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
        //StartPos = gameObject.transform.position;
        //EndPos = GameObject.Find("Engine/Model/" + gameObject.name).transform.position;
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
            gameObject.transform.position = EndPos;
            GlobalVar._PartsManager.AssembleFlag = false;
            AssebleStaus = 1;
            if (gameObject.name == "NONE 106")
            {
                GameObject.Find("Engine/Model/NONE 106").SetActive(false);
                GameObject.Find("Engine/Model/NONE 196").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 196").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 196")
            {
                GameObject.Find("Engine/Model/NONE 196").SetActive(false);
                GameObject.Find("Engine/Model/NONE 107").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 107").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 107")
            {
                GameObject.Find("Engine/Model/NONE 107").SetActive(false);
                GameObject.Find("Engine/Model/NONE 109").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 109").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 109")
            {
                GameObject.Find("Engine/Model/NONE 109").SetActive(false);
                GameObject.Find("Engine/Model/NONE 101").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 101").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 101")
            {
                GameObject.Find("Engine/Model/NONE 101").SetActive(false);
                GameObject.Find("Engine/Model/NONE 83").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 83").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 83")
            {
                GameObject.Find("Engine/Model/NONE 83").SetActive(false);
                GameObject.Find("Engine/Model/NONE 208").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 208").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 208")
            {
                GameObject.Find("Engine/Model/NONE 208").SetActive(false);
                GameObject.Find("Engine/Model/NONE 18").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 18").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 18")
            {
                GameObject.Find("Engine/Model/NONE 18").SetActive(false);
                GameObject.Find("Engine/Model/NONE 5").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 5").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 5")
            {
                GameObject.Find("Engine/Model/NONE 5").SetActive(false);
                GameObject.Find("Engine/Model/NONE 21").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 21").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 21")
            {
                GameObject.Find("Engine/Model/NONE 21").SetActive(false);
                GameObject.Find("Engine/Model/NONE 22").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 22").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 22")
            {
                GameObject.Find("Engine/Model/NONE 22").SetActive(false);
                GameObject.Find("Engine/Model/NONE 35").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 35").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 35")
            {
                GameObject.Find("Engine/Model/NONE 35").SetActive(false);
                GameObject.Find("Engine/Model/NONE 43").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 43").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 43")
            {
                GameObject.Find("Engine/Model/NONE 43").SetActive(false);
                GameObject.Find("Engine/Model/NONE 86").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 86").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 86")
            {
                GameObject.Find("Engine/Model/NONE 86").SetActive(false);
                GameObject.Find("Engine/Model/NONE 91").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 91").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 91")
            {
                GameObject.Find("Engine/Model/NONE 91").SetActive(false);
                GameObject.Find("Engine/Model/NONE 92").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 92").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 92")
            {
                GameObject.Find("Engine/Model/NONE 92").SetActive(false);
                GameObject.Find("Engine/Model/NONE 93").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 93").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 93")
            {
                GameObject.Find("Engine/Model/NONE 93").SetActive(false);
                GameObject.Find("Engine/Model/NONE 99").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 99").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 99")
            {
                GameObject.Find("Engine/Model/NONE 99").SetActive(false);
                GameObject.Find("Engine/Model/NONE 100").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 100").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 100")
            {
                GameObject.Find("Engine/Model/NONE 100").SetActive(false);
                GameObject.Find("Engine/Model/NONE 103").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 103").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 103")
            {
                GameObject.Find("Engine/Model/NONE 103").SetActive(false);
                GameObject.Find("Engine/Model/NONE 143").SetActive(true);
                GameObject.Find("Models/ModelsManager/Engine/8.0 BTDA_44215_REV C/NONE 143").AddComponent<HandDraggable>().enabled = true;
            }
            else if (gameObject.name == "NONE 143")
            {
                GameObject.Find("Engine/Model/NONE 143").SetActive(false);
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
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GlobalVar._ErrorMassage.SetActive(false);
        if (null != gameObject.GetComponent<MeshFilter>())
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.NomalMate;
        }
    }
}
