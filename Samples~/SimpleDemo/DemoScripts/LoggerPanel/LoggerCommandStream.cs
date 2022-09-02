using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The CommandStream wrapper object of the logger command demo
    /// </summary>
    public class LoggerCommandStream : MonoBehaviour
    {   
        /// <summary>
        /// The singleton instance of this object
        /// </summary>
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
        /// <summary>
        /// The internal CommandStream
        /// </summary>
        private CommandStream commandStream = new CommandStream();
        /// <summary>
        /// Queues a LoggerCommand into the internal CommandStream
        /// </summary>
        /// <param name="loggerCommand">The LoggerCommand to queue</param>
        public void QueueCommand(LoggerCommand loggerCommand) {
            commandStream.QueueCommand(loggerCommand);
        }
        /// <summary>
        /// Sets the singleton instance of this object
        /// </summary>
        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Executes the next command of the command stream if it isn't empty
        /// </summary>
        void Update() {
            if (commandStream.QueueCount > 0) {
                commandStream.TryExecuteNext();
            }
        }
    }
}