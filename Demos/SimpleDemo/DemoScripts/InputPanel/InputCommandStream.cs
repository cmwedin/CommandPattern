using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    /// <summary>
    /// The different types of input that can be entered
    /// </summary>
    public enum InputType {
        MoveUp,MoveDown,MoveLeft,MoveRight,Sprint,Fire,AltFire,Undo
    }
    /// <summary>
    /// The wrapper object for the CommandStream handling player inputs
    /// </summary>
    public class InputCommandStream : MonoBehaviour {
        /// <summary>
        /// The singleton instance of this object
        /// </summary>
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
        /// <summary>
        /// the switch to turn the input demo on or off
        /// </summary>
        [HideInInspector] public Toggle activateDemoSwitch;
        /// <summary>
        /// the player object of the demo
        /// </summary>
        [HideInInspector] public Player player;

        /// <summary>
        /// Sets the singleton instance of the object, gets the reference to the player and demo toggle, and initializes all of the input keys
        /// </summary>
        private void Awake() {
            Instance = this;
            player = GetComponentInChildren<Player>();
            activateDemoSwitch = GetComponentInChildren<Toggle>();
            InitializeKeys();
        }

        /// <summary>
        /// The dictionary for the keycodes that correspond to each input
        /// </summary>
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
        /// <summary>
        /// Sets the input keycode to their initial values
        /// </summary>
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
        /// <summary>
        /// The internal CommandStream of the object, limits history depth to one million due to commands being created every frame
        /// </summary>
        private CommandStream internalStream = new CommandStream(1000000);
        /// <summary>
        /// The stack of movement inputs to undo
        /// </summary>
        private Stack<IUndoable> undoStack = new Stack<IUndoable>();
        /// <summary>
        /// The last IUndoable command that was undone
        /// </summary>
        private IUndoable prevUndo;
        /// <summary>
        /// Queues a command into the internal CommandStream
        /// </summary>
        /// <param name="command">The command to queue</param>
        public void QueueCommand(Command command) {
            internalStream.QueueCommand(command);
        }
        /// <summary>
        /// queues the undo command the top IUndoable of the undoStack and sets prevUndo to that IUndoable 
        /// </summary>
        public void Undo() {
            if(undoStack.TryPop(out var topUndo)) {
                prevUndo = topUndo;
                internalStream.TryQueueUndoCommand(topUndo);
            }
        }
        /// <summary>
        /// Executes the next command of the command queue and adds it to the undoStack if it wasn't the undo command of prevUndo
        /// </summary>
        private void Update() {
            if(internalStream.TryExecuteNext(out var topCommand) == ExecuteCode.Success) {
                if(topCommand is IUndoable && topCommand != prevUndo?.GetUndoCommand()) {
                    undoStack.Push((IUndoable)topCommand);
                }
            }
        }

    }
}