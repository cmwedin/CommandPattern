using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// This is the object that stores commands to be invoked and executes them when told to by the client. It has no knowledge of the implementation of commands beyond their interfaces.
    /// </summary>
    public class CommandStream {
        /// <summary>
        /// This is a Queue of the commands to be executed
        /// </summary>
        private Queue<Command> commandQueue = new Queue<Command>();
        /// <summary>
        /// This is a list of the executed command history
        /// </summary>
        private List<Command> commandHistory = new List<Command>();
        private int historyStartIndex = 0;
        private List<Command> UnwrapHistory() {
            List<Command> output = new List<Command>();
            int index = historyStartIndex;
            bool doneUnwrapping = false;
            while(!doneUnwrapping) {
                output.Add(commandHistory[index]);
                index++;
                if(index == historyStartIndex) {
                    doneUnwrapping = true;
                } else if (index == HistoryCount) {
                    index = 0;
                }
            }
            return output;
        }
        /// <summary>
        ///  Get the CommandStream's history of executed Commands.   
        /// </summary>
        /// <returns>The history of executed commands, null if history is not recorded. </returns>
        public ReadOnlyCollection<Command> GetCommandHistory() {
            if(HistoryDepth == 0){
                Debug.LogWarning("This CommandStream does not record its history, returning null");
                return null;
            }
            if(historyStartIndex == 0) {
                return commandHistory.AsReadOnly();
            } else {
                return UnwrapHistory().AsReadOnly();
            }
        }
        /// <summary>
        /// Get the queue of commands to be executed by the command stream.
        /// </summary>
        /// <returns> The queue of commands the commandStream will execute. </returns>
        public ReadOnlyCollection<Command> GetCommandQueue() {
            return commandQueue.ToList().AsReadOnly();
        }
        /// <summary>
        /// Gets commandQueue.Count == 0
        /// </summary>
        public bool QueueEmpty { get => commandQueue.Count == 0; }


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
        /// Gets the number of Commands currently recorded in the CommandStream's history.
        /// </summary>
        public int HistoryCount {
            get => commandHistory.Count;
        }
        /// <summary>
        /// Gets the number of Commands currently waiting in the CommandStream's queue.
        /// </summary>
        public int QueueCount {
            get => commandQueue.Count();
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
        /// This will remove all commands from the CommandStream's queue and replace it with a new empty queue.
        /// </summary>
        /// <remark> This can be useful to rearrange the commands in a queue. Simple preform the needed changes on the returned list and re-queue it </remark>
        /// <returns> The commands in the previous queue, in case this information is needed.</returns>
        public List<Command> DropQueue() {
            var output = commandQueue.ToList();
            commandQueue = new Queue<Command>();
            return output;
        }
        /// <summary>
        /// This will remove all commands from the CommandStream's history and replace it with a new empty list.
        /// </summary>
        /// <remark> This can be useful if you need the CommandStream to record all of its history but also need it to execute an extremely large number of commands without running out of memory. Even if every command is the same object a CommandStream will run out of memory at 2-3 hundred million commands in its queue or history. </remark>
        /// <returns> The commands in the previous history, in case this information is needed </returns>
        public ReadOnlyCollection<Command> DropHistory() {
            var output = GetCommandHistory();
            commandHistory = new List<Command>();
            historyStartIndex = 0;
            return output;
        }
        private void RecordCommand(Command command) {
            if(historyDepth == 0) return; //? we should never be here if this is true but just in case
            if (HistoryCount < HistoryDepth) {
                commandHistory.Add(command);
            } else if(HistoryCount == HistoryDepth) {
                commandHistory[historyStartIndex] = command;
                historyStartIndex++;
                if(historyStartIndex == HistoryCount) {
                    historyStartIndex = 0;
                }
            } else { //? HistoryCount > HistoryDepth
                throw new Exception("Recorded history has exceeded maximum history depth");
            }
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
            try {
                nextCommand.Execute();
            } catch (IrreversibleCompositeFailureException ex) {
                throw ex;
            } catch(ReversibleCompositeCommandException ex) {
                Debug.LogWarning(ex.Message);
                return false;
            }
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
        /// <summary>
        /// Execute's Commands from the CommandStream queue until it is empty. Be warned this will not give any indication of commands failing.
        /// </summary>
        public void ExecuteFullQueue() {
            Command prevCommand = new NullCommand();
            while (prevCommand != null) {
                while(TryExecuteNext(out prevCommand)) {}
            }
        }
        /// <summary>
        /// Executes Commands from the CommandStream's queue until it is empty. Returns the a list of any Commands that failed as an out parameter
        /// </summary>
        /// <param name="failedCommands"> A list of any Commands in the Queue that failed to execute </param>
        public void ExecuteFullQueue(out List<Command> failedCommands) {
            Command prevCommand = new NullCommand();
            failedCommands = new List<Command>();
            while (prevCommand != null) {
                while(TryExecuteNext(out prevCommand)) {}
                if(prevCommand != null) {
                    failedCommands.Add(prevCommand);
                }
            }
        }
    }
}