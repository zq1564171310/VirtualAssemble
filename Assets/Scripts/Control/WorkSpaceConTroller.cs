/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 工作区控制类
/// </summary>
namespace WyzLink.Control
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Parts;

    public class WorkSpaceConTroller : MonoBehaviour
    {
        private List<WorkSpace> WorkSpaceList = new List<WorkSpace>();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 获取工作区的集合
        /// </summary>
        /// <returns></returns>
        private List<WorkSpace> GetWorkSpaceList()
        {
            return WorkSpaceList;
        }

        /// <summary>
        /// 往工作区集合中添加元素
        /// </summary>
        /// <param name="workSpace"></param>
        private void AddWorkSpaceListItem(WorkSpace workSpace)
        {
            WorkSpaceList.Add(workSpace);
        }

        /// <summary>
        /// 删除工作区集合中的某个元素
        /// </summary>
        /// <param name="workSpace"></param>
        private void DeleteWorkSpaceItem(WorkSpace workSpace)
        {
            WorkSpaceList.Remove(workSpace);
        }
    }
}
