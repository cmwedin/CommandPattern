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
        [HideInInspector] public KeyCode altfireKeyCode = KeyCode.Mouse1;

        [HideInInspector] public KeyCode undoKeyCode = KeyCode.Backspace;
        [HideInInspector] public KeyCode sprintKey = KeyCode.LeftShift;



        private CommandStream internalStream = new CommandStream(100000000);
        private Stack<IUndoable> undoStack = new Stack<IUndoable>();
        private IUndoable prevUndo;
        public void QueueCommand(Command command) {
            internalStream.QueueCommand(command);
        }
        public void Undo() {
            if(undoStack.TryPop(out var topUndo)) {
                prevUndo = topUndo;
                internalStream.TryQueueUndoCommand(topUndo);
            }
        }

        private void Update() {
            if(internalStream.TryExecuteNext(out var topCommand)) {
                if(topCommand is IUndoable && topCommand != prevUndo?.GetUndoCommand()) {
                    undoStack.Push((IUndoable)topCommand);
                }
            }
        }

    }
}