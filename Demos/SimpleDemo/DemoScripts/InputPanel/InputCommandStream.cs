using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public class InputCommandStream : MonoBehaviour {
        private static InputCommandStream instance;
        public static InputCommandStream Instance { 
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

        [HideInInspector] public KeyCode upKeyCode = KeyCode.W;
        [HideInInspector] public KeyCode downKeyCode = KeyCode.S;
        [HideInInspector] public KeyCode leftKeyCode = KeyCode.A;
        [HideInInspector] public KeyCode rightKeyCode = KeyCode.D;
        [HideInInspector] public KeyCode fireKeyCode = KeyCode.Mouse0;

        private CommandStream internalStream = new CommandStream(100000000);
        public void QueueCommand(Command command) {
            internalStream.QueueCommand(command);
        }

        private void Update() {
            if(internalStream.TryExecuteNext(out var topCommand)){}
        }

    }
}