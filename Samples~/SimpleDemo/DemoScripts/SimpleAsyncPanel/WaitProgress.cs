using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// An object to report the progress of the WaitForSecondsCommand
    /// </summary>
    public class WaitProgress : IProgress<float>
    {
        /// <summary>
        /// an event that is invoked when a new value is reported for the progress
        /// </summary>
        public event Action<float> ProgressChanged;
        /// <summary>
        /// invokes the ProgressChanges event with a new progress between zero and one
        /// </summary>
        /// <param name="value"></param>
        public void Report(float value) {
            ProgressChanged.Invoke(Mathf.Clamp(value,0,1));
        }
    }
}