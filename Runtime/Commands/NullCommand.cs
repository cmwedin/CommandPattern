namespace SadSapphicGames.CommandPattern {
    public class NullCommand : Command, IUndoable
    {
        public NullCommand() {
        }

        public override void Execute() {
        }

        public Command GetUndoCommand() {
            return this;
        }
    }
}