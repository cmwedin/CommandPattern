using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class CommandStreamTests {
    // A Test behaves as an ordinary method
    [Test]
    public void CommandStreamTrivialTest() {
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
    [Test]
    public void CommandStreamTickerTest() {
        int testDepth = 20;
        CommandStream commandStream = new CommandStream();
        Ticker ticker = new Ticker();
        for (int i = 0; i < testDepth; i++) {
            commandStream.QueueCommand(new TickerCommand(ticker));
        }
        int j = 0;
        while (commandStream.TryExecuteNext()) {
            j++;
            Assert.AreEqual(expected: j, actual: ticker.count);
        }
    }
    [Test]
    public void CommandStreamHistoryTest(){
        int maxDepth = 100;
        for (int testDepth = 0; testDepth <= maxDepth; testDepth++)
        {
            Command[] testCommands = new Command[testDepth * 2];
            CommandStream commandStream = new CommandStream(testDepth);
            Assert.AreEqual(expected: testDepth, actual: commandStream.HistoryDepth);
            for (int i = 0; i < testDepth * 2; i++)
            {
                Command command = new NullCommand();
                commandStream.QueueCommand(command);
                testCommands[i] = command;
            }
            int j = 0;
            while (commandStream.TryExecuteNext())
            {
                if (j < commandStream.HistoryDepth)
                {
                    Assert.AreEqual(expected: j + 1, actual: commandStream.HistoryCount);
                    Assert.AreEqual(
                        expected: testCommands[0],
                        actual: commandStream.GetCommandHistory()[0]
                    );
                }
                else
                {
                    Assert.AreEqual(expected: commandStream.HistoryDepth, actual: commandStream.HistoryCount);
                    Assert.AreEqual(
                        expected: testCommands[(int)(j + 1 - commandStream.HistoryDepth)],
                        actual: commandStream.GetCommandHistory()[0]
                    );
                }
                Assert.AreEqual(
                    expected: testCommands[j],
                    actual: commandStream.GetCommandHistory()[^1]
                );
                j++;
            }
            Assert.AreEqual(expected: testDepth * 2, actual: j);
        }
    }
    [Test]
    public void EmptyQueueTest() {
        CommandStream commandStream = new CommandStream();
        var testCommand = new NullCommand();
        commandStream.QueueCommand(testCommand);
        var oldQueue = commandStream.EmptyQueue();
        Assert.IsFalse(commandStream.TryExecuteNext());
        Assert.AreEqual(expected: 1, actual: oldQueue.Count);
        Assert.AreEqual(expected: testCommand, actual: oldQueue[0]);

    }
}
