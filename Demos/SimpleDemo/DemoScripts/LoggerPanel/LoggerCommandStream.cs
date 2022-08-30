using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    
    public class LoggerCommandStream : MonoBehaviour
    {
        private static LoggerCommandStream instance;
        public static LoggerCommandStream Instance { 
            get => instance; 
            private set {
                if(instance != null && instance != value){
                    Destroy(value);
                } else {
                    instance = value;
                }
            } 
        }
        private CommandStream commandStream = new CommandStream();
        public void QueueCommand(LoggerCommand loggerCommand) {
            commandStream.QueueCommand(loggerCommand);
        }

        void Awake()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update() {
            if (commandStream.QueueCount > 0) {
                commandStream.TryExecuteNext();
            }
        }
    }
}