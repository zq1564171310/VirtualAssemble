/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.Utils;

    public class AnimationCollection : Singleton<AnimationCollection>
    {
        private IDictionary<string, Transform> animationList;
        public bool selfTest;
        public Transform[] targets;

        void Start()
        {
            animationList = new Dictionary<string, Transform>();
            foreach (Transform t in transform)
            {
                animationList.Add(t.gameObject.name, t);
                t.gameObject.SetActive(false);
            }

            if (selfTest)
            {
                StartCoroutine(TestAnimations());
            }
        }

        // To invoke an animation, call PlayAnimation with animation name and animation target, 
        // The animation target is a transform, with Z forward as the forward
        private IEnumerator TestAnimations()
        {
            foreach (var t in targets)
            {
                if (t == null)
                {
                    continue;
                }
                yield return new WaitForSeconds(3);

                PlayAnimation("十字螺丝刀旋入", t);

                yield return new WaitForSeconds(3);

                PlayAnimation("一字螺丝刀旋入", t);

                yield return new WaitForSeconds(3);

                PlayAnimation("M12内六角旋入", t);
            }
        }

        public void PlayAnimation(string name, Transform target)
        {
            Transform animationTransform;
            if (animationList.TryGetValue(name, out animationTransform))
            {
                animationTransform.position = target.position;
                animationTransform.rotation = target.rotation;
                animationTransform.gameObject.SetActive(true);
                StartCoroutine(TurnOffAnimation(animationTransform));
            }
            else
            {
                Debug.Log("Not about to find the animation of name " + name);
            }
        }

        private IEnumerator TurnOffAnimation(Transform t)
        {
            yield return new WaitForSeconds(1);
            t.gameObject.SetActive(false);
        }
    }
}