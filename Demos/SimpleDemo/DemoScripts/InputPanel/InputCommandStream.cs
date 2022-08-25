using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public enum InputType {
        MoveUp,MoveDown,MoveLeft,MoveRight,Sprint,Fire,AltFire,Undo
    }

    public class InputCommandStream : MonoBehaviour {
        private static InputCommandStream instance;
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
        // [HideInInspector] public KeyCode upKeyCode;
        // [HideInInspector] public KeyCode downKeyCode;
        // [HideInInspector] public KeyCode leftKeyCode;
        // [HideInInspector] public KeyCode rightKeyCode;
        // [HideInInspector] public KeyCode fireKeyCode;
        // [HideInInspector] public KeyCode altFireKeyCode;
        // [HideInInspector] public KeyCode undoKeyCode;
        // [HideInInspector] public KeyCode sprintKey;
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
        // public async Task RebindKey(KeyCode currentKey) {
        //     if (currentKey == upKeyCode) {
        //         KeyCode newkey = await GetNewKey();
        //         if (ValidateKeySelection(newkey)) { upKeyCode = newkey; }
        //     }
        //     if (currentKey == downKeyCode) {
        //         KeyCode newkey = await GetNewKey();
        //         if (ValidateKeySelection(newkey)) { downKeyCode = newkey; }
        //     }
        //     if (currentKey == leftKeyCode) {
        //         KeyCode newkey = await GetNewKey();
        //         if (ValidateKeySelection(newkey)) { leftKeyCode = newkey; }
        //     }
        //     if (currentKey == rightKeyCode) {
        //         KeyCode newkey = await GetNewKey();
        //         if (ValidateKeySelection(newkey)) { rightKeyCode = newkey; }
        //     }
        //     if (currentKey == fireKeyCode) {
        //         KeyCode newkey = await GetNewKey();
        //         if (ValidateKeySelection(newkey)) { fireKeyCode = newkey; }
        //     }
        //     if (currentKey == altFireKeyCode) {
        //         KeyCode newkey = await GetNewKey();
        //         if (ValidateKeySelection(newkey)) { altFireKeyCode = newkey; }
        //     }
        //     if (currentKey == undoKeyCode) {
        //         KeyCode newkey = await GetNewKey();
        //         if (ValidateKeySelection(newkey)) { undoKeyCode = newkey; }
        //     }
        // }

        private async Task<KeyCode> GetNewKey() {
            KeyCode key = KeyCode.None;
            bool keySelected = false;
            KeySelector selector = gameObject.AddComponent<KeySelector>();
            selector.KeySelected += ((keyCode) => {
                key = keyCode;
                keySelected = true;
            });
            while(!keySelected) {
                await Task.Delay(1);
            }
            Destroy(selector);
            return key;
        }

        // private bool ValidateKeySelection(KeyCode keyCode) {
        //     return !(
        //         upKeyCode == keyCode
        //         || downKeyCode == keyCode
        //         || leftKeyCode == keyCode
        //         || rightKeyCode == keyCode
        //         || fireKeyCode == keyCode
        //         || altFireKeyCode == keyCode
        //         || undoKeyCode == keyCode
        //         || keyCode == KeyCode.None);
        // }


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