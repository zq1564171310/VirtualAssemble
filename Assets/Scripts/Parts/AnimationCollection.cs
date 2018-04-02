/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// This is the manager for the animations. We use the animation object from 
/// the children of this gameobject and use the name to identify.
/// </summary>

namespace WyzLink.Parts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WyzLink.LogicManager;
    using WyzLink.Utils;

    public class AnimationCollection : Singleton<AnimationCollection>
    {
        public class AnimationPlay : CustomYieldInstruction
        {
            private bool isDone = false;
            public void SetIsDone()
            {
                this.isDone = true;
            }
            public override bool keepWaiting
            {
                get
                {
                    return !isDone;
                }
            }
        }
        private IDictionary<string, Transform> animationList;

        void Start()
        {
            this.animationList = LoadAnimationsFromChildren();
        }

        private IDictionary<string, Transform> LoadAnimationsFromChildren()
        {
            var animationList = new Dictionary<string, Transform>();
            foreach (Transform t in transform)
            {
                animationList.Add(t.gameObject.name, t);
                t.gameObject.SetActive(false);
            }
            return animationList;
        }

        /// <summary>
        /// This function will invoke a animation to play in the specific position and rotation
        /// </summary>
        /// <param name="name">The name of the animation, which is the name of the gameObject of the child of AnimationCollection</param>
        /// <param name="target">The position and rotation where the animation will happen</param>
        public AnimationPlay PlayAnimation(string name, Transform target)
        {
            var play = new AnimationPlay();
            Transform animationTransform;
            if (animationList.TryGetValue(name, out animationTransform))
            {
                animationTransform.position = target.position;
                animationTransform.rotation = target.rotation;
                animationTransform.gameObject.SetActive(true);
                StartCoroutine(TurnOffAnimation(animationTransform, play));
            }
            else
            {
                Debug.Log("Not about to find the animation of name " + name);
                play.SetIsDone();
            }
            return play;
        }

        private IEnumerator TurnOffAnimation(Transform t, AnimationPlay play)
        {
            var animator = t.GetComponent<Animator>();

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
            {
                yield return new WaitForSeconds(1.3f);
            }
            if (UI.EntryMode.GetAssembleModel() == AssembleModel.StudyModel)
            {
                AssembleManagerStudyModel.Instance.Recovery();
            }
            t.gameObject.SetActive(false);

            play.SetIsDone();

        }
    }
}