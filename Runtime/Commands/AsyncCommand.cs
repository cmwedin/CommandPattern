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

        /// <summary>
        /// Created the cancellation token of the async command and sets up its disposal once the task completes (wether that is from success, cancellation, or faulting)
        /// </summary>
        protected AsyncCommand() {
            OnTaskCompleted += () => {
                cancellationTokenSource.Dispose();
                OnAnyTaskEnd?.Invoke();
            };
            OnTaskCanceled += () => {
                cancellationTokenSource.Dispose();
                OnAnyTaskEnd?.Invoke();

            };
            OnTaskFaulted += (ex) => {
                cancellationTokenSource.Dispose();
                OnAnyTaskEnd?.Invoke();
            };
        }
        /// <summary>
        /// This can be used to signal to CommandTask that it should be canceled, null until the execute method is invoked
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
        /// This event is invoked when any of the OnTask[blank] events are to avoid needed to subscribe the same delegate to multiple events when logic needs to run regardless of how the task finished
        /// </summary>
        public event Action OnAnyTaskEnd;
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
            cancellationTokenSource = new CancellationTokenSource();
            CommandTask = ExecuteAsync();
            if (CommandTask.Status == TaskStatus.Faulted) { throw CommandTask.Exception; }
            invokeCompletionEventTask = InvokeWhenTaskCompleted();
        }

        /// <summary>
        /// Executes the command asynchronously. Remember to add the async keyword as that is not considered part of the method signature and cannot be added to abstract methods. 
        /// <br/>
        /// IMPORTANT: if you want to be able to cancel the AsyncCommand's task you must invoke CancellationToken.ThrowIfCancellationRequested() within this method somewhere after the first await. If you do not do so attempting to cancel it will do nothing.
        /// </summary>
        /// <returns> The task for the completion of this method after it reaches its first await and returns control to the calling method. </returns>
        public abstract Task ExecuteAsync();
    }
}