using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WyzLink.ToolsAndCommonParts;

public class ToolsController : Singleton<ToolsController>
{
    private List<Tool> ToolList = new List<Tool>();              //存放所有零件集合

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Tool> GetToolList()
    {
        return ToolList;
    }

    public void SetToolList(List<Tool> listist)
    {
        ToolList = listist;
    }

    public void AddToolList(Tool tool)
    {
        ToolList.Add(tool);
    }
}
