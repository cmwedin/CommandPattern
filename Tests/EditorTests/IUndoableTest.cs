using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    public class IUndoableTest
    {
        [Test]
        public void IUndoableTrivialTest()
        {
            Ticker ticker = new Ticker();
            CommandStream commandStream = new CommandStream();
            TickerCommand testCommand = new TickerCommand(ticker);
            commandStream.QueueCommand(testCommand);
            commandStream.TryExecuteNext();
            commandStream.TryQueueUndoCommand(testCommand);
            commandStream.TryExecuteNext();
            Assert.AreEqual(expected: 0, actual: ticker.count);
        }
        [Test]
        public void TryQueueUndoCommandTest()
        {
            NullCommand testUndoableCommand = new NullCommand();
            CommandStream commandStream = new CommandStream(1);
            CommandStream commandStreamNoHistory = new CommandStream(0);

            commandStreamNoHistory.QueueCommand(testUndoableCommand);
            Assert.IsTrue(commandStreamNoHistory.TryExecuteNext() == ExecuteCode.Success);
            Assert.IsFalse(commandStreamNoHistory.TryQueueUndoCommand(testUndoableCommand));

            Assert.IsFalse(commandStream.TryQueueUndoCommand(testUndoableCommand));
            commandStream.QueueCommand(testUndoableCommand);
            Assert.IsFalse(commandStream.TryQueueUndoCommand(testUndoableCommand));
            Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.Success);
            Assert.IsTrue(commandStream.TryQueueUndoCommand(testUndoableCommand));
            Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.Success);
            Assert.IsFalse(commandStream.TryExecuteNext() == ExecuteCode.Success);

            //? null commands are their own undo so we need to execute a filler command to push testCommand out of the streams history
            commandStream.QueueCommand(new NullCommand());
            Assert.IsTrue(commandStream.TryExecuteNext()  == ExecuteCode.Success);
            Assert.IsFalse(commandStream.TryQueueUndoCommand(testUndoableCommand));
        }

        [Test]
        public void ForceUndoCommandTest()
        {
            NullCommand testUndoableCommand = new NullCommand();
            CommandStream testStreamNoHistory = new CommandStream(0);
            Assert.IsFalse(testStreamNoHistory.TryQueueUndoCommand(testUndoableCommand));
            testStreamNoHistory.ForceQueueUndoCommand(testUndoableCommand);
            Assert.IsFalse(testStreamNoHistory.QueueEmpty);
            Assert.IsTrue(testStreamNoHistory.TryExecuteNext(out var executedCommand)  == ExecuteCode.Success);
            Assert.AreEqual(
                expected: testUndoableCommand.GetUndoCommand(),
                actual: executedCommand
            );
        }
    }
}