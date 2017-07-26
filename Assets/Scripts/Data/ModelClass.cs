using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelClass : MonoBehaviour
{
    //名称
    public string Name;

    //零件类别
    public int ModelType;

    // Use this for initialization
    void Start()
    {

    }
}

/// <summary>
/// UI菜单类别
/// </summary>
public enum UIMenu
{
    //零件
    Part,
    //工具
    Tools,
}

/// <summary>
/// 零件类型
/// </summary>
public enum UIPartsType
{
    Parts1,
    Parts2,
    Parts3,
}

/// <summary>
/// 工具类型
/// </summary>
public enum UIToolsType
{
    Tools1,
    Tools2,
    Tools3,
}
