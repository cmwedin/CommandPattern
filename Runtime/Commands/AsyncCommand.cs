using System;
using System.Threading.Tasks;

namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// The base class of all commands who's execute method involves asynchronous tasks
    /// </summary>
    public abstract class AsyncCommand : Command, IAsyncCommand
    {
        /// <summary>
        /// The backing field of CommandTask
        /// </summary>
        private Task commandTask;
        
        /// <summary>
        /// The task for invoking OnCompletionEvent. Runs when commandTask is completed.
        /// </summary>
        private Task invokeCompletionEventTask;
        
        /// <summary>
        /// The task for the completion of the ExecuteAsync method after it reaches its first await and returns control back to the calling method (CommandStream.TryExecuteNext())
        /// </summary>
        public Task CommandTask { get => commandTask; private set => commandTask = value; }

        /// <summary>
        /// This event is invoked when CommandTask is completed. I.E. - when we reach the end of ExecuteAsync(). Subscribe to it to preform an action at after the command has fully completed.
        /// </summary>
        public event Action OnTaskCompleted;

        /// <summary>
        /// This method invokes the OnTaskCompleted when CommandTask is completed 
        /// </summary>
        /// <returns> The task for invoking OnTaskCompleted </returns>
        private async Task InvokeWhenTaskCompleted() {
            await CommandTask;
            OnTaskCompleted?.Invoke();
        }

        /// <summary>
        /// This method may not be overridden in asynchronous commands. Use ExecuteAsync for your command's logic instead. 
        /// </summary>
        public sealed override void Execute() {
            CommandTask = ExecuteAsync();
            invokeCompletionEventTask = InvokeWhenTaskCompleted();
        }

        /// <summary>
        /// Executes the command asynchronously. Remember to add the async keyword as that is not considered part of the method signature and cannot be added to abstract methods. 
        /// </summary>
        /// <returns> The task for the completion of this method after it reaches its first await and returns control to the calling method. </returns>
        public abstract Task ExecuteAsync();
    }
}