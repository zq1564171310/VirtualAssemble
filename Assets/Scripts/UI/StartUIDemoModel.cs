/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI 开始界面
/// </summary>
namespace WyzLink.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using WyzLink.Assemble;
    using WyzLink.LogicManager;

    public class StartUIDemoModel : MonoBehaviour, StartUI
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 重新开始按钮的点击事件
        /// </summary>
        public void Restart()
        {
            //演示模式暂时没用到，空实现
        }

        /// <summary>
        /// 读取存档点击按钮
        /// </summary>
        public void Record()
        {

        }
    }
}
