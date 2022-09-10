using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The AsyncCommand for rebinding an input key
    /// </summary>
    public class RebindKeyCommand : AsyncCommand
    {
        /// <summary>
        /// A QoL reference to the InputCommandStream instance's binding dictionary, not needed as this is a public field of a singleton, 
        /// but using this reference significantly shortens what would otherwise be very long lines of code
        /// </summary>
        protected Dictionary<InputType, KeyCode> keyBinds;
        /// <summary>
        /// The input that is being rebound
        /// </summary>
        protected InputType inputToRebind;
        /// <summary>
        /// An event that is invoked when the rebinding begins, used to update the binding ui element to blank while waiting for the new binding 
        /// </summary>
        public event Action OnRebindStart;
        /// <summary>
        /// wether or note the demo was active before this command started, as it will be set to inactive during its while it is running
        /// </summary>
        private bool prevDemoState;
        /// <summary>
        /// The KeystrokeListener component the command uses to determine if a key has been pressed
        /// </summary>
        private KeystrokeListener keystrokeListener;

        /// <summary>
        /// Creates a RebindKeyCommand for a given input add subscribes delegates to switch the input demo off on the command is started 
        /// and to switch it back to its previous state and destroy the KeystrokeListener game object once it has finished
        /// </summary>
        /// <param name="inputToRebind">The input to be rebound</param>
        public RebindKeyCommand(InputType inputToRebind) {
            this.keyBinds = InputCommandStream.Instance.InputKeybinds;
            this.inputToRebind = inputToRebind;
        

            this.OnRebindStart += () => { 
                prevDemoState = InputCommandStream.Instance.activateDemoSwitch.isOn;
                InputCommandStream.Instance.activateDemoSwitch.isOn = false;
                InputCommandStream.Instance.activateDemoSwitch.gameObject.SetActive(false);
            };

            this.OnAnyTaskEnd += () => {
                if(keystrokeListener != null) {GameObject.Destroy(keystrokeListener.gameObject);}
                InputCommandStream.Instance.activateDemoSwitch.gameObject.SetActive(true);
            };
        }
        /// <summary>
        /// Executes the command asynchronously
        /// creates an object with the KeystrokeListener component
        /// when that component detects a keystroke verifies the entered key was a valid and if it is sets it to be the new key for the input
        /// cancelled if the KeystrokeListener is prematurely destroyed;
        /// </summary>
        public override async Task ExecuteAsync() {
            OnRebindStart?.Invoke();
            Debug.Log("async rebind task started");
            bool done = false;
            GameObject tempGO = new GameObject("TempKeystrokeListener");
            keystrokeListener = tempGO.AddComponent<KeystrokeListener>();
            keystrokeListener.rebindKeyCommand = this;
            keystrokeListener.OnKeystrokeDetected += (KeyCode) => { 
                if(keyBinds.ContainsValue(KeyCode)) { 
                    if(keyBinds[inputToRebind] == KeyCode) {
                        done = true;
                    } else {
                        return;
                    }
                } else {
                    keyBinds[inputToRebind] = KeyCode;
                    done = true;
                }
            };
            while(!done) {
                await Task.Yield();
                CancellationToken.ThrowIfCancellationRequested();
            }
            Debug.Log("async rebind task completed");
        }
    }
}