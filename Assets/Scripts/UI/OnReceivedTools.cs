using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WyzLink.Manager;

public class OnReceivedTools : MonoBehaviour {

    private List<GameObject> ToolsList; //工具集合
    private List<string> ToolsType = new List<string>();//工具类型集合
                                                        // Use this for initialization
    void Start ()
    {
        //ToolsList = AddToolsManager.ToolsList;
        //ToolsType = AddToolsManager.ToolsType;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
