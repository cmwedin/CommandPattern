using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class IFailableTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void IFailableTrivialTest() {
        CommandStream commandStream = new CommandStream();
        commandStream.QueueCommand(new AlwaysFailsCommand());
        
        Assert.IsFalse(commandStream.TryExecuteNext(out Command nextCommand));
        Assert.IsNotNull(nextCommand);
        Assert.AreEqual(expected: 0, actual: commandStream.HistoryCount);
    }
    [Test]
    public void CompositeFailureTest() {
        CommandStream commandStream = new CommandStream();
        CompositeCommand testComposite = new CompositeCommand(new List<Command> {
            new NullCommand(),
            new AlwaysFailsCommand()
        });
        commandStream.QueueCommand(testComposite);
        
        Assert.IsFalse(commandStream.TryExecuteNext(out Command nextCommand));
        Assert.IsNotNull(nextCommand);
        Assert.AreEqual(expected: 0, actual: commandStream.HistoryCount);
    }
}
