using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
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

        [HideInInspector] public KeyCode upKeyCode;
        [HideInInspector] public KeyCode downKeyCode;
        [HideInInspector] public KeyCode leftKeyCode;
        [HideInInspector] public KeyCode rightKeyCode;
        [HideInInspector] public KeyCode fireKeyCode;
        [HideInInspector] public KeyCode altFireKeyCode;
        [HideInInspector] public KeyCode undoKeyCode;
        [HideInInspector] public KeyCode sprintKey;
        private void InitializeKeys() {
            if (upKeyCode == KeyCode.None) { upKeyCode = KeyCode.W; }
            if (downKeyCode == KeyCode.None) { downKeyCode = KeyCode.S; }
            if (leftKeyCode == KeyCode.None) { leftKeyCode = KeyCode.A; }
            if (rightKeyCode == KeyCode.None) { rightKeyCode = KeyCode.D; }
            if (fireKeyCode == KeyCode.None) { fireKeyCode = KeyCode.Mouse0; }
            if (altFireKeyCode == KeyCode.None) { altFireKeyCode = KeyCode.Mouse1; }
            if (undoKeyCode == KeyCode.None) { undoKeyCode = KeyCode.Backspace; }
        }
        public async Task RebindKey(KeyCode currentKey) {
            if (currentKey == upKeyCode) {
                KeyCode newkey = await GetNewKey();
                if (ValidateKeySelection(newkey)) { upKeyCode = newkey; }
            }
            if (currentKey == downKeyCode) {
                KeyCode newkey = await GetNewKey();
                if (ValidateKeySelection(newkey)) { downKeyCode = newkey; }
            }
            if (currentKey == leftKeyCode) {
                KeyCode newkey = await GetNewKey();
                if (ValidateKeySelection(newkey)) { leftKeyCode = newkey; }
            }
            if (currentKey == rightKeyCode) {
                KeyCode newkey = await GetNewKey();
                if (ValidateKeySelection(newkey)) { rightKeyCode = newkey; }
            }
            if (currentKey == fireKeyCode) {
                KeyCode newkey = await GetNewKey();
                if (ValidateKeySelection(newkey)) { fireKeyCode = newkey; }
            }
            if (currentKey == altFireKeyCode) {
                KeyCode newkey = await GetNewKey();
                if (ValidateKeySelection(newkey)) { altFireKeyCode = newkey; }
            }
            if (currentKey == undoKeyCode) {
                KeyCode newkey = await GetNewKey();
                if (ValidateKeySelection(newkey)) { undoKeyCode = newkey; }
            }
        }

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

        private bool ValidateKeySelection(KeyCode keyCode) {
            return !(
                upKeyCode == keyCode
                || downKeyCode == keyCode
                || leftKeyCode == keyCode
                || rightKeyCode == keyCode
                || fireKeyCode == keyCode
                || altFireKeyCode == keyCode
                || undoKeyCode == keyCode
                || keyCode == KeyCode.None);
        }


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