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
        AwaitingCompletion,
        /// <summary>
        /// Top command was an AsyncCommand but its task had already been started and hasn't been completed
        /// </summary>
        AlreadyRunning
        
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
        /// <summary>
        /// This event will be invoked if one of the tasks from an IAsyncCommand executed by this stream faults. Can be used to throw any exception's caused by tasks rather than storing them on the task object
        /// </summary>
        public event Action<Exception> OnTaskFaulted;
        /// <summary>
        /// What index in the commandHistory list is the oldest command stored in
        /// </summary>
        private int historyStartIndex = 0;
        /// <summary>
        /// Converts command history into a list who's oldest entry is at index 0, a linear time operation if historyStartIndex is not 0
        /// </summary>
        /// <returns>A list of the executed commands starting with the oldest command </returns>
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
        ///  Get the CommandStream's history of executed Commands, a linear time operation if HistoryCount has reached HistoryDepth   
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
        /// <summary>
        /// Gets the CancellationTokenSource of a running AsyncCommand's task
        /// </summary>
        /// <param name="task">The task to get the CTS of</param>
        /// <returns>The CTS of task if it is running, null if it is not</returns>
        public CancellationTokenSource GetRunningTaskCTS(Task task) {
            try {
                return runningCommandTasks[task];
            } catch (KeyNotFoundException) {
                Debug.Log($"The task {task.ToString()} is not running");
                return null;
            }
        }
        /// <summary>
        /// Gets the CancellationTokenSource of a running AsyncCommand's task
        /// </summary>
        /// <param name="task">The AsyncCommand to get the CTS of</param>
        /// <returns>The CTS of task if it is running, null if it is not</returns>
        public CancellationTokenSource GetRunningTaskCTS(IAsyncCommand asyncCommand) {
            return GetRunningTaskCTS(asyncCommand.CommandTask);
        }
        /// <summary>
        /// Cancels a task if that task is currently running
        /// </summary>
        /// <param name="task">The task to cancel</param>
        public void CancelRunningCommandTask(Task task){
                try {
                    runningCommandTasks[task].Cancel();
                } catch (KeyNotFoundException) {
                    Debug.Log($"The task {task.ToString()} is not running");
                }
        }
        /// <summary>
        /// Cancels the task of an IAsyncCommand if that task is currently running.
        /// </summary>
        /// <param name="asyncCommand">The IAsyncCommand to cancel the task off</param>
        public void CancelRunningCommandTask(IAsyncCommand asyncCommand){
            CancelRunningCommandTask(asyncCommand.CommandTask);
        }
        /// <summary>
        /// Get a dictionary of any faulted tasks and the exceptions that they threw
        /// </summary>
        /// <returns> A read only copy of the dictionary of faulted tasks and their exceptions </returns>
        public ReadOnlyDictionary<Task,Exception> GetFaultedCommandTasks() {
            return new ReadOnlyDictionary<Task,Exception>(faultedCommandTasks);
        }
        /// <summary>
        /// Gets commandQueue.Count == 0
        /// </summary>
        public bool QueueEmpty { get => commandQueue.Count == 0; }

        /// <summary>
        /// The maximum number of commands that will be recorded in the CommandStream's history
        /// </summary>
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
        /// <summary>
        /// Adds the argument to the commandHistory list, a constant time operation by virtue of moving the historyStartIndex once HistoryCount reaches HistoryDepth 
        /// </summary>
        /// <param name="command"> The command being recorded</param>
        /// <exception cref="Exception"> Thrown if the HistoryCount manages to exceed the HistoryDepth, this should never happen though </exception>
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
        /// <param name="undoable">The IUndoable Command to try and queue the undo-Command of</param>
        /// <returns>Wether the undo command was queued</returns>
        public bool TryQueueUndoCommand(IUndoable undoable) {
            if(historyDepth == 0) {
                Debug.LogWarning("This CommandStream does not record its history, use ForceQueueUndoCommand(IUndoable undoable) instead");
                return false;
            } if(commandHistory.Contains((ICommand)undoable)) {
                QueueCommand(undoable.GetUndoCommand());
                return true;
            } else if(commandQueue.Contains((ICommand)undoable)) {
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
        /// <param name="undoable">The IUndoable Command to queue the undo-Command of</param>
        public void ForceQueueUndoCommand(IUndoable undoable) {
            QueueCommand(undoable.GetUndoCommand());
        }
        /// <summary>
        /// Examine the next command in the commandQueue with out executing it
        /// </summary>
        /// <param name="nextCommand">The next command in the queue, null if empty</param>
        /// <returns>Wether there was a command in queue or not</returns>
        public bool TryPeekNext(out ICommand nextCommand) {
            return commandQueue.TryPeek(out nextCommand);
        }
        /// <summary>
        /// Removes the next command in the CommandStream's queue, if it has one, and adds it back to the end of the queue.
        /// </summary>
        public void RequeueNextCommand() {
            if(commandQueue.TryDequeue(out var nextCommand)) {
                commandQueue.Enqueue(nextCommand);
            }
        }
        /// <summary>
        /// Removes the next command from the queue without executing it
        /// </summary>
        public void SkipNextCommand(){
            commandQueue.TryDequeue(out var skippedCommand);
        }
        /// <summary>
        /// private method that handles executing a command
        /// </summary>
        /// <param name="command">The command that will be executed</param>
        /// <returns>The Execute code for the attempt to execute the argument </returns>
        private ExecuteCode TryExecuteCommand(ICommand command){
            if(
                command is IFailable asFailable
                && asFailable.WouldFail()
            ) {
                return ExecuteCode.Failure;
            }

            try {
                command.Execute();
            } catch (IrreversibleCompositeFailureException ex) {
                //? A composite failed halfway through execution and can't be reversed - let the wrapper attempt to handle it
                throw ex;
            } catch(ReversibleCompositeFailureException ex) {
                //? A composite failed halfway through execution and could be reversed - we can handle it (and already did in the composite)
                Debug.LogWarning(ex.Message);
                return ExecuteCode.CompositeFailure;
            } catch(AlreadyRunningException ex) {
                Debug.LogWarning(ex.Message);
                return ExecuteCode.AlreadyRunning;
            }
                catch(SystemException ex) {
                //? Some other exception happened that the invoker can't know how to handle - let the wrapper attempt to handle it
                throw ex;
            }
            
            if(historyDepth > 0) {
                RecordCommand(command);
            }

            //? At this point we should have successfully executed the command 
            if(
                command is IAsyncCommand asAsync
                && !asAsync.CommandTask.IsCompleted
            ) {
                Task asyncTask = asAsync.CommandTask;
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                asAsync.CancellationToken = cancellationTokenSource.Token;
                runningCommandTasks.Add(asyncTask, cancellationTokenSource);
                asAsync.OnAnyTaskEnd += () => {
                    cancellationTokenSource.Dispose();
                    asAsync.CancellationToken = CancellationToken.None;
                    runningCommandTasks.Remove(asyncTask);
                };
                asAsync.OnTaskFaulted += (ex) => {
                    //? bubbles the exception up to the wrapper if it wishes to subscribe to this and throw
                    this.OnTaskFaulted?.Invoke(ex);
                    //? otherwise warns the user a fault occurred and stores the fault data somewhere they can access
                    faultedCommandTasks.Add(asyncTask, ex);
                    Debug.LogWarning($"task of Command {asAsync.ToString()} faulted with the following exception");
                    Debug.LogWarning(ex);
                };
                return ExecuteCode.AwaitingCompletion;
            } else {
                return ExecuteCode.Success;
            }   
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
            return TryExecuteCommand(topCommand);
        }
        /// <summary>
        /// Bypass the command queue and immediately attempt to execute a command
        /// </summary>
        /// <param name="command">The command to immediately be executed</param>
        /// <returns>The ExecuteCode for the attempt to execute the command</returns>
        public ExecuteCode TryExecuteImmediate(ICommand command) {
            return TryExecuteCommand(command);
        }
        /// <summary>
        /// Bypass the command queue and immediately attempt to execute an IUndoable's undo command if the IUndoable is in the CommandStream's history
        /// </summary>
        /// <param name="undoable">the IUndoable to execute the undo command of</param>
        /// <returns>The ExecuteCode of the attempt to execute the undo command of the IUndoable, Execute.Failure if the IUndoable was not in the CommandStream's history </returns>
        public ExecuteCode TryUndoImmediate(IUndoable undoable) {
            if(historyDepth == 0) {
                Debug.LogWarning("This CommandStream does not record its history, use ForceTryUndoImmediate(IUndoable undoable) instead");
                return ExecuteCode.Failure;
            } if(commandHistory.Contains((ICommand)undoable)) {
                return TryExecuteCommand(undoable.GetUndoCommand());
            } else if(commandQueue.Contains((ICommand)undoable)) {
                Debug.LogWarning("The command you are trying to undo has not been executed yet but is in the queue");
            } else if(HistoryCount >= HistoryDepth) { //? should never be greater but better to catch all cases
                Debug.LogWarning("The command you are trying to undo is not in the CommandStream's history, but it may have been dropped");
            } else {
                Debug.LogWarning("The command you are trying to undo has never been executed");
            }
            return ExecuteCode.Failure;
        }
        /// <summary>
        /// Bypass the command queue and immediately attempt to execute an IUndoable's undo command, regardless of wether the IUndoable is in the CommandStream's history
        /// </summary>
        /// <param name="undoable">the IUndoable to execute the undo command of</param>
        /// <returns>The ExecuteCode of the attempt to execute the undo command of the IUndoable</returns>
        public ExecuteCode ForceTryUndoImmediate(IUndoable undoable) {
            return TryExecuteCommand(undoable.GetUndoCommand());
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