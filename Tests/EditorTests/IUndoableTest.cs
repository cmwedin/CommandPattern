using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class IUndoableTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void IUndoableTrivialTest() {
        Ticker ticker = new Ticker();
        CommandStream commandStream = new CommandStream();
        TickerCommand testCommand = new TickerCommand(ticker);
        commandStream.QueueCommand(testCommand);
        commandStream.TryExecuteNext();
        commandStream.QueueUndoCommand(testCommand);
        commandStream.TryExecuteNext();
        Assert.AreEqual(expected: 0, actual: ticker.count);
    }
}
