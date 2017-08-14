using UnityEngine;

/// <summary>
/// 存放项目资源对象，包含脚本，物体，材质，后续UI优化以后，将只保留材质贴图等固定资源，实现模块化，将不在获取物体，只获取绝对位置的脚本和物体
/// </summary>
public class GlobalVar : MonoBehaviour
{
    //物体
    public static GameObject _ToolsGameObjects = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ToolsPanel/ToolsGameObject");         //获取工具类型UI对象

    public static GameObject _ErrorMassage = GameObject.Find("Canvas/UIManagerPlane/BackGroudImage/ErrorMassage");         //获取工具类型UI对象

    public static GameObject _RuntimeObject = GameObject.Find("RuntimeObject");

    public static UnityEngine.UI.Slider _Slider = GameObject.Find("Canvas/WorkSpacePanel/SliderPlane/Slider").GetComponent<UnityEngine.UI.Slider>();
    public static UnityEngine.UI.Text _SliderText = GameObject.Find("Canvas/WorkSpacePanel/SliderPlane/SliderText").GetComponent<UnityEngine.UI.Text>();

    //材质
    public static Material NextInstallMate = Resources.Load<Material>("NextInstallMate");
    public static Material HighLightMate = Resources.Load<Material>("HighLightMate");
    public static Material HideLightMate = Resources.Load<Material>("Test2");

    //音频

    //大小
    public static Vector3 ModelSize = new Vector3(0.15f, 0.15f, 0.15f);                                         //定义零件架上零件大小，统一规格摆放到零件架上
}
