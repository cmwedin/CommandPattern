using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    public class AsyncCommandsTests
    {

        // A Test behaves as an ordinary method
        //? async void is bad practice but this has to return void so it is recognized as a test
        [UnityTest]
        public IEnumerator AsyncCommandsTrivialTest() {
            TestAsyncCommand asyncCommand = new TestAsyncCommand();
            CommandStream commandStream = new CommandStream();
            commandStream.QueueCommand(asyncCommand);
            Assert.IsTrue(commandStream.TryExecuteNext());
            Assert.AreEqual(expected: 1, actual: commandStream.HistoryCount);
            Assert.AreEqual(expected: asyncCommand, actual: commandStream.GetCommandHistory()[0]);
            Assert.AreEqual(expected: 1, actual: commandStream.GetRunningCommandTasks().Count);
            Assert.AreEqual(expected: asyncCommand.CommandTask, actual: commandStream.GetRunningCommandTasks()[0]);

            asyncCommand.Complete();
            while(!asyncCommand.CommandTask.IsCompleted) {
                yield return null;
            }
            Assert.IsTrue(asyncCommand.CommandTask.IsCompletedSuccessfully);
            Assert.AreEqual(expected: 0, actual: commandStream.GetRunningCommandTasks().Count);
            // Debug.Log("End of test");
        }
    }
}