using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolPanel : MonoBehaviour {

    [SerializeField]
    private GameObject Tool_Panel_1, Tool_Panel_2, Tool_Panel_3;
    [SerializeField]
    private Toggle ToolClass_1, ToolClass_2, ToolClass_3;




    // Use this for initialization
    void Start ()
    {
        ToolClass_1.onValueChanged.AddListener(OnValChanged1);
        ToolClass_2.onValueChanged.AddListener(OnValChanged2);
        ToolClass_3.onValueChanged.AddListener(OnValChanged3);

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnValChanged1(bool check)
    {
        Tool_Panel_1.SetActive(check);
        Tool_Panel_2.SetActive(!check);
        Tool_Panel_3.SetActive(!check);
    }
    void OnValChanged2(bool check)
    {
        Tool_Panel_1.SetActive(!check);
        Tool_Panel_2.SetActive(check);
        Tool_Panel_3.SetActive(!check);
    }
    void OnValChanged3(bool check)
    {
        Tool_Panel_1.SetActive(!check);
        Tool_Panel_2.SetActive(!check);
        Tool_Panel_3.SetActive(check);
    }

}
