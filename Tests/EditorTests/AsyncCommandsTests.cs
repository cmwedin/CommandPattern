using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    public class AsyncCommandsTests
    {

        // A Test behaves as an ordinary method
        [UnityTest]
        public IEnumerator AsyncCommandsCompletionTest() {
            TestAsyncCommand asyncCommand = new TestAsyncCommand();
            // var cts = asyncCommand.CancellationTokenSource;
            CommandStream commandStream = new CommandStream();
            Assert.IsNull(asyncCommand.CancellationTokenSource);
            commandStream.QueueCommand(asyncCommand);
            Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.AwaitingCompletion);
            Assert.AreEqual(expected: 1, actual: commandStream.HistoryCount);
            Assert.AreEqual(expected: asyncCommand, actual: commandStream.GetCommandHistory()[0]);
            Assert.AreEqual(expected: 1, actual: commandStream.GetRunningCommandTasks().Count);
            Assert.AreEqual(expected: asyncCommand.CommandTask, actual: commandStream.GetRunningCommandTasks()[0]);

            commandStream.QueueCommand(asyncCommand);
            Assert.AreEqual(commandStream.TryExecuteNext(), ExecuteCode.AlreadyRunning);
            Assert.IsNotNull(asyncCommand.CancellationTokenSource);

            asyncCommand.Complete();
            while(!asyncCommand.CommandTask.IsCompleted) {
                yield return null;
            }
            Assert.IsTrue(asyncCommand.CommandTask.IsCompletedSuccessfully);
            Assert.AreEqual(expected: 0, actual: commandStream.GetRunningCommandTasks().Count);
        }
        [UnityTest]
        public IEnumerator AsyncCommandCancellationTest() {
            TestAsyncCommand asyncCommand = new TestAsyncCommand();
            // CancellationTokenSource cts = asyncCommand.CancellationTokenSource;
            CommandStream commandStream = new CommandStream();
            commandStream.QueueCommand(asyncCommand);
            Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.AwaitingCompletion);
            Assert.AreEqual(expected: 1, actual: commandStream.HistoryCount);
            Assert.AreEqual(expected: asyncCommand, actual: commandStream.GetCommandHistory()[0]);
            Assert.AreEqual(expected: 1, actual: commandStream.GetRunningCommandTasks().Count);
            Assert.AreEqual(expected: asyncCommand.CommandTask, actual: commandStream.GetRunningCommandTasks()[0]);

            Assert.IsNotNull(asyncCommand.CancellationTokenSource);
            commandStream.CancelRunningCommandTask(asyncCommand);
            while(!asyncCommand.CommandTask.IsCompleted) {
                yield return null;
            }
            Assert.IsTrue(asyncCommand.CommandTask.IsCanceled);
            Assert.AreEqual(expected: 0, actual: commandStream.GetRunningCommandTasks().Count);
            Assert.AreEqual(expected: 0, actual: commandStream.GetFaultedCommandTasks().Count);
            Assert.Throws<ObjectDisposedException>(() => { asyncCommand.CancellationTokenSource.Cancel(); });
        }
        [UnityTest]
        public IEnumerator AsyncCommandFaultTest()
        {
            FaultsAsyncCommand asyncCommand = new FaultsAsyncCommand();
            CancellationTokenSource cts = asyncCommand.CancellationTokenSource;
            CommandStream commandStream = new CommandStream();
            commandStream.QueueCommand(asyncCommand);
            commandStream.TryExecuteNext();
            // Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.AwaitingCompletion);
            Assert.AreEqual(expected: 1, actual: commandStream.HistoryCount);
            Assert.AreEqual(expected: asyncCommand, actual: commandStream.GetCommandHistory()[0]);
            Assert.AreEqual(expected: 1, actual: commandStream.GetRunningCommandTasks().Count);
            Assert.AreEqual(expected: asyncCommand.CommandTask, actual: commandStream.GetRunningCommandTasks()[0]);

            asyncCommand.Complete();
            while(!asyncCommand.CommandTask.IsCompleted) {
                yield return null;
            }
            Assert.IsTrue(asyncCommand.CommandTask.IsFaulted);
            Assert.AreEqual(expected: 0, actual: commandStream.GetRunningCommandTasks().Count);
            Assert.AreEqual(expected: 1, actual: commandStream.GetFaultedCommandTasks().Count);
            Assert.IsTrue(commandStream.GetFaultedCommandTasks()[asyncCommand.CommandTask] is System.Exception);
            Assert.Throws<ObjectDisposedException>(() => { asyncCommand.CancellationTokenSource.Cancel(); });
            
        }
    }
}