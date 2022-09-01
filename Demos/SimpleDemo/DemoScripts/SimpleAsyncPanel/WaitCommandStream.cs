using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class WaitCommandStream : MonoBehaviour
    {
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
        private void Awake() {
            Instance = this;
        }

        private CommandStream commandStream = new CommandStream();
        public void QueueCommand(WaitForSecondsCommand command) {
            commandStream.QueueCommand(command);
        }
        public void CancelCommand(WaitForSecondsCommand command) {
            commandStream.CancelRunningCommandTask(command);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(commandStream.QueueCount > 0) {
                commandStream.TryExecuteNext();
            }
        }
    }
}