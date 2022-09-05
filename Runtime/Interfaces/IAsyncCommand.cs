using System;
using System.Threading;
using System.Threading.Tasks;

namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// This Interface of the AsyncCommand abstract class, It is strongly recommended you use the AsyncCommand class rather than implement this yourself unless you are very familiar with asynchronous programming  
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// This should get the asynchronous task returned by ExecuteAsync after it reaches its first await
        /// </summary>
        public Task CommandTask { get; }
        /// <summary>
        /// This can be used to Cancel the task after it has been started.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// This event should be invoked when CommandTask is successfully completed.
        /// </summary>
        public event Action OnTaskCompleted;
        /// <summary>
        /// This event should be invoked when CommandTask is cancelled.
        /// </summary>
        public event Action OnTaskCanceled;
        /// <summary>
        /// This event should be invoked when CommandTask throws an exception
        /// </summary>
        public event Action<Exception> OnTaskFaulted;
        /// <summary>
        /// This event should be invoked when any of the above three are
        /// </summary>
        public event Action OnAnyTaskEnd;

        /// <summary>
        /// This is where the logic of executing the command should be placed for an AsyncCommand, Execute should just store the return in CommandTask and setup the OnTaskCompleted method. Remember to make this method async as that isn't considered part of its signature.
        /// </summary>
        /// <returns> The Task representing the completion of the method after it reaches its first await statement</returns>
        public abstract Task ExecuteAsync();
    }
}