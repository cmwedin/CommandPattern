using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The base class of the differet 'Tween commands
    /// </summary>
    public abstract class AbstractTweenCommand : Command
    {
        /// <summary>
        /// The length of time over which the tween command will run
        /// </summary>
        protected float timeSpan;
        /// <summary>
        /// the type of tween command that this command is
        /// </summary>
        protected TweenType tweenType;
        /// <summary>
        /// The time that the tween command starts
        /// </summary>
        protected float startTime;
        /// <summary>
        /// A property for getting the difference between the current time and the start time of the tween command
        /// </summary>
        protected float deltaTime { get => Time.time - startTime; }
        /// <summary>
        /// the game object being tween-ed
        /// </summary>
        protected GameObject gameObject;
        /// <summary>
        /// The constructor for the base type of tween commands
        /// </summary>
        protected AbstractTweenCommand(float timeSpan, GameObject gameObject)
        {
            this.timeSpan = timeSpan;
            this.gameObject = gameObject;
        }

        /// <summary>
        /// The coroutine the tween command runs
        /// </summary>
        protected abstract IEnumerator TweenCoroutine();

        /// <summary>
        /// Starts the tween coroutine
        /// </summary>
        public override void Execute() {
            TweenCommandStream.Instance.StartCoroutine(TweenCoroutine());
            UnityEngine.Debug.Log($"{tweenType}-tween coroutine started");

        }
    }
}