using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class IUndoableTest
{
    [Test]
    public void IUndoableTrivialTest() {
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
    public void TryQueueUndoCommandTest() {
        NullCommand testCommand = new NullCommand();
        CommandStream commandStream = new CommandStream(1);
        CommandStream commandStreamNoHistory = new CommandStream(0);

        commandStreamNoHistory.QueueCommand(testCommand);
        Assert.IsTrue(commandStreamNoHistory.TryExecuteNext());
        //? Verify a CommandStream that doesnt record history will not let you use TryQueueUndo
        Assert.IsFalse(commandStreamNoHistory.TryQueueUndoCommand(testCommand));

        //? Verify if a CommandStream does not have a command anywhere it will not let you undo it
        Assert.IsFalse(commandStream.TryQueueUndoCommand(testCommand));
        commandStream.QueueCommand(testCommand);
        //? Verify if a CommandStream has a command in its queue it will not let you undo it
        Assert.IsFalse(commandStream.TryQueueUndoCommand(testCommand));
        Assert.IsTrue(commandStream.TryExecuteNext());
        //? Verify if a CommandStream records a command in its history it will let you undo it
        Assert.IsTrue(commandStream.TryQueueUndoCommand(testCommand));
        //? Since the undo command was queued it should be able to be executed
        Assert.IsTrue(commandStream.TryExecuteNext());
        //? but after that the queue should be empty
        Assert.IsFalse(commandStream.TryExecuteNext());

        //? null commands are their own undo so we need to execute a filler command to push testCommand out of the streams history
        commandStream.QueueCommand(new NullCommand());
        Assert.IsTrue(commandStream.TryExecuteNext());
        //? Verify if a command has been pushed out of a CommandStreams history it will not let you undo it
        Assert.IsFalse(commandStream.TryQueueUndoCommand(testCommand));
    }
}
