using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// A Command that is composed of multiple child commands, all of which are executed together and leave one record in the CommandStream's history. 
    /// <remark> For more information on this type of object seek external documentation on the composite design pattern </remark>
    /// </summary>
    public class CompositeCommand : Command {
        protected List<Command> subCommands = new List<Command>();

        /// <summary>
        /// Creates an empty CompositeCommand
        /// </summary>
        public CompositeCommand() {
        }

        /// <summary>
        ///  Creates a CompositeCommand object from a collection of commands. 
        /// </summary>
        /// <param name="subCommands"> The collection of commands to be packaged into a CompositeCommand </param>
        public CompositeCommand(IEnumerable<Command> subCommands)
        {
            var subCommandsEnum = subCommands.GetEnumerator();
            while(subCommandsEnum.MoveNext()) {
                this.subCommands.Add(subCommandsEnum.Current);
            }
        }

        /// <summary>
        /// Adds a Command to this objects children 
        /// </summary>
        /// <param name="childCommand"> The Command to be added to the objects children </param>
        protected virtual void AddChild(Command childCommand) {
            subCommands.Add(childCommand);
        }

        /// <summary>
        /// Number of child Commands included in this object
        /// </summary>
        public int ChildCount { get => subCommands.Count; }
        
        /// <summary>
        /// Executes each of the commands included in this object
        /// </summary>
        public override void Execute() {
            for (int i = 0; i < ChildCount; i++) {
                subCommands[i].Execute();
            }
        }
    }
}