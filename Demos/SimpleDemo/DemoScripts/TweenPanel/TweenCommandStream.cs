using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public enum TweenType {
        move, scale,rotate
    }
    public class TweenCommandStream : MonoBehaviour
    {
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
        public Dictionary<TweenType, bool> RunningTweens = new Dictionary<TweenType, bool>{
            {TweenType.move,false},
            {TweenType.rotate, false},
            {TweenType.scale,false}
        };

        public float GetTime() {
            return Time.time;
        }
        private void Awake() {
            Instance = this;
        }

        CommandStream moveStream = new CommandStream();
        public void QueueCommand(MoveTweenCommand moveTweenCommand) {
            moveStream.QueueCommand(moveTweenCommand);
        }

        // Update is called once per frame
        void Update()
        {
            if(moveStream.QueueCount > 0 && !RunningTweens[TweenType.move]) {
                moveStream.TryExecuteNext();
            }
        }
    }
}