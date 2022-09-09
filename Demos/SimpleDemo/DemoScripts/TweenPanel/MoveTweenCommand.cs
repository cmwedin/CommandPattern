using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The 'Tween command for moving an object between two points
    /// </summary>
    public class MoveTweenCommand : AbstractTweenCommand
    {
        /// <summary>
        /// the target point of the object
        /// </summary>
        private Vector3 target;
        /// <summary>
        /// the start point of the object
        /// </summary>
        private Vector3 start;

        /// <summary>
        /// Constructs the MoveTweenCommand object
        /// </summary>
        public MoveTweenCommand(GameObject gameObject, Vector3 target, float timeSpan) : base(timeSpan,gameObject)
        {
            this.target = target;
            tweenType = TweenType.move;
        }

        /// <summary>
        /// The MoveTweenCommands Coroutine
        /// sets the startTime and start position 
        /// Lerps the game object position between the start position nad the target position
        /// </summary>
        protected override IEnumerator TweenCoroutine()
        {
            TweenCommandStream.Instance.RunningTweens[TweenType.move] = true;
            startTime = Time.time;
            start = gameObject.transform.position;

            while(deltaTime <= timeSpan) {
                gameObject.transform.position = Vector3.Lerp(start, target, deltaTime / timeSpan);
                yield return null;
            }
            TweenCommandStream.Instance.RunningTweens[TweenType.move] = false;
            Debug.Log($"{tweenType} tween coroutine finished");
        }
    }
}