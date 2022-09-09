using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The 'Tween command for rotating an object 
    /// </summary>
    public class RotateTweenCommand : AbstractTweenCommand {
        /// <summary>
        /// the target euler angles of the rotation quaternion
        /// </summary>
        Vector3 targetEulers;
        /// <summary>
        /// the starting euler angles of the rotation quaternion
        /// </summary>
        Vector3 startEulers;
        /// <summary>
        /// the angle to rotate the object by
        /// </summary>
        private float angle;
        /// <summary>
        /// Constructs the RotateTweenCommand object
        /// </summary>
        public RotateTweenCommand(GameObject gameObject, float angle, float timeSpan) : base(timeSpan, gameObject) {
            this.angle = angle;
            tweenType = TweenType.rotate;
        }

        /// <summary>
        /// The rotate tween coroutine 
        /// sets the start time and euler angles
        /// sets the target angle based on the start angle
        /// </summary>
        protected override IEnumerator TweenCoroutine()
        {
            TweenCommandStream.Instance.RunningTweens[TweenType.rotate] = true;
            startTime = Time.time;
            startEulers = gameObject.transform.rotation.eulerAngles;
            targetEulers = startEulers + Vector3.forward*angle;
            while (deltaTime <= timeSpan) {
                gameObject.transform.rotation = Quaternion.Euler(
                    Vector3.Lerp(startEulers, targetEulers, deltaTime / timeSpan)
                );
                yield return null;
            }
            TweenCommandStream.Instance.RunningTweens[TweenType.rotate] = false;
        }
    }
}