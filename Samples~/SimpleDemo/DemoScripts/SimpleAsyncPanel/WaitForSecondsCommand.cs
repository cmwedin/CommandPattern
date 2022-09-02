using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// An AsyncCommand the will wait for an amount of seconds before completing 
    /// </summary>
    public class WaitForSecondsCommand : AsyncCommand
    {
        /// <summary>
        /// the time-step in milliseconds which the async command await before checking for cancellation and reporting progress
        /// </summary>
        int timeStep;
        /// <summary>
        /// the length of time in seconds the command will wait for
        /// </summary>
        float secondsToWait;
        int millisecondsToWait { get => (int)(secondsToWait * 1000); }
        /// <summary>
        /// The IProgress object through which the command reports its progress
        /// </summary>
        WaitProgress waitProgress = new WaitProgress();
        /// <summary>
        /// An event to bubble up the waitProgress object's ProgressChanged event
        /// </summary>
        public event Action<float> ProgressChanged;

        /// <summary>
        /// Constructs the WaitForSecondsCommand and subscribes its ProgressChanged event to the waitProgress objects ProgressChanged event
        /// </summary>
        public WaitForSecondsCommand(float secondsToWait, int timeStep)
        {
            this.secondsToWait = secondsToWait;
            this.timeStep = timeStep;
            waitProgress.ProgressChanged += (value) => { ProgressChanged.Invoke(value); };
        }
        /// <summary>
        /// Executes the command asynchronously in steps of time-step
        /// </summary>
        /// <returns>The task for the completion of the async command after each await</returns>
        public override async Task ExecuteAsync() {
            Debug.Log($"starting {millisecondsToWait} ms wait");
            waitProgress.Report(0);
            for (int i = 0; i <= millisecondsToWait; i+=timeStep) {
                await Task.Delay(timeStep);
                waitProgress.Report((float)i / (float)millisecondsToWait);
                CancellationToken.ThrowIfCancellationRequested();
            }
            waitProgress.Report(1);
            Debug.Log("Wait done");
        }
    }
}