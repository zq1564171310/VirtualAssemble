using UnityEngine;
using UnityEngine.UI;
using WyzLink.UI;

/// <summary>
/// 存放项目资源对象，包含脚本，物体，材质，后续UI优化以后，将只保留材质贴图等固定资源，实现模块化，将不在获取物体，只获取绝对位置的脚本和物体
/// </summary>
public class GlobalVar : MonoBehaviour
{
    //材质
    public static Material NextInstallMate = Resources.Load<Material>("NextInstallMate");
    public static Material HighLightMate = Resources.Load<Material>("HighLightMate");
    public static Material HideLightMate = Resources.Load<Material>("Test2");

    //音频

    //大小
    public static Vector3 ModelSize = new Vector3(0.15f, 0.15f, 0.15f);                //定义零件架上零件大小，统一规格摆放到零件架上

    //提示内容
    public static string ErrorTips1 = "安装错误！零件将被退回零件架，请选择正确的零件安装！";
    public static string ErrorTips2 = "错误：请先将上一个零件安装完成！";
}
