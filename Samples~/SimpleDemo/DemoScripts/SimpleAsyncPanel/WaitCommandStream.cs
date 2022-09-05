using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// Wrapper object for the CommandStream of the simple async demo
    /// </summary>
    public class WaitCommandStream : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of this object
        /// </summary>
        private static WaitCommandStream instance;
        public static WaitCommandStream Instance { 
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
        /// Sets the singleton instance
        /// </summary>
        private void Awake() {
            Instance = this;
        }
        /// <summary>
        /// the internal CommandStream
        /// </summary>
        private CommandStream commandStream = new CommandStream();
        /// <summary>
        /// Queues a WaitForSecondsCommand
        /// </summary>
        /// <param name="command">the command to queue</param>
        public void QueueCommand(WaitForSecondsCommand command) {
            commandStream.QueueCommand(command);
        }
        /// <summary>
        /// Cancels a WaitForSecondsCommand
        /// </summary>
        /// <param name="command">The command to cancel</param>
        public void CancelCommand(WaitForSecondsCommand command) {
            commandStream.CancelRunningCommandTask(command);
        }

        /// <summary>
        /// Executes the next command of the CommandStream if the queue isn't empty  and logs its execute code as a demonstration of how async commands work
        /// </summary>
        void Update()
        {
            if(commandStream.QueueCount > 0) {
                var execCode = commandStream.TryExecuteNext();
                Debug.Log($"TryExecuteNext returned with code {execCode}");
            }
        }
    }
}