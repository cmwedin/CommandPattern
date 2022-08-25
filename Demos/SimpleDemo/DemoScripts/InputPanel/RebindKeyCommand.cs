using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class RebindKeyCommand : Command, IUndoable
    {
        protected Dictionary<InputType, KeyCode> keyBinds;
        protected InputType inputToRebind;
        protected KeyCode prevBinding;
        public event Action OnRebindFinished;
        protected Command undoCommand;
        protected void InvokeOnRebindFinished() {
            OnRebindFinished?.Invoke();
        }

        public RebindKeyCommand(Dictionary<InputType, KeyCode> keyBinds, InputType inputToRebind) {
            this.keyBinds = keyBinds;
            this.inputToRebind = inputToRebind;
            prevBinding = keyBinds[inputToRebind];
        }

        public async override void Execute() {
            bool done = false;
            GameObject tempGO = new GameObject("TempKeystrokeListener");
            var selector = tempGO.AddComponent<KeySelector>();
            selector.KeySelected += (KeyCode) => { 
                keyBinds[inputToRebind] = KeyCode;
                done = true;
                GameObject.Destroy(tempGO);
            };
            while(!done) {
                await Task.Delay(1);
            }
            InvokeOnRebindFinished();
        }

        public Command GetUndoCommand()
        {
            if(undoCommand == null){
                undoCommand = new SilentRebindCommand(keyBinds, inputToRebind, prevBinding);
            }
            return undoCommand;
        }
    }
}