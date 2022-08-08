using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// Indicates a command can be undone
    /// </summary>
    interface IUndoable {
        /// <summary>
        /// Creates a command to revert the changes of the implementing command
        /// </summary>
        /// <returns> a command that reverts the implementing command</returns>
        public Command GetUndoCommand();
    }
}
