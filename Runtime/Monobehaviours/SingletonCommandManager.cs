using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;



namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// A singleton manager for a single-stream, out of the box implementation of the Command pattern. For more complicated implementations create your own CommandStream wrappers. 
    /// Does not drop Commands from its CommandHistory. Executes the next command in the CommandStream every frame.
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
        private CommandStream commandStream;
        private bool freezeCommandExecution = false;
        /// <summary>
        /// Turns command execution off if its on and on if its off
        /// </summary>
        public void ToggleCommandExecution()
        {
            freezeCommandExecution = !freezeCommandExecution;
        }
        /// <summary>
        /// Turns command execution off or on
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
        public ReadOnlyCollection<Command> GetCommandHistory() {
            return commandStream.GetCommandHistory();
        }
        /// <summary>
        /// Empties the history of the internal CommandStream and replaces it with an empty one.
        /// </summary>
        /// <returns> The old history of the internal CommandStream </returns>
        public ReadOnlyCollection<Command> DropCommandHistory() {
            return commandStream.DropHistory();
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
        public void QueueCommand(Command command) {
            commandStream.QueueCommand(command);
        }
        /// <summary>
        /// Queue's multiple commands into the CommandManager's CommandStream
        /// </summary>
        /// <param name="commands"> The collection of commands to be Queued </param>
        public void QueueCommands(IEnumerable<Command> commands) {
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
        /// <param name="commandToUndo">The IUndoable commadn to undo </param>
        public void ForceQueueUndoCommand(IUndoable commandToUndo) {
            commandStream.ForceQueueUndoCommand(commandToUndo);
        }

        private void Awake() {
            if(Instance != null && Instance != this) {
                Destroy(this);
            } else {
                Instance = this;
            }
        }
        // Start is called before the first frame update
        void Start() {
            if (maximumHistoryDepth >= 0) {
                commandStream = new CommandStream(maximumHistoryDepth);
            } else {
                commandStream = new CommandStream();
            }
        }

        // Update is called once per frame
        void Update() {
            if (!freezeCommandExecution) {
                commandStream?.TryExecuteNext();
            }
        }
    }
}