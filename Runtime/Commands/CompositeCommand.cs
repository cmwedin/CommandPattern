using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// A Command that is composed of multiple child commands, all of which are executed together and leave one record in the CommandStream's history. 
    /// <remark> For more information on this type of object seek external documentation on the composite design pattern </remark>
    /// </summary>
    public class CompositeCommand : Command {
        private List<Command> subCommands = new List<Command>();

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