/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// UI处理
/// </summary>
namespace WyzLink.Manager
{
    using System.Collections;
    using UnityEngine;
    using WyzLink.Parts;

    public class UIPlaneManager : MonoBehaviour
    {
        public int MenuNum;        //菜单选择项
        public int TypeNum;        //类型选择项
        public bool UIRefreshFlag; //UI刷新标记，一旦菜单选择项或者类型选择项发生改变，那么将标记改为True

        // Use this for initialization
        void Start()
        {
        }
    }
}
