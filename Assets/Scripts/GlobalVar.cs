using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVar : MonoBehaviour
{
    //脚本实例话
    public static UIPlaneManager _UIManagerPlaneScript = GameObject.Find("Canvas/UIManagerPlane").GetComponent<UIPlaneManager>();   //UI管理类的实例
    public static PartsManager _PartsManager = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject1").GetComponent<PartsManager>();

    public static GetModelSize _GetModelSize = GameObject.Find("ScriptPrefab").GetComponent<GetModelSize>();

    public static PaginationUtil _PaginationUtil = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject").GetComponent<PaginationUtil>();



    //物体
    public static GameObject _PartsTypePlane = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/TypePanel");         //获取零件类型UI对象
    public static GameObject _ToolsTypePlane = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ToolsPanel/TypePanel");         //获取工具类型UI对象

    public static GameObject _PartsGameObjects = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject");         //获取零件类型UI对象
    public static GameObject _ToolsGameObjects = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ToolsPanel/ToolsGameObject");         //获取工具类型UI对象

    public static GameObject _ErrorMassage = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ErrorMassage");         //获取工具类型UI对象

    public static GameObject _FatherGameObject = GameObject.Find("Engine/Model");         //获取工具类型UI对象

    public static GameObject _Parts208 = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 208");
    public static GameObject _Parts196 = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 196");
    public static GameObject _Parts106 = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 106");
    public static GameObject _Parts86 = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 86");
    public static GameObject _Parts101 = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 101");
    public static GameObject _Parts22 = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject/NONE 22");


    public static GameObject _PartsFatherGameObject = GameObject.Find("Models/ModelsManager");


    //UI按钮
    public static Button _PartsMenuBtn = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsBtn").GetComponent<Button>();
    public static Button _ToolsMenuBtn = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ToolsPanel/ToolsBtn").GetComponent<Button>();

    public static Button _PartsNextBtn = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/NextBtn").GetComponent<Button>();
    public static Button _PartsPreviousBtn = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PreviousBtn").GetComponent<Button>();

    //UI按钮文本
    public static Text _PartsMenuBtnText = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsBtn/Text").GetComponent<Text>();
    public static Text _ToolsMenuBtnText = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ToolsPanel/ToolsBtn/Text").GetComponent<Text>();
    

    //材质
    public static Material NomalMate = Resources.Load<Material>("Test");
    public static Material HighLightMate = Resources.Load<Material>("Test4");
    public static Material HideLightMate = Resources.Load<Material>("Test2");

    //音频


    //大小
    //public static Vector2 ModelSize = new Vector2(0.1f, 0.1f);
}
