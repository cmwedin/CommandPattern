using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class MoveTweenCommand : AbstractTweenCommand
    {
        private Vector3 target;
        private Vector3 start;

        public MoveTweenCommand(GameObject gameObject, Vector3 target, float timeSpan) : base(timeSpan,gameObject)
        {
            this.target = target;
        }

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
        }
    }
}