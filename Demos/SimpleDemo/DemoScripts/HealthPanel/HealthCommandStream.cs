using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The CommandStream wrapper for the Health demo
    /// </summary>
    public class HealthCommandStream : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of HealthCommandStream
        /// </summary>
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
        /// <summary>
        /// The CommandStream this object wraps
        /// </summary>
        private CommandStream internalStream = new CommandStream();
        /// <summary>
        /// The stack of IUndoable commands
        /// </summary>
        private Stack<IUndoable> undoStack = new Stack<IUndoable>();
        /// <summary>
        /// The last IUndoable command that was undone, used to prevent its undo command from being added to undoStack
        /// </summary>
        private IUndoable lastUndo;
        /// <summary>
        /// queues the undo command the top IUndoable of the undoStack and sets lastUndo to that IUndoable 
        /// </summary>
        public void UndoPrev() {
            if(undoStack.TryPop(out var nextUndo)) {
                lastUndo = nextUndo;
                internalStream.TryQueueUndoCommand(nextUndo);
            }
        }

        /// <summary>
        /// Sets the singleton instance
        /// </summary>
        private void Awake() {
            Instance = this;
        }
        private void Start() {
        }
        /// <summary>
        /// Queues a ModifyHealthCommand into the internal stream
        /// </summary>
        /// <param name="modifyHealthCommand">The Command to queue</param>
        public void QueueHealthCommand(ModifyHealthCommand modifyHealthCommand) {
            internalStream.QueueCommand(modifyHealthCommand);
        }
        /// <summary>
        /// Attempts to execute the next command in the internalCommand stream's queue.
        /// If it succeeds and that wasn't the undo command of lastUndo, adds the next command to undoStack
        /// If it fails, queues a new command to modify health to precisely 0 or maxHealth as appropriate  
        /// </summary>
        private void Update() {
            var execCode = internalStream.TryExecuteNext(out var executedCommand);
            if( execCode == ExecuteCode.Success) {
                if(executedCommand is IUndoable && executedCommand != lastUndo?.GetUndoCommand()) {
                    undoStack.Push((IUndoable)executedCommand);
                }
            } else if(execCode == ExecuteCode.Failure) {
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
    }
}
