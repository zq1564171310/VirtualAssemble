/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The placeholder to play the animation
/// </summary>

namespace WyzLink.Parts
{
    using UnityEngine;

    public class AnimationPlayer : MonoBehaviour
    {
        [SerializeField]
        private string animationName;

        public AnimationCollection.AnimationPlay PlayAnimation()
        {
            #region Test
            if (null == AnimationCollection.Instance)
            {
                return GameObject.Find("UnityMain/AnimationCollection").GetComponent<AnimationCollection>().PlayAnimation(animationName, this.transform);
            }
            #endregion
            return AnimationCollection.Instance.PlayAnimation(animationName, this.transform);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            DrawWireCone(this.transform, 0.25f, 0.07f);
        }

        private void DrawWireCone(Transform transform, float length, float radius)
        {
            int split = 5;
            for (int i = 0; i < split; i++)
            {
                var angle = Mathf.PI * 2 * i / split;
                var point = -Vector3.forward * length + new Vector3(radius * Mathf.Sin(angle), radius * Mathf.Cos(angle), 0);
                Gizmos.DrawLine(transform.position, transform.TransformPoint(point));
            }
        }
    }
}