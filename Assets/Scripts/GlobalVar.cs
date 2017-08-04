using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVar : MonoBehaviour
{
    //脚本实例话
    public static UIPlaneManager _UIManagerPlaneScript = GameObject.Find("Canvas/UIManagerPlane").GetComponent<UIPlaneManager>();   //UI管理类的实例
    public static PartsManager _PartsManager = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel").GetComponent<PartsManager>();

    public static GetModelSize _GetModelSize = GameObject.Find("Models").GetComponent<GetModelSize>();

    public static PaginationUtil _PaginationUtil = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject").GetComponent<PaginationUtil>();

    //物体
    public static GameObject _PartsTypePlane = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/TypePanel");         //获取零件类型UI对象
    public static GameObject _ToolsTypePlane = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ToolsPanel/TypePanel");         //获取工具类型UI对象

    public static GameObject _PartsGameObjects = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/PartsPanel/PartsGameObject");         //获取零件类型UI对象
    public static GameObject _ToolsGameObjects = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ToolsPanel/ToolsGameObject");         //获取工具类型UI对象

    public static GameObject _ErrorMassage = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ErrorMassage");         //获取工具类型UI对象

    public static GameObject _FatherGameObject = GameObject.Find("Engine/Model");         //获取工具类型UI对象

    public static GameObject _PartsFatherGameObject = GameObject.Find("Models");

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
    public static Vector3 ModelSize = new Vector3(0.2f, 0.2f, 0.2f);                                         //定义零件架上零件大小，统一规格摆放到零件架上
}
