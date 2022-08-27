using System.Collections.Generic;
using System.Linq;

namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// Like the NullCommand this is a composite command that does nothing, multiple times.
    /// </summary>
    public class NullCompositeCommand : CompositeCommand, IUndoable {
        /// <summary>
        /// Creates a NullCompositeCommand composed of multiple NullCommands
        /// </summary>
        /// <param name="size">How many NullCommands to include in the composite</param>
        public NullCompositeCommand(int size) : base(){
            List<ICommand> subCommands = new List<ICommand>();
            for (int i = 0; i < size; i++) {
                subCommands.Add(new NullCommand());
            }
            this.subCommands = subCommands;
        }
        /// <summary>
        /// Like the NullCommand it is composed of, a NullCompositeCommand is its own undo-command
        /// </summary>
        /// <returns>This object</returns>
        public ICommand GetUndoCommand() {
            return this;
        }
    }
}