using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 存放项目资源对象，包含脚本，物体，材质，后续UI优化以后，将只保留材质贴图等固定资源，实现模块化，将不在获取物体，只获取绝对位置的脚本和物体
/// </summary>
public class GlobalVar : MonoBehaviour
{
    //物体
    public static GameObject _RuntimeObject = GameObject.Find("RuntimeObject");

    public static GameObject _MainWorkSpace = GameObject.Find("Canvas/Floor/MainWorkSpace");

    //UI
    public static UIPartsPanelClass _UIPartsPanelClass = GameObject.Find("Canvas/BG/PartsPanel/PartsClassPanel").GetComponent<UIPartsPanelClass>();
    public static UIPartsPage _UIPartsPage = GameObject.Find("Canvas/BG/PartsPanel/SinglePartPanel").GetComponent<UIPartsPage>();
    public static Slider _Slider = GameObject.Find("Canvas/Floor/MainWorkSpace/SliderPlane/Slider").GetComponent<Slider>();
    public static Text _SliderText = GameObject.Find("Canvas/Floor/MainWorkSpace/SliderPlane/SliderText").GetComponent<Text>();
    public static Text _Tips = GameObject.Find("Canvas/BG/PartsPanel/Tips").GetComponent<Text>();         //提示框

    //材质
    public static Material NextInstallMate = Resources.Load<Material>("NextInstallMate");
    public static Material HighLightMate = Resources.Load<Material>("HighLightMate");
    public static Material HideLightMate = Resources.Load<Material>("Test2");

    //音频

    //大小
    public static Vector3 ModelSize = new Vector3(0.15f, 0.15f, 0.15f);                //定义零件架上零件大小，统一规格摆放到零件架上
}
