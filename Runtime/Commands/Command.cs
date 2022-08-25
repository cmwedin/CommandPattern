using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern
{
    /// <summary>
    /// The base class of all commands
    /// </summary>
    public abstract class Command : ICommand {
        /// <summary>
        /// Executes the command
        /// </summary>
        public abstract void Execute();
    }
}
