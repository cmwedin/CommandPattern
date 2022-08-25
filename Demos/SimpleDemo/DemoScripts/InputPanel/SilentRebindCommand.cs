using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class SilentRebindCommand : RebindKeyCommand {
        KeyCode rebindTo;
        public SilentRebindCommand(Dictionary<InputType, KeyCode> keyBinds, InputType inputToRebind, KeyCode rebindTo) : base(keyBinds, inputToRebind) {
            this.rebindTo = rebindTo;
        }

        public override void Execute() {
            base.keyBinds[base.inputToRebind] = rebindTo;
            base.InvokeOnRebindFinished();
        }
    }
}