using System.Collections.Generic;

namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// A CompositeCommand created from a collection of Command's that cannot fail
    /// </summary>
    public class SimpleComposite : CompositeCommand {
        /// <summary>
        /// Creates a SimpleComposite from a collection of Command's that cannot fail
        /// </summary>
        /// <param name="subCommands">The collection of unfailable Commands to be included in the composite </param>
        /// <exception cref="System.ArgumentException">One or more of the Commands included in the argument implement IFailable </exception>
        public SimpleComposite(IEnumerable<Command> subCommands) {
            var subCommandsEnum = subCommands.GetEnumerator();
            while(subCommandsEnum.MoveNext()) {
                if (subCommandsEnum.Current is IFailable) {
                    throw new System.ArgumentException("A simple composite command cannot contain IFailable commands");
                } else {
                    AddChild(subCommandsEnum.Current);
                }
            }
        }
    }
}