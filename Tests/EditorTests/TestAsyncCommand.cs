using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    public class TestAsyncCommand : AsyncCommand
    {
        private bool isComplete;

        public override async Task ExecuteAsync() {
            while(!isComplete) {
                await Task.Yield();
                CancellationToken.ThrowIfCancellationRequested();
            }
        }

        public void Complete() {
            isComplete = true;
        }
    }
}