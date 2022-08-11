using System.Collections.Generic;

namespace SadSapphicGames.CommandPattern
{
    public class SimpleComposite : CompositeCommand {
        public SimpleComposite(IEnumerable<Command> subCommands) {
            var subCommandsEnum = subCommands.GetEnumerator();
            while(subCommandsEnum.MoveNext()) {
                if (subCommandsEnum.Current is IFailable) {
                    throw new System.ArgumentException("A simple composite command can not contain failable commands");
                } else {
                    AddChild(subCommandsEnum.Current);
                }
            }
        }
    }
}