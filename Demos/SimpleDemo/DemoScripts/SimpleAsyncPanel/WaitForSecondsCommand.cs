using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class WaitForSecondsCommand : AsyncCommand
    {
        int timeStep;
        float secondsToWait;
        int millisecondsToWait { get => (int)(secondsToWait * 1000); }
        WaitProgress waitProgress = new WaitProgress();
        public event Action<float> ProgressChanged;

        public WaitForSecondsCommand(float secondsToWait, int timeStep)
        {
            this.secondsToWait = secondsToWait;
            this.timeStep = timeStep;
            waitProgress.ProgressChanged += (value) => { ProgressChanged.Invoke(value); };
        }

        public override async Task ExecuteAsync() {
            Debug.Log($"starting {millisecondsToWait} ms wait");
            waitProgress.Report(0);
            for (int i = 0; i <= millisecondsToWait; i+=timeStep) {
                await Task.Delay(timeStep);
                waitProgress.Report((float)i / (float)millisecondsToWait);
                CancellationToken.ThrowIfCancellationRequested();
            }
            Debug.Log("Wait done");
        }
    }
}