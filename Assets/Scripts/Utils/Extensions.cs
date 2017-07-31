/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// Extension utilities
/// </summary>

namespace WyzLink.Utils
{
    using UnityEngine;

    public static class Extensions
    {
        public static Rect Union(this Rect rect, Rect rect1)
        {
            var x = Mathf.Min(rect.x, rect1.x);
            var y = Mathf.Min(rect.y, rect1.y);
            var w = Mathf.Max(rect.xMax, rect1.xMax) - x;
            var h = Mathf.Max(rect.yMax, rect1.yMax) - y;
            return new Rect(x, y, w, h);
        }

        public static Rect Extend(this Rect rect, float x, float y)
        {
            return new Rect(rect.x - x, rect.y - y, rect.width + x * 2, rect.height + y * 2);
        }
    }
}