using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class HealthCommandStream : MonoBehaviour
    {
        private static HealthCommandStream instance;
        public static HealthCommandStream Instance { 
            get => instance; 
            private set {
                if(instance != null && instance != value){
                    Destroy(value);
                } else {
                    instance = value;
                }
            } 
        }
        private CommandStream internalStream = new CommandStream();
        private Stack<IUndoable> undoStack = new Stack<IUndoable>();
        private IUndoable lastUndo;


        private void Awake() {
            Instance = this;
        }
        private void Start() {
        }
        public void QueueHealthCommand(ModifyHealthCommand modifyHealthCommand) {
            internalStream.QueueCommand(modifyHealthCommand);
        }
        private void Update() {
            if(internalStream.TryExecuteNext(out var executedCommand)) {
                if(executedCommand is IUndoable && executedCommand != lastUndo?.GetUndoCommand()) {
                    undoStack.Push((IUndoable)executedCommand);
                }
            } else if(executedCommand != null) {
                if (executedCommand is ModifyHealthCommand) {
                    IHealth receiver = ((ModifyHealthCommand)executedCommand).healthImplementer;
                    if(((ModifyHealthCommand)executedCommand).Magnitude > 0 && receiver.Health != receiver.MaxHealth) {
                        internalStream.QueueCommand(new ModifyHealthCommand(receiver.MaxHealth - receiver.Health, receiver));
                    } else if (((ModifyHealthCommand)executedCommand).Magnitude < 0 && receiver.Health != 0) {
                        internalStream.QueueCommand(new ModifyHealthCommand(-receiver.Health, receiver));
                    }
                    }
            }
        }
        public void UndoPrev() {
            if(undoStack.TryPop(out var nextUndo)) {
                lastUndo = nextUndo;
                internalStream.TryQueueUndoCommand(nextUndo);
            }
        }
    }
}
