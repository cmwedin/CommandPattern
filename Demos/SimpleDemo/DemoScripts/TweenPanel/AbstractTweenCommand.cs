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

        protected AbstractTweenCommand(float timeSpan, GameObject tweenerObj)
        {
            this.timeSpan = timeSpan;
            this.gameObject = tweenerObj;
        }

        protected abstract IEnumerator TweenCoroutine();
    }
}