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

        public TestAsyncCommand()
        {
            OnTaskCompleted += () => { Debug.Log("Completion event invoked"); };
        }

        public override async Task ExecuteAsync() {
            while(!isComplete) {
                await Task.Delay(1);
            }
            Debug.Log("Async test done");
        }

        public void Complete() {
            isComplete = true;
        }
    }
}