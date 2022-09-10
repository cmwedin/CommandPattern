using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;



namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// A singleton manager for a single-stream, out of the box implementation of the Command pattern. 
    /// Once you understand how the package works it is highly recommended  create your own CommandStream wrapper tailored to the needs of your project. 
    /// Executes the next command in the CommandStream every frame.
    /// </summary>
    public class SingletonCommandManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of the CommandManger.
        /// </summary>
        public static SingletonCommandManager Instance { get; private set; }
        /// <summary>
        /// The value that will be used in the internal CommandStream's constructor, set to negative to record all history
        /// </summary>
        [SerializeField, Tooltip("The value that will be used in the internal CommandStream's constructor for its history depth, set to negative to record all history")]
        public int maximumHistoryDepth = -1;
        /// <summary>
        /// The internal CommandStream this object wraps
        /// </summary>
        private CommandStream commandStream;
        /// <summary>
        /// Can be used to stop the manager from executing commands
        /// </summary>
        private bool freezeCommandExecution = false;
        /// <summary>
        /// Invoked when the inner CommandStream invokes its OnTaskFaulted event
        /// </summary>
        public event Action<Exception> OnTaskFaulted;
        /// <summary>
        /// Turns command execution off if its on and on if its off
        /// </summary>
        public void ToggleCommandExecution()
        {
            freezeCommandExecution = !freezeCommandExecution;
        }
        /// <summary>
        /// Turns command execution on or off
        /// </summary>
        /// <param name="onoff">if false stops the execution of commands, if true enables it</param>
        public void ToggleCommandExecution(bool onoff)
        {
            freezeCommandExecution = !onoff;
        }
        /// <summary>
        /// Get the underlying CommandStream's history
        /// </summary>
        /// <returns> A ReadOnlyCollection of all the commands executed by the CommandManager's CommandStream </returns>
        public ReadOnlyCollection<ICommand> GetCommandHistory() {
            return commandStream.GetCommandHistory();
        }
        /// <summary>
        /// Get the currently uncompleted tasks from executed AsyncCommands
        /// </summary>
        /// <returns>A ReadOnlyCollection of uncompleted tasks from executed AsyncCommands </returns>
        public ReadOnlyCollection<Task> GetRunningCommandTasks() {
            return commandStream.GetRunningCommandTasks();
        }
        /// <summary>
        /// Gets the CancellationTokenSource of a running AsyncCommand's task
        /// </summary>
        /// <param name="task">The task to get the CTS of</param>
        /// <returns>The CTS of task if it is running, null if it is not</returns>
        public CancellationTokenSource GetRunningTaskCTS(Task task){
            return commandStream.GetRunningTaskCTS(task);
        }
        /// <summary>
        /// Gets the CancellationTokenSource of a running AsyncCommand's task
        /// </summary>
        /// <param name="task">The AsyncCommand to get the CTS of</param>
        /// <returns>The CTS of task if it is running, null if it is not</returns>
        public CancellationTokenSource GetRunningTaskCTS(IAsyncCommand asyncCommand){
            return commandStream.GetRunningTaskCTS(asyncCommand);
        }
        /// <summary>
        /// Cancels an AsyncCommand's running task through a reference to the task
        /// </summary>
        /// <param name="taskToCancel">the task of an AsyncCommand to cancel</param>
        public void CancelRunningCommandTask(Task taskToCancel) {
            commandStream.CancelRunningCommandTask(taskToCancel);
        }
        /// <summary>
        /// Cancels an AsyncCommand's running task through a reference to the command
        /// </summary>
        /// <param name="taskToCancel">the AsyncCommand who's task should be canceled </param>
        public void CancelRunningCommandTask(IAsyncCommand asyncCommand) {
            commandStream.CancelRunningCommandTask(asyncCommand);
        }
        /// <summary>
        /// Get a dictionary of any faulted tasks and the exceptions that they threw
        /// </summary>
        /// <returns> A read only copy of the dictionary of faulted tasks and their exceptions </returns>
        public ReadOnlyDictionary<Task, Exception> GetFaultedCommandTasks() {
            return commandStream.GetFaultedCommandTasks();
        }
        /// <summary>
        /// Empties the history of the internal CommandStream and replaces it with an empty one.
        /// </summary>
        /// <returns> The old history of the internal CommandStream </returns>
        public ReadOnlyCollection<ICommand> DropHistory() {
            return commandStream.DropHistory();
        }
        /// <summary>
        /// This will remove all commands from the CommandStream's queue and replace it with a new empty queue.
        /// </summary>
        /// <remark> This can be useful to rearrange the commands in a queue. Simple preform the needed changes on the returned list and re-queue it </remark>
        /// <returns> The commands in the previous queue, in case this information is needed.</returns>
        public List<ICommand> DropQueue() {
            return commandStream.DropQueue();
        }
        /// <summary>
        /// The number of Commands recorded by the CommandManager's CommandStream
        /// </summary>
        public int HistoryCount
        {
            get => commandStream.HistoryCount;
        }
        /// <summary>
        /// The depth to which the CommandManager's CommandStream records its history
        /// </summary>
        public float HistoryDepth {
            get => commandStream.HistoryDepth;
        }
        /// <summary>
        /// The Number of commands queued in the CommandManager's CommandStream 
        /// </summary>
        public int QueueCount {
            get => commandStream.QueueCount;
        }
        /// <summary>
        /// Wether or not the CommandManger's CommandStream has an empty queue
        /// </summary>
        public bool QueueEmpty {
            get => commandStream.QueueEmpty;
        }
        /// <summary>
        /// Queue's a Command into the CommandManager's CommandStream
        /// </summary>
        /// <param name="command"> The Command to be Queued </param>
        public void QueueCommand(ICommand command) {
            commandStream.QueueCommand(command);
        }
        /// <summary>
        /// Queue's multiple commands into the CommandManager's CommandStream
        /// </summary>
        /// <param name="commands"> The collection of commands to be Queued </param>
        public void QueueCommands(IEnumerable<ICommand> commands) {
            commandStream.QueueCommands(commands);
        }
        /// <summary>
        /// Queue the undo-command of a Command implementing IUndoable into the CommandStream
        /// </summary>
        /// <param name="commandToUndo"> The IUndoable Command to queue an undo-command for </param>
        /// <returns>Wether the undo command was allowed to be queued </returns>
        public bool TryQueueUndoCommand(IUndoable commandToUndo) {
            return commandStream.TryQueueUndoCommand(commandToUndo);
        }
        /// <summary>
        /// Forces the internal CommandStream to queue and IUndoable commands undo command
        /// </summary>
        /// <param name="commandToUndo">The IUndoable command to undo </param>
        public void ForceQueueUndoCommand(IUndoable commandToUndo) {
            commandStream.ForceQueueUndoCommand(commandToUndo);
        }
        /// <summary>
        /// Examine the next command in the CommandStream's commandQueue with out executing it
        /// </summary>
        /// <param name="nextCommand">The next command in the queue, null if empty</param>
        /// <returns>Wether there was a command in queue or not</returns>
        public bool TryPeekNext(out ICommand nextCommand) {
            return commandStream.TryPeekNext(out nextCommand);
        }
        /// <summary>
        /// Removes the next command in the CommandStream's queue, if it has one, and adds it back to the end of the queue.
        /// </summary>
        public void RequeueNextCommand() {
            commandStream.RequeueNextCommand();
        }
        /// <summary>
        /// Removes the next command from the CommandStream's queue without executing it
        /// </summary>
        public void SkipNextCommand(){
            commandStream.SkipNextCommand();
        }
        /// <summary>
        /// Bypass the CommandStream's queue and immediately attempt to execute a command
        /// </summary>
        /// <param name="command">The command to immediately be executed</param>
        /// <returns>The ExecuteCode for the attempt to execute the command</returns>
        public ExecuteCode TryExecuteImmediate(ICommand command) {
            return commandStream.TryExecuteImmediate(command);
        }
        /// <summary>
        /// Bypass the CommandStream's queue and immediately attempt to execute an IUndoable's undo command if the IUndoable is in the CommandStream's history
        /// </summary>
        /// <param name="undoable">the IUndoable to execute the undo command of</param>
        /// <returns>The ExecuteCode of the attempt to execute the undo command of the IUndoable, Execute.Failure if the IUndoable was not in the CommandStream's history </returns>
        public ExecuteCode TryUndoImmediate(IUndoable undoable) {
            return commandStream.TryUndoImmediate(undoable);
        }
        /// <summary>
        /// Bypass the command queue and immediately attempt to execute an IUndoable's undo command, regardless of wether the IUndoable is in the CommandStream's history
        /// </summary>
        /// <param name="undoable">the IUndoable to execute the undo command of</param>
        /// <returns>The ExecuteCode of the attempt to execute the undo command of the IUndoable</returns>
        public ExecuteCode ForceTryUndoImmediate(IUndoable undoable) {
            return commandStream.ForceTryUndoImmediate(undoable);
        }
        /// <summary>
        /// sets the singleton instance of this object
        /// </summary>
        private void Awake() {
            if(Instance != null && Instance != this) {
                Destroy(this);
            } else {
                Instance = this;
            }
        }
        /// <summary>
        /// Constructs the objects internal CommandStream
        /// </summary>
        void Start() {
            if (maximumHistoryDepth >= 0) {
                commandStream = new CommandStream(maximumHistoryDepth);
            } else {
                commandStream = new CommandStream();
            }
            commandStream.OnTaskFaulted += (ex) => OnTaskFaulted.Invoke(ex);
        }
        /// <summary>
        /// Executes the next command in queue if it isn't empty and command execution isn't frozen
        /// </summary>
        void Update() {
            if (!freezeCommandExecution && !QueueEmpty) {
                commandStream?.TryExecuteNext();
            }
        }
    }
}