using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class RotateTweenCommand : AbstractTweenCommand
    {
        Quaternion targetQuat;
        Quaternion startQuat;
        private float angle;
        public RotateTweenCommand(GameObject gameObject, float angle, float timeSpan) : base(timeSpan, gameObject) {
            this.angle = angle;
        }

        protected override IEnumerator TweenCoroutine()
        {
            TweenCommandStream.Instance.RunningTweens[TweenType.rotate] = true;
            startTime = Time.time;
            startQuat = gameObject.transform.rotation;
            targetQuat = startQuat * Quaternion.Euler(Vector3.forward * angle);
            while (deltaTime <= timeSpan) {
                gameObject.transform.rotation =
                    Quaternion.Lerp(startQuat, targetQuat, deltaTime / timeSpan);
                yield return null;
            }
            TweenCommandStream.Instance.RunningTweens[TweenType.rotate] = false;
        }
    }
}