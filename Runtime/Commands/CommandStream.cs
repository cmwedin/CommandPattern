using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SadSapphicGames.CommandPattern
{
    
    public static class DropoutStackExtension {
        /// <summary>
        ///  An extension method to drop the bottom of a stack such that it is a certain size 
        /// </summary>
        /// <param name="desiredSize"> The desired size of the stack after dropping the bottom elements </param>
        public static void Dropout<T>(this Stack<T> stack, int desiredSize) {
            if(stack.Count <= desiredSize) {
                return;
            } else {
                stack = new Stack<T>(
                    stack.ToArray().Take(desiredSize)
                );
                return;
            }
        }
    }
    public class CommandStream {
        /// <summary>
        /// This is a Queue of the commands to be executed
        /// </summary>
        private Queue<Command> commandQueue = new Queue<Command>();
        /// <summary>
        /// This is a stack of the executed command history
        /// </summary>
        private Stack<Command> CommandHistory = new Stack<Command>();


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
        private void RecordCommand(Command command) {
            CommandHistory.Push(command);
            if(CommandHistory.Count > historyDepth) {
                CommandHistory.Dropout((int)historyDepth);
            }

        }
        /// <summary>
        /// Executes the next command in the CommandStream and records if it the requested history depth is greater than 0
        /// </summary>
        public void ExecuteNext() {
            Command nextCommand;
            if(!commandQueue.TryDequeue(out nextCommand)) {
                Debug.LogWarning("CommandStream queue empty");
                return;
            }
            nextCommand.Execute();
            if(historyDepth > 0) {
                RecordCommand(nextCommand);
            } 
        }
    }
}