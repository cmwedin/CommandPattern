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
        /// this is a limit of the maximum number of commands that will be recorded in the CommandHistory
        /// </summary>
        public float HistoryDepth { 
            get => historyDepth; 
            private set { 
                historyDepth = Mathf.Max(value,0); 
            } 
        }

        /// <summary>
        /// Creates a new CommandStream
        /// </summary>
        /// <param name="_historyDepth"> 
        /// The depth to which previously executed commands will be recorded, default 100. Once this depth is reached the oldest commands will be forgotten first. 
        /// <remark> 
        /// For the best performance in both time and space this should be zero if the command history is not needed.
        /// If the command history is needed and space complexity is irrelevant (or significantly less important than time complexity) it should be Positive Infinity as dropping the  bottom of the stack can be expensive.
        /// </remark> 
        /// </param>
        public CommandStream(float _historyDepth = Single.PositiveInfinity) {
            HistoryDepth = _historyDepth;
        }
        /// <summary>
        /// This adds a new command to the queue of commands to be executed by the CommandStream
        /// </summary>
        /// <param name="command"> The Command to be Queued </param>
        public void QueueCommand(Command command) {
            commandQueue.Enqueue(command);
        }
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
        /// Executes the next command in the CommandStream and records if it the requested history depth is greater than 0
        /// </summary>
        public bool TryExecuteNext() {
            Command nextCommand;
            if(!commandQueue.TryDequeue(out nextCommand)) {
                Debug.LogWarning("CommandStream queue empty");
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