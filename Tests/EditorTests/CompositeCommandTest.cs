using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class CompositeCommandTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CompositeCommandTrivialTest() {
        CommandStream commandStream = new CommandStream();
        CompositeCommand testComposite = new NullCompositeCommand(20);
        commandStream.QueueCommand(testComposite);
        commandStream.TryExecuteNext();
        var commandHistory = commandStream.GetCommandHistory();
        Assert.AreEqual(expected: 1, actual: commandHistory.Count);
        Assert.AreEqual(expected: testComposite, actual: commandHistory[0]);
        Assert.AreEqual(expected: 20, actual: testComposite.ChildCount);
    }
    [Test]
    public void CompositeCommandTickerTest() {
        CommandStream commandStream = new CommandStream();
        Ticker ticker = new Ticker();
        List<Command> subCommands = new List<Command>();
        int testSize = 20;
        for (int i = 0; i < testSize; i++) {
            subCommands.Add(new TickerCommand(ticker));
        }
        CompositeCommand testComposite = new SimpleComposite(subCommands);
        commandStream.QueueCommand(testComposite);
        commandStream.TryExecuteNext();
        Assert.AreEqual(expected: 1, actual: commandStream.HistoryCount);
        Assert.AreEqual(expected: testSize, actual: ticker.count);
    }
}
