

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    /// <summary>
    /// A mockup of a command that does not implement IUndoable, aside from that identical to NullCommand
    /// </summary>
    public class IrreversibleNullCommand : Command
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        public override void Execute()
        {
        }
    }
}