namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// This is the Interface of the Command abstract class, unless you are defining your own base type for commands you should probably inherit from Command over this
    /// </summary>
    public interface ICommand {
        /// <summary>
        /// Executes the function of the command
        /// </summary>
        public abstract void Execute();
    }
}