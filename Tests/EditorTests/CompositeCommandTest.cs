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
}
