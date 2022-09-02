using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The Command to modify the health of an IHealth implementing object
    /// </summary>
    public class ModifyHealthCommand : Command, IUndoable, IFailable
    {
        /// <summary>
        /// The amount to modify the health by 
        /// </summary>
        public int Magnitude { get; private set; }
        /// <summary>
        /// The receiver of the command, the IHealth object being modified
        /// </summary>
        public IHealth healthImplementer;
        /// <summary>
        /// The undoCommand of this object
        /// Caching the undo command as a member, while not required by IUndoable, can be useful so each time we get a Command's undo we are working with the same object
        /// </summary>
        private Command undoCommand;
        /// <summary>
        /// Constructs a ModifyHealthCommand object
        /// </summary>
        public ModifyHealthCommand(int magnitude, IHealth healthImplementer) {
            this.Magnitude = magnitude;
            this.healthImplementer = healthImplementer;
        }
        /// <summary>
        /// Executes the command
        /// </summary>
        public override void Execute() {
            healthImplementer.Health += Magnitude;
        }
        /// <summary>
        /// If undoCommand has not yet been created, creates it, then returns undoCommand
        /// </summary>
        /// <returns> The undoCommand of this object </returns>
        public ICommand GetUndoCommand() {
            if(undoCommand == null) {
                undoCommand = new ModifyHealthCommand(-Magnitude, healthImplementer);
            }
            return undoCommand;
        }
        /// <summary>
        /// Determines if the command would modify the health of its receiver above its maximum or below zero
        /// </summary>
        /// <returns>If the above is true</returns>
        public bool WouldFail()
        {
            var futureValue = healthImplementer.Health + Magnitude;
            return (futureValue > healthImplementer.MaxHealth || futureValue < 0);
        }
    }
}