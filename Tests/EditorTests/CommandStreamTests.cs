using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class CommandStreamTests {
    // A Test behaves as an ordinary method
    [Test]
    public void CommandStreamTrivialTest()
    {
        int testDepth = 10;
        CommandStream commandStream = new CommandStream();
        Command[] commands = new Command[testDepth];
        for (int i = 0; i < testDepth; i++) {
            commands[i] = new NullCommand();
        }
        commandStream.QueueCommands(commands);
        int j = 0;
        while(commandStream.TryExecuteNext()) {
            Assert.AreEqual(
                expected: commands[j],
                actual: commandStream.GetCommandHistory()[j]
            );
            Assert.IsTrue(j <= testDepth);
            j++;
        }
    }
}
