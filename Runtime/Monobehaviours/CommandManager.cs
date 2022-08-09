using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// A singleton manager for a single-stream, out of the box implementation of the Command pattern. For more complicated implementations create your own CommandStream wrappers. 
/// Does not drop Commands from its CommandHistory. Executes the next command in the CommandStream every frame.
/// </summary>

namespace SadSapphicGames.CommandPattern {
    public class SingletonCommandManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of the CommandManger.
        /// </summary>
        public static SingletonCommandManager Instance { get; private set; }
        private CommandStream commandStream = new CommandStream();
        /// <summary>
        /// Get the underlying CommandStream's history
        /// </summary>
        /// <returns> A ReadOnlyCollection of all the commands executed by the CommandManager's CommandStream </returns>
        public ReadOnlyCollection<Command> GetCommandHistory() {
            return commandStream.GetCommandHistory();
        }
        /// <summary>
        /// The number of Commands executed by the CommandManager
        /// </summary>
        public int HistoryCount {
            get => GetCommandHistory().Count;
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
        public void QueueUndoCommand(IUndoable commandToUndo) {
            commandStream.QueueUndoCommand(commandToUndo);
        }

        private void Awake() {
            if(Instance != null && Instance != this) {
                Destroy(this);
            } else {
                Instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update() {
            commandStream.TryExecuteNext();
        }
    }
}