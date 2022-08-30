using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public abstract class AbstractTweenCommand : Command
    {
        protected float timeSpan;
        protected float startTime;
        protected float deltaTime { get => Time.time - startTime; }
        protected GameObject gameObject;

        protected AbstractTweenCommand(float timeSpan, GameObject gameObject)
        {
            this.timeSpan = timeSpan;
            this.gameObject = gameObject;
        }

        protected abstract IEnumerator TweenCoroutine();
        public override void Execute() {
            TweenCommandStream.Instance.StartCoroutine(TweenCoroutine());
            UnityEngine.Debug.Log("tween coroutine started");

        }
    }
}