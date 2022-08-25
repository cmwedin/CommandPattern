using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class ModifyHealthCommand : Command, IUndoable, IFailable
    {
        public int Magnitude { get; private set; }
        private IHealth healthImplementer;
        //? Caching the undo command as a member can be useful so each time we get a Command's undo we are working with the same object
        private Command undoCommand;

        public ModifyHealthCommand(int magnitude, IHealth healthImplementer) {
            this.Magnitude = magnitude;
            this.healthImplementer = healthImplementer;
        }

        public override void Execute() {
            healthImplementer.Health += Magnitude;
        }

        public Command GetUndoCommand() {
            if(undoCommand == null) {
                undoCommand = new ModifyHealthCommand(-Magnitude, healthImplementer);
            }
            return undoCommand;
        }

        public bool WouldFail()
        {
            var futureValue = healthImplementer.Health + Magnitude;
            return (futureValue > healthImplementer.MaxHealth || futureValue < 0);
        }
    }
}