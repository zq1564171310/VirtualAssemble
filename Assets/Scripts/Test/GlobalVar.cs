using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    //脚本实例话
    public static UIManager _UIManagerScript = GameObject.Find("UIManager").GetComponent<UIManager>();   //UI管理类的实例

    public static Assembles _Assembles = GameObject.Find("GameObject/Cylinder").GetComponent<Assembles>();

    //物体
    public static GameObject _UIManager = GameObject.Find("UIManager");                                  //UI父物体对象
    public static GameObject _PartsTypeBtn = GameObject.Find("UIManager/Menu/PartsManager/PartsBtn");         //获取零件类型UI按钮
    public static GameObject _ToolsTypeBtn = GameObject.Find("UIManager/Menu/ToolsManager/ToolsBtn");         //获取工具类型UI按钮
    public static GameObject _PartsTypeUI = GameObject.Find("UIManager/Menu/PartsManager/Type");         //获取零件类型UI对象
    public static GameObject _ToolsTypeUI = GameObject.Find("UIManager/Menu/ToolsManager/Type");         //获取工具类型UI对象
    public static GameObject _PartsTypeText = GameObject.Find("UIManager/Menu/PartsManager/PartsText");         //获取零件类型UI对象
    public static GameObject _ToolsTypeText = GameObject.Find("UIManager/Menu/ToolsManager/ToolsText");         //获取工具类型UI对象
    public static GameObject _PartsTypeObject = GameObject.Find("UIManager/Menu/PartsManager/Parts");         //获取零件类型UI对象
    public static GameObject _ToolsTypeObject = GameObject.Find("UIManager/Menu/ToolsManager/Tools");         //获取工具类型UI对象

    public static GameObject _ErrorMassage = GameObject.Find("UIManager/Menu/ErrorMassage");         //获取工具类型UI对象

    //音频
    //public static AudioSource _AudioSource = _FatherGameObject.GetComponent<AudioSource>();                  //音频源
}
