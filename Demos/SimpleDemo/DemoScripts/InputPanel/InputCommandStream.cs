using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public enum InputType {
        MoveUp,MoveDown,MoveLeft,MoveRight,Sprint,Fire,AltFire,Undo
    }

    public class InputCommandStream : MonoBehaviour {
        private static InputCommandStream instance;
        public Toggle activateDemo;
        public Player player;
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
            player = GetComponentInChildren<Player>();
            InitializeKeys();
        }
        private void Start() {
            activateDemo = GetComponentInChildren<Toggle>();
        }

        public Dictionary<InputType, KeyCode> InputKeybinds = new Dictionary<InputType, KeyCode> {
            { InputType.MoveUp, KeyCode.None },
            { InputType.MoveDown, KeyCode.None },
            { InputType.MoveLeft, KeyCode.None },
            { InputType.MoveRight, KeyCode.None },
            { InputType.Sprint, KeyCode.None },
            { InputType.Fire, KeyCode.None },
            { InputType.AltFire, KeyCode.None },
            { InputType.Undo, KeyCode.None }
        };

        private void InitializeKeys() {
            if (InputKeybinds[InputType.MoveUp] == KeyCode.None) { InputKeybinds[InputType.MoveUp] = KeyCode.W; }
            if (InputKeybinds[InputType.MoveDown] == KeyCode.None) { InputKeybinds[InputType.MoveDown]  = KeyCode.S; }
            if (InputKeybinds[InputType.MoveLeft]  == KeyCode.None) { InputKeybinds[InputType.MoveLeft]  = KeyCode.A; }
            if (InputKeybinds[InputType.MoveRight]  == KeyCode.None) { InputKeybinds[InputType.MoveRight]  = KeyCode.D; }
            if (InputKeybinds[InputType.Sprint]  == KeyCode.None) { InputKeybinds[InputType.Sprint]  = KeyCode.LeftShift; }
            if (InputKeybinds[InputType.Fire]  == KeyCode.None) { InputKeybinds[InputType.Fire]  = KeyCode.Mouse0; }
            if (InputKeybinds[InputType.AltFire]  == KeyCode.None) { InputKeybinds[InputType.AltFire]  = KeyCode.Mouse1; }
            if (InputKeybinds[InputType.Undo]  == KeyCode.None) { InputKeybinds[InputType.Undo]  = KeyCode.Backspace; }
        }

        private CommandStream internalStream = new CommandStream(1000000);
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
            if(internalStream.TryExecuteNext(out var topCommand) == ExecuteCode.Success) {
                if(topCommand is IUndoable && topCommand != prevUndo?.GetUndoCommand()) {
                    undoStack.Push((IUndoable)topCommand);
                }
            }
        }

    }
}