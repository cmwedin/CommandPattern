namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// This is the Interface of the Command abstract class, probably not needed unless you want to introduce your own base class for your commands that doesn't inherit from the packages abstract Command
    /// </summary>
    public interface ICommand {
        /// <summary>
        /// Executes the function of the command
        /// </summary>
        public abstract void Execute();
    }
}