using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The 'Tween command for scaling an object
    /// </summary>
    public class ScaleTweenCommand : AbstractTweenCommand
    {
        /// <summary>
        /// The factor to change the scale by
        /// </summary>
        private float scaleFactor;
        /// <summary>
        /// The target scale of the game object
        /// </summary>
        private Vector3 targetScale;
        /// <summary>
        /// The starting scale of the game object
        /// </summary>
        private Vector3 startScale;
        /// <summary>
        /// Constructs a ScaleTweenCommand
        /// </summary>
        public ScaleTweenCommand(GameObject gameObject, float scaleFactor, float timeSpan ) : base(timeSpan, gameObject) {
            this.scaleFactor = scaleFactor;
            tweenType = TweenType.scale;
        }

        /// <summary>
        /// The scale tween coroutine 
        /// sets the start time and scale
        /// sets the target scale based on the start scale
        /// </summary>
        protected override IEnumerator TweenCoroutine()
        {
            TweenCommandStream.Instance.RunningTweens[TweenType.scale] = true;
            startTime = Time.time;
            startScale = gameObject.transform.localScale;
            targetScale = startScale * scaleFactor;
            while (deltaTime <= timeSpan) {
                gameObject.transform.localScale =
                    Vector3.Lerp(startScale, targetScale, deltaTime / timeSpan);
                yield return null;
            }
            TweenCommandStream.Instance.RunningTweens[TweenType.scale] = false;
            Debug.Log($"{tweenType} tween coroutine finished");
        }
    }
}