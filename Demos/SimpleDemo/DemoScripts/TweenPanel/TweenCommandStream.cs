using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// enum for the different types of tween commands
    /// </summary>
    public enum TweenType {
        move, scale,rotate
    }
    /// <summary>
    /// The wrapper object of the CommandStream for the Tween Command Demo
    /// </summary>
    public class TweenCommandStream : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the object
        /// </summary>
        private static TweenCommandStream instance;
        public static TweenCommandStream Instance { 
            get => instance; 
            private set {
                if(instance != null && instance != value){
                    Destroy(value);
                } else {
                    instance = value;
                }
            } 
        }
        /// <summary>
        /// Dictionary for which types of tween currently have coroutines running
        /// </summary>
        public Dictionary<TweenType, bool> RunningTweens = new Dictionary<TweenType, bool>{
            {TweenType.move,false},
            {TweenType.rotate, false},
            {TweenType.scale,false}
        };
        /// <summary>
        /// Sets the singleton instance
        /// </summary>
        private void Awake() {
            Instance = this;
        }
        /// <summary>
        /// The objects internal CommandStream MoveTweenCommands
        /// </summary>
        CommandStream moveStream = new CommandStream();
        /// <summary>
        /// Queues a MoveTweenCommand into that 'tween type's CommandStream
        /// </summary>
        /// <param name="moveTweenCommand">The command to queue</param>
        public void QueueCommand(MoveTweenCommand moveTweenCommand) {
            moveStream.QueueCommand(moveTweenCommand);
        }
        /// <summary>
        /// The objects internal CommandStream ScaleTweenCommands
        /// </summary>
        CommandStream scaleStream = new CommandStream();
        /// <summary>
        /// Queues a ScaleTweenCommand into that 'tween type's CommandStream
        /// </summary>
        /// <param name="moveTweenCommand">The command to queue</param>
        public void QueueCommand(ScaleTweenCommand scaleTweenCommand) {
            scaleStream.QueueCommand(scaleTweenCommand);
        }
        /// <summary>
        /// The objects internal CommandStream RotateTweenCommands
        /// </summary>
        CommandStream rotateStream = new CommandStream();
        /// <summary>
        /// Queues a RotateTweenCommand into that 'tween type's CommandStream
        /// </summary>
        /// <param name="moveTweenCommand">The command to queue</param>
        public void QueueCommand(RotateTweenCommand rotateTweenCommand) {
            rotateStream.QueueCommand(rotateTweenCommand);
        }

        /// <summary>
        /// Executes a command from each CommandStream that isn't empty and doesn't have a coroutine currently running
        /// </summary>
        void Update()
        {
            if(moveStream.QueueCount > 0 && !RunningTweens[TweenType.move]) {
                moveStream.TryExecuteNext();
            }
            if(scaleStream.QueueCount > 0 && !RunningTweens[TweenType.scale]) {
                scaleStream.TryExecuteNext();
            }
            if(rotateStream.QueueCount > 0 && !RunningTweens[TweenType.rotate]) {
                rotateStream.TryExecuteNext();
            }
        }
    }
}