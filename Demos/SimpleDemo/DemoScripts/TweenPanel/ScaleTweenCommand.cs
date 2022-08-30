using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class ScaleTweenCommand : AbstractTweenCommand
    {
        private Vector3 targetScale;
        private Vector3 startScale;
        public ScaleTweenCommand(GameObject gameObject, Vector3 targetScale, float timeSpan ) : base(timeSpan, gameObject) {
            this.targetScale = targetScale;
        }

        protected override IEnumerator TweenCoroutine()
        {
            TweenCommandStream.Instance.RunningTweens[TweenType.scale] = true;
            startTime = Time.time;
            startScale = gameObject.transform.localScale;
            while (deltaTime <= timeSpan) {
                gameObject.transform.localScale =
                    Vector3.Lerp(startScale, targetScale, deltaTime / timeSpan);
                yield return null;
            }
            TweenCommandStream.Instance.RunningTweens[TweenType.scale] = false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}