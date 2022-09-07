using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// This component is used to listen for keystrokes
    /// </summary>
    public class KeystrokeListener : MonoBehaviour
    {
        /// <summary>
        /// The rebindKeyCommand that created this object
        /// </summary>
        public RebindKeyCommand rebindKeyCommand { get; set; }
        /// <summary>
        /// an event raised when a keycode is detected
        /// </summary>
        public event Action<KeyCode> OnKeystrokeDetected;
        /// <summary>
        /// If this keycode is pressed the RebindCommand will be cancelled 
        /// </summary>
        private KeyCode cancelKeyCode = KeyCode.Escape;
        /// <summary>
        /// loops through the possible inputs and raises the OnKeystrokeDetected event with it as an argument if it is currently down
        /// </summary>
        void Update()
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode)) {
                    if(keyCode == cancelKeyCode) {
                        //? Destroying this components also cancels the command
                        Destroy(gameObject);
                    } else
                    {
                        OnKeystrokeDetected?.Invoke(keyCode);
                    }
                }
            }
        }
        /// <summary>
        /// Cancels the RebindKeyCommand if this object is destroyed before it is finished
        /// </summary>
        private void OnDestroy() {
            if(!rebindKeyCommand.CommandTask.IsCompleted) {
                RebindKeyManager.Instance.CancelRebindCommand(rebindKeyCommand);
            }
        }
    }
}