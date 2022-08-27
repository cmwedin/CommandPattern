using System;
using System.Threading.Tasks;

namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// This Interface of the AsyncCommand abstract class, Similar to ICommand you shouldn't inherit this unless you want to implement your own base class for asynchronous commands, which you should only do if you know what your doing re-asynchronous programming. 
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// This should get the asynchronous task returned by ExecuteAsync after it reaches its first await
        /// </summary>
        public Task CommandTask { get; }
        /// <summary>
        /// This event should be invoked when CommandTask is completed so the CommandStream that executed this object can remove the task from its runningCommandTasks list 
        /// </summary>
        public event Action OnTaskCompleted;

        /// <summary>
        /// This is where the logic of executing the command should be placed for an AsyncCommand, Execute should just store the return in CommandTask and setup the OnTaskCompleted method. Remember to make this method async as that isn't considered part of its signature.
        /// </summary>
        /// <returns> The Task representing the completion of the method after it reaches its first await statement</returns>
        public abstract Task ExecuteAsync();
    }
}