using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern
{
    public class CommandStream {
        private Queue<Command> CommandQueue = new Queue<Command>();
        private Stack<Command> ExecutedCommands = new Stack<Command>();

        public CommandStream() {
        }
        public void QueueCommand(Command command) {
            CommandQueue.Enqueue(command);
        }
        public void ExecuteNext() {
            Command nextCommand;
            if(!CommandQueue.TryDequeue(out nextCommand)) {
                Debug.LogWarning("CommandStream queue empty");
                return;
            }
            nextCommand.Execute();
        }
    }
}