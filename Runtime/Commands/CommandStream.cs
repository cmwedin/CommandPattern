using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace SadSapphicGames.CommandPattern
{
    public class CommandStream {
        /// <summary>
        /// This is a Queue of the commands to be executed
        /// </summary>
        private Queue<Command> commandQueue = new Queue<Command>();
        /// <summary>
        /// This is a list of the executed command history
        /// </summary>
        private List<Command> commandHistory = new List<Command>();
        /// <summary>
        ///  Get the CommandStream's history of executed Commands.   
        /// </summary>
        /// <returns>The history of executed commands, null if history is not recorded. </returns>
        public ReadOnlyCollection<Command> GetCommandHistory() {
            if(HistoryDepth == 0){
                Debug.LogWarning("This CommandStream does no record its history");
                return null;
            } else {
                return commandHistory.AsReadOnly();
            }
        }


        private float historyDepth = 0;
        /// <summary>
        /// this is the maximum number of commands that will be recorded in the CommandHistory
        /// </summary>
        public float HistoryDepth { 
            get => historyDepth; 
            private set { 
                historyDepth = Mathf.Max(value,0); 
            } 
        }
        /// <summary>
        /// This is the number of Commands currently stored in the CommandStreams history.
        /// </summary>
        public int HistoryCount {
            get => commandHistory.Count;
        }

        /// <summary>
        /// Creates a new CommandStream
        /// </summary>
        /// <param name="_historyDepth"> 
        /// The depth to which previously executed commands will be recorded. Once this depth is reached the oldest commands will be forgotten first. 
        /// <remark> 
        /// To not record history, set to zero. To never forget executed commands, set to positive infinity.
        /// </remark> 
        /// </param>
        public CommandStream(float _historyDepth = Single.PositiveInfinity) {
            HistoryDepth = _historyDepth;
        }
        /// <summary>
        /// This adds a Command to the command Queue
        /// </summary>
        /// <param name="command"> The Command to be Queued </param>
        public void QueueCommand(Command command) {
            commandQueue.Enqueue(command);
        }
        /// <summary>
        /// Adds multiple Commands to the queue
        /// </summary>
        /// <param name="commands"> The commands to be queued </param>
        public void QueueCommands(IEnumerable<Command> commands) {
            var commandEnum = commands.GetEnumerator();
            while(commandEnum.MoveNext()) {
                QueueCommand(commandEnum.Current);
            }
        }
        /// <summary>
        /// This will remove all commands from the CommandStream's queue and replace it with a new empty queue
        /// </summary>
        /// <returns> The commands in the previous queue, in case this information is needed (for example to rearrange an requeue them).</returns>
        public List<Command> EmptyQueue(){
            var output = commandQueue.ToList();
            commandQueue = new Queue<Command>();
            return output;
        }
        private void RecordCommand(Command command) {
            if(historyDepth == 0) return; //? we should never be here if this is true but just in case
            commandHistory.Add(command);
            if(HistoryCount > historyDepth) {
                DropOldCommandHistory();
            }
        }
        private void DropOldCommandHistory() {
            commandHistory = commandHistory.Skip((int)(HistoryCount - HistoryDepth)).ToList();
        }
        /// <summary>
        /// Attempt to queue's the undo command of a Command object implementing IUndoable if that command exists in this CommandStream's history
        /// </summary>
        /// <param name="commandToUndo">The IUndoable Command to try and queue the undo-Command of</param>
        /// <returns>Wether the undo command was queued</returns>
        public bool TryQueueUndoCommand(IUndoable commandToUndo) {
            if(historyDepth == 0) {
                Debug.LogWarning("This CommandStream does not record its history, undoing commands requires a command history");
                return false;
            } if(commandHistory.Contains((Command)commandToUndo)) {
                QueueCommand(commandToUndo.GetUndoCommand());
                return true;
            } else if(commandQueue.Contains((Command)commandToUndo)) {
                Debug.LogWarning("The command you are trying to undo has not been executed yet but is in the queue");
            } else if(HistoryCount >= HistoryDepth) { //? should never be greater but better to catch all cases
                Debug.LogWarning("The command you are trying to undo is not in the CommandStream's history, but it may have been dropped");
            } else {
                Debug.LogWarning("The command you are trying to undo has never been executed");
            }
            return false;
        }
        /// <summary>
        /// Force the stream to queue's the undo command of a Command object implementing IUndoable regardless of whether the command is recorded in this CommandStream's history
        /// </summary>
        /// <remark>This is equivalent to passing the result of IUndoable.GetUndoCommand() into CommandStream.QueueCommand(Command command) directly</remark>
        /// <param name="commandToUndo">The IUndoable Command to queue the undo-Command of</param>
        public void ForceQueueUndoCommand(IUndoable commandToUndo) {
            QueueCommand(commandToUndo.GetUndoCommand());
        }

        /// <summary>
        /// Attempts to execute the next command in the queue, returns false if it is empty or the command is IFailable and would fail.
        /// </summary>
        /// <param name="nextCommand"> The command that was next in the queue, null if the queue was empty</param>
        /// <returns> False if the command queue is empty, or the next command would fail. True otherwise. </returns>
        public bool TryExecuteNext(out Command nextCommand) {
            if(!commandQueue.TryDequeue(out nextCommand)) {
                return false;
            }
            if(
                nextCommand is IFailable 
                && ((IFailable)nextCommand).WouldFail()
            ) {
                return false;
            }
            nextCommand.Execute();
            if(historyDepth > 0) {
                RecordCommand(nextCommand);
            }
            return true;
        }
        /// <summary>
        /// Attempts to execute the next command in the queue, returns false if it is empty or the command is IFailable and would fail.
        /// </summary>
        /// <returns> False if the command queue is empty, or the next command would fail. True otherwise. </returns>
        public bool TryExecuteNext() {
            return TryExecuteNext(out var empty);
        }
    }
}