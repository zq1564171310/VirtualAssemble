/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;

    public class Part : MonoBehaviour
    {
        [Header("零件属性")]
        [Tooltip("零件名称")]
        public string partName;
        [Tooltip("零件唯一标识")]
        public string partId;
        [Tooltip("注释")]
        public string note;
        [Tooltip("库存编号")]
        public string inventoryId;
    }
}