using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

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

        private CancellationTokenSource cancellationTokenSource;

        protected AsyncCommand() {
            cancellationTokenSource = new CancellationTokenSource();
            OnTaskCompleted += () => {
                cancellationTokenSource.Dispose();
            };
            OnTaskCanceled += () => {
                cancellationTokenSource.Dispose();
            };
            OnTaskFaulted += (ex) => {
                cancellationTokenSource.Dispose();
            };
        }
        /// <summary>
        /// This can be used to signal to CommandTask that it should be canceled, provided that you respond to CancellationToken indicating it has been canceled from ExecuteAsync
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get => cancellationTokenSource; }
        /// <summary>
        /// This can be used in ExecuteAsync to determine if the CommandTask has been canceled.
        /// </summary>
        protected CancellationToken CancellationToken { get => CancellationTokenSource.Token; }

        /// <summary>
        /// This event is invoked when CommandTask is completed. I.E. - when we reach the end of ExecuteAsync(). Subscribe to it to preform an action at after the command has fully completed.
        /// </summary>
        public event Action OnTaskCompleted;
        /// <summary>
        /// This event is invoked if the command task is canceled. Most bookkeeping that occurs in this situation is handled by the package but you can subscribe to this as well if needed.
        /// </summary>
        public event Action OnTaskCanceled;
        /// <summary>
        /// This event is invoked if the command task throws an exception.  
        /// </summary>
        public event Action<Exception> OnTaskFaulted;
        /// <summary>
        /// This method invokes the OnTaskCompleted when CommandTask is completed 
        /// </summary>
        /// <returns> The task for invoking OnTaskCompleted </returns>
        private async Task InvokeWhenTaskCompleted() {
            try {
                await CommandTask;
            } catch (OperationCanceledException ex) {
                OnTaskCanceled?.Invoke();
                throw ex;
            } catch (Exception ex) {
                OnTaskFaulted?.Invoke(ex);
                throw ex;
            }
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