using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// The possible return values of CommandStream.TryExecuteNext()
    /// </summary>
    public enum ExecuteCode
    {
        /// <summary>
        /// Top Command executed successfully
        /// </summary>
        Success,
        /// <summary>
        /// Top Command would fail
        /// </summary>
        Failure,
        /// <summary>
        /// Command queue was empty
        /// </summary>
        QueueEmpty,
        /// <summary>
        /// Top Command was a CompositeCommand that did not indicate it would fail but failed partway through execution - was reversible
        /// </summary>
        CompositeFailure,
        /// <summary>
        /// Top command was an AsyncCommand that is awaiting completion
        /// </summary>
        AwaitingCompletion
    }
    /// <summary>
    /// This is the object that stores commands to be invoked and executes them when told to by the client. It has no knowledge of the implementation of commands beyond their interfaces.
    /// </summary>
    public class CommandStream {
        /// <summary>
        /// This is a Queue of the commands to be executed
        /// </summary>
        private Queue<ICommand> commandQueue = new Queue<ICommand>();
        /// <summary>
        /// This is a list of the executed command history
        /// </summary>
        private List<ICommand> commandHistory = new List<ICommand>();
        /// <summary>
        /// This is a list of the asynchronous tasks currently being run by AsyncCommands this CommandStream has executed.
        /// </summary>
        private Dictionary<Task,CancellationTokenSource> runningCommandTasks = new Dictionary<Task, CancellationTokenSource>();
        private Dictionary<Task,Exception> faultedCommandTasks = new Dictionary<Task, Exception>();
        public event Action<Exception> OnTaskFaulted;

        private int historyStartIndex = 0;
        private List<ICommand> UnwrapHistory() {
            List<ICommand> output = new List<ICommand>();
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
        public ReadOnlyCollection<ICommand> GetCommandHistory() {
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
        public ReadOnlyCollection<ICommand> GetCommandQueue() {
            return commandQueue.ToList().AsReadOnly();
        }
        /// <summary>
        /// Get the Tasks being being run by AsyncCommands executed by this CommandStream
        /// </summary>
        /// <returns>The tasks that are currently being run</returns>
        public ReadOnlyCollection<Task> GetRunningCommandTasks() { 
            return runningCommandTasks.Keys.ToList().AsReadOnly(); 
        }
        public void CancelRunningCommandTask(Task task){
            runningCommandTasks[task].Cancel();
        }
        public void CancelRunningCommandTask(IAsyncCommand asyncCommand){
            foreach (var task in runningCommandTasks.Keys)
            {
                if(runningCommandTasks[task] == asyncCommand.CancellationTokenSource) {
                    runningCommandTasks[task].Cancel();
                }
            };
        }

        public ReadOnlyDictionary<Task,Exception> GetFaultedCommandTasks() {
            return new ReadOnlyDictionary<Task,Exception>(faultedCommandTasks);
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
        public void QueueCommand(ICommand command) {
            commandQueue.Enqueue(command);
        }
        /// <summary>
        /// Adds multiple Commands to the queue
        /// </summary>
        /// <param name="commands"> The commands to be queued </param>
        public void QueueCommands(IEnumerable<ICommand> commands) {
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
        public List<ICommand> DropQueue() {
            var output = commandQueue.ToList();
            commandQueue = new Queue<ICommand>();
            return output;
        }
        /// <summary>
        /// This will remove all commands from the CommandStream's history and replace it with a new empty list.
        /// </summary>
        /// <remark> This can be useful if you need the CommandStream to record all of its history but also need it to execute an extremely large number of commands without running out of memory. Even if every command is the same object a CommandStream will run out of memory at 2-3 hundred million commands in its queue or history. </remark>
        /// <returns> The commands in the previous history, in case this information is needed </returns>
        public ReadOnlyCollection<ICommand> DropHistory() {
            var output = GetCommandHistory();
            commandHistory = new List<ICommand>();
            historyStartIndex = 0;
            return output;
        }
        private void RecordCommand(ICommand command) {
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
            } if(commandHistory.Contains((ICommand)commandToUndo)) {
                QueueCommand(commandToUndo.GetUndoCommand());
                return true;
            } else if(commandQueue.Contains((ICommand)commandToUndo)) {
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
        /// Attempts to execute the next command in the queue, returns an enum indicating if it was able to or not or if the queue was empty or the command is async and awaiting completion.
        /// </summary>
        /// <param name="topCommand"> The command that was next in the queue, null if the queue was empty</param>
        /// <returns> An ExecuteCode enum value indicating what happened when attempting to execute the next command </returns>
        public ExecuteCode TryExecuteNext(out ICommand topCommand) {
            if(!commandQueue.TryDequeue(out topCommand)) {
                return ExecuteCode.QueueEmpty;
            }
            if(
                topCommand is IFailable asFailable
                && asFailable.WouldFail()
            ) {
                return ExecuteCode.Failure;
            }

            try {
                topCommand.Execute();
            } catch (IrreversibleCompositeFailureException ex) {
                //? A composite failed halfway through execution and can't be reversed - let the wrapper attempt to handle it
                throw ex;
            } catch(ReversibleCompositeFailureException ex) {
                //? A composite failed halfway through execution and could be reversed - we can handle it (and already did in the composite)
                Debug.LogWarning(ex.Message);
                return ExecuteCode.CompositeFailure;
            } catch(SystemException ex) {
                //? Some other exception happened that the invoker can't know how to handle - let the wrapper attempt to handle it
                throw ex;
            }
            
            if(historyDepth > 0) {
                RecordCommand(topCommand);
            }

            //? At this point we should have successfully executed the command 
            if(
                topCommand is IAsyncCommand asAsync
                && !asAsync.CommandTask.IsCompleted
            ) {
                Task asyncTask = asAsync.CommandTask;
                runningCommandTasks.Add(asyncTask,asAsync.CancellationTokenSource);
                asAsync.OnTaskCompleted += () => { runningCommandTasks.Remove(asyncTask); };
                asAsync.OnTaskCanceled += () => { runningCommandTasks.Remove(asyncTask); };
                asAsync.OnTaskFaulted += (ex) => {
                    this.OnTaskFaulted?.Invoke(ex);
                    runningCommandTasks.Remove(asyncTask);
                    faultedCommandTasks.Add(asyncTask, ex);
                    Debug.LogWarning($"task of Command {asAsync.ToString()} faulted with the following exception");
                    Debug.LogWarning(ex);
                };

                //TODO add event for async command failure 
                return ExecuteCode.AwaitingCompletion;
            } else {
                return ExecuteCode.Success;
            }
        }
        /// <summary>
        /// Attempts to execute the next command in the queue, returns false if it is empty or the command is IFailable and would fail.
        /// </summary>
        /// <returns> False if the command queue is empty, or the next command would fail. True otherwise. </returns>
        public ExecuteCode TryExecuteNext() {
            return TryExecuteNext(out var empty);
        }
        /// <summary>
        /// Execute's Commands from the CommandStream queue until it is empty. Be warned this will not give any indication of commands failing.
        /// </summary>
        public void ExecuteFullQueue() {
            ICommand prevCommand = new NullCommand();
            while (prevCommand != null) {
                while(TryExecuteNext(out prevCommand) != ExecuteCode.QueueEmpty) {}
            }
        }
        /// <summary>
        /// Executes Commands from the CommandStream's queue until it is empty. Returns the a list of any Commands that failed as an out parameter
        /// </summary>
        /// <param name="failedCommands"> A list of any Commands in the Queue that failed to execute </param>
        public void ExecuteFullQueue(out List<ICommand> failedCommands) {
            ICommand topCommand;
            failedCommands = new List<ICommand>();
            ExecuteCode executeCode = TryExecuteNext(out topCommand);
            while(executeCode != ExecuteCode.QueueEmpty) {
                if(executeCode == ExecuteCode.Failure || executeCode == ExecuteCode.CompositeFailure) {
                    failedCommands.Add(topCommand);
                } else if(executeCode == ExecuteCode.AwaitingCompletion) {
                    //TODO add event for async command failure 
                }
                executeCode = TryExecuteNext(out topCommand);
            }
        }
    }
}