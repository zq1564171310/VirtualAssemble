/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Utils
{
    using UnityEngine;

    /// <summary>
    /// 生成下一个整数标识符
    /// </summary>
    public class IdCounter : Singleton<IdCounter>
    {
        public int currentId;

        public int GetNextId()
        {
            return this.currentId++;
        }

        private void Reset()
        {
            this.currentId = 5000;
        }
    }
}