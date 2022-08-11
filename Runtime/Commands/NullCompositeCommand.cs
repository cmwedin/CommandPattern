using System.Collections.Generic;
using System.Linq;

namespace SadSapphicGames.CommandPattern {
    public class NullCompositeCommand : CompositeCommand, IUndoable {
        public NullCompositeCommand(int size) : base(){
            List<Command> subCommands = new List<Command>();
            for (int i = 0; i < size; i++) {
                subCommands.Add(new NullCommand());
            }
            this.subCommands = subCommands;
        }

        public Command GetUndoCommand() {
            return this;
        }
    }
}