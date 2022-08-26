using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class RebindKeyCommand : AsyncCommand
    {
        protected Dictionary<InputType, KeyCode> keyBinds;
        protected InputType inputToRebind;
        protected KeyCode prevBinding;
        public event Action OnRebindStart;
        protected void InvokeOnRebindStart() {
            OnRebindStart?.Invoke();
        }

        public RebindKeyCommand(Dictionary<InputType, KeyCode> keyBinds, InputType inputToRebind) {
            this.keyBinds = keyBinds;
            this.inputToRebind = inputToRebind;
            prevBinding = keyBinds[inputToRebind];
        }
        public override async Task ExecuteAsync() {
            InvokeOnRebindStart();
            Debug.Log("async rebind task started");
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
            Debug.Log("async rebind task completed");
        }
    }
}