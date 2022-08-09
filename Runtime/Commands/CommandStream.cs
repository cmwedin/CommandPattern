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
        /// This is a stack of the executed command history
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
        private void RecordCommand(Command command) {
            if(historyDepth == 0) return; //? we should never be here if this is true but just in case
            commandHistory.Add(command);
            if(commandHistory.Count > historyDepth) {
                commandHistory.Take((int)historyDepth);
            }
        }

        /// <summary>
        /// Attempts to execute the next command in the queue, returns false if it is empty
        /// </summary>
        /// <returns> False if the command queue is empty. True otherwise. </returns>
        public bool TryExecuteNext() {
            Command nextCommand;
            if(!commandQueue.TryDequeue(out nextCommand)) {
                return false;
            }
            nextCommand.Execute();
            if(historyDepth > 0) {
                RecordCommand(nextCommand);
            }
            return true;
        }
    }
}