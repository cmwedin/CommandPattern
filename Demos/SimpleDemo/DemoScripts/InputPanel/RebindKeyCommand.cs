using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class RebindKeyCommand : AsyncCommand, IUndoable
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
        public override async Task ExecuteAsync() {
            InvokeOnRebindStart();
            bool done = false;
            bool prevDemoState = InputCommandStream.Instance.activateDemo.isOn;
            InputCommandStream.Instance.activateDemo.isOn = false;
            InputCommandStream.Instance.activateDemo.gameObject.SetActive(false);
            GameObject tempGO = new GameObject("TempKeystrokeListener");
            var selector = tempGO.AddComponent<KeystrokeListener>();
            selector.OnKeystrokeDetected += (KeyCode) => { 
                if(keyBinds.ContainsValue(KeyCode)) { 
                    if(keyBinds[inputToRebind] == KeyCode) {
                        done = true;
                        GameObject.Destroy(tempGO);
                    } else {
                        return;
                    }
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
            InputCommandStream.Instance.activateDemo.gameObject.SetActive(true);
            Debug.Log("rebind async task completed");
            InvokeOnRebindFinished();
        }

        public override void Execute() {
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