using System;
using System.Threading.Tasks;

namespace SadSapphicGames.CommandPattern
{
    public abstract class AsyncCommand : Command, IAsyncCommand
    {
        private Task commandTask;
        private Task invokeTask;
        
        public Task CommandTask { get => commandTask; private set => commandTask = value; }

        public event Action OnTaskCompleted;
        private async Task InvokeWhenTaskCompleted() {
            await CommandTask;
            OnTaskCompleted?.Invoke();
        }

        public override void Execute() {
            CommandTask = ExecuteAsync();
            invokeTask = InvokeWhenTaskCompleted();
        }

        public abstract Task ExecuteAsync();
    }
}