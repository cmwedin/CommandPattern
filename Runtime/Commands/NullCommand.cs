namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// A Command that does nothing
    /// </summary>
    public class NullCommand : Command, IUndoable
    {
        public NullCommand() {
        }
        /// <summary>
        ///  Does Nothing
        /// </summary>
        public override void Execute() {
        }
        /// <summary>
        ///  Since the command doesn't do anything it returns itself
        /// </summary>
        /// <returns> The same null command object </returns>
        public ICommand GetUndoCommand() {
            return this;
        }
    }
}