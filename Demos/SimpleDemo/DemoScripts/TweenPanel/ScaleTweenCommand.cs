using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class ScaleTweenCommand : AbstractTweenCommand
    {
        private float scaleFactor;
        private Vector3 targetScale;
        private Vector3 startScale;
        public ScaleTweenCommand(GameObject gameObject, float scaleFactor, float timeSpan ) : base(timeSpan, gameObject) {
            this.scaleFactor = scaleFactor;
        }

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