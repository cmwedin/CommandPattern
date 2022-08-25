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
        public event Action OnRebindStart;
        protected void InvokeOnRebindStart() {
            OnRebindStart?.Invoke();
        }
        public event Action OnRebindFinished;
        protected void InvokeOnRebindFinished() {
            OnRebindFinished?.Invoke();
        }
        public Task commandTask;
        protected Command undoCommand;


        public RebindKeyCommand(Dictionary<InputType, KeyCode> keyBinds, InputType inputToRebind) {
            this.keyBinds = keyBinds;
            this.inputToRebind = inputToRebind;
            prevBinding = keyBinds[inputToRebind];
        }
        private async Task ExecuteAsync() {
            bool done = false;
            bool prevDemoState = InputCommandStream.Instance.activateDemo.isOn;
            InputCommandStream.Instance.activateDemo.isOn = false;
            GameObject tempGO = new GameObject("TempKeystrokeListener");
            var selector = tempGO.AddComponent<KeystrokeListener>();
            selector.OnKeystrokeDetected += (KeyCode) => { 
                if(keyBinds.ContainsValue(KeyCode)) { 
                    return;
                } else {
                    keyBinds[inputToRebind] = KeyCode;
                    done = true;
                    GameObject.Destroy(tempGO);
                }
            };
            while(!done) {
                await Task.Delay(1);
            }
            InputCommandStream.Instance.activateDemo.isOn = prevDemoState;
            Debug.Log("rebind async task completed");
            InvokeOnRebindFinished();
        }

        public override void Execute() {
            InvokeOnRebindStart();
            commandTask = ExecuteAsync();
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