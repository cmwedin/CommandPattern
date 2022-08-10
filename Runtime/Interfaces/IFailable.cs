using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// Interface implemented by commands that could fail to execute. Commands that would fail do not have their execute method invoked and are not recorded in the CommandStream's history
    /// </summary>
    public interface IFailable
    {
        /// <summary>
        /// Determine if the implementing command would be able to be executed or if it would fail
        /// </summary>
        /// <returns> True if the implementing command would fail, false if it would execute successfully. </returns>
        public bool WouldFail();
    }
}