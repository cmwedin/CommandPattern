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
            // var undoableCommands = 
            //     from com in subCommands 
            //     where com is IUndoable //? should be every command
            //     select (IUndoable)com;
            var undoCommands =
                from com in subCommands.Cast<IUndoable>()
                select com.GetUndoCommand();
            return new CompositeCommand(undoCommands);
        }
    }
}