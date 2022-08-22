using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class ModifyHealthCommand : Command, IUndoable, IFailable
    {
        public int Magnitude { get; private set; }
        private HealthBar healthBar;
        //? Caching the undo command as a member can be useful so each time we get a Command's undo we are working with the same object
        private Command undoCommand;

        public ModifyHealthCommand(int magnitude, HealthBar healthBar) {
            this.Magnitude = magnitude;
            this.healthBar = healthBar;
        }

        public override void Execute() {
            healthBar.Health += Magnitude;
        }

        public Command GetUndoCommand() {
            if(undoCommand == null) {
                undoCommand = new ModifyHealthCommand(-Magnitude, healthBar);
            }
            return undoCommand;
        }

        public bool WouldFail()
        {
            var futureValue = healthBar.Health + Magnitude;
            return (futureValue > healthBar.MaxHealth || futureValue < 0);
        }
    }
}