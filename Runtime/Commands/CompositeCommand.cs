using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// A Command that is composed of multiple child commands, all of which are executed together and leave one record in the CommandStream's history.
    /// As a CompositeCommand can be created from any collection of Commands and some of these commands may implement IFailable, all composite commands also implement IFailable.
    /// If any sub-command of a CompositeCommand is IFailable and returns true for WouldFail(), the composite will also return true.  
    /// <remark> For more information on this type of object seek external documentation on the composite design pattern </remark>
    /// </summary>
    public abstract class CompositeCommand : Command {
        protected List<Command> subCommands = new List<Command>();

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