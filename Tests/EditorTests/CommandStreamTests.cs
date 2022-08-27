using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    public class CommandStreamTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CommandStreamTrivialTest()
        {
            int testDepth = 10;
            CommandStream commandStream = new CommandStream();
            Command[] commands = new Command[testDepth];
            for (int i = 0; i < testDepth; i++)
            {
                commands[i] = new NullCommand();
            }
            commandStream.QueueCommands(commands);
            Assert.AreEqual(expected: testDepth, actual: commandStream.QueueCount);
            int j = 0;
            while (commandStream.TryExecuteNext() == ExecuteCode.Success)
            {
                Assert.AreEqual(
                    expected: commands[j],
                    actual: commandStream.GetCommandHistory()[j]
                );
                Assert.IsTrue(j <= testDepth);
                j++;
            }
            Assert.IsTrue(commandStream.QueueEmpty);
        }
        [Test]
        public void CommandStreamTickerTest()
        {
            int testDepth = 20;
            CommandStream commandStream = new CommandStream();
            Ticker ticker = new Ticker();
            for (int i = 0; i < testDepth; i++)
            {
                commandStream.QueueCommand(new TickerCommand(ticker));
            }
            int j = 0;
            while (commandStream.TryExecuteNext() == ExecuteCode.Success)
            {
                j++;
                Assert.AreEqual(expected: j, actual: ticker.count);
            }
        }
        [Test]
        public void CommandStreamHistoryTest()
        {
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
                while (commandStream.TryExecuteNext() == ExecuteCode.Success)
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
        public void CommandStreamNoHistoryTest()
        {
            const int testLength = 10;
            CommandStream commandStream = new CommandStream(0);
            for (int i = 0; i < testLength; i++)
            {
                commandStream.QueueCommand(new NullCommand());
                Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.Success);
            }
            Assert.IsNull(commandStream.GetCommandHistory());
        }
        [Test]
        public void DropQueueTest()
        {
            CommandStream commandStream = new CommandStream();
            var testCommand = new NullCommand();
            commandStream.QueueCommand(testCommand);
            var oldQueue = commandStream.DropQueue();
            Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.QueueEmpty);
            Assert.AreEqual(expected: 1, actual: oldQueue.Count);
            Assert.AreEqual(expected: testCommand, actual: oldQueue[0]);

        }
        [Test]
        public void DropHistoryTest()
        {
            CommandStream commandStream = new CommandStream();
            var testCommand = new NullCommand();
            commandStream.QueueCommand(testCommand);
            Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.Success);
            var oldHistory = commandStream.DropHistory();
            Assert.AreEqual(expected: 0, actual: commandStream.HistoryCount);
            Assert.AreEqual(expected: 1, actual: oldHistory.Count);
            Assert.AreEqual(expected: testCommand, actual: oldHistory[0]);

        }
        [Test]
        public void ExecuteFullQueueNoFailureTest()
        {
            int testLength = 10;
            CommandStream commandStream = new CommandStream();
            for (int i = 0; i < testLength; i++)
            {
                commandStream.QueueCommand(new NullCommand());
            }
            commandStream.ExecuteFullQueue();
            Assert.AreEqual(expected: 0, actual: commandStream.GetCommandQueue().Count);
            Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.QueueEmpty);
        }
        [Test]
        public void ExecuteFullQueueWithFailuresTest()
        {
            int testLength = 10;
            CommandStream commandStream = new CommandStream();
            Command[] IFailableCommands = new Command[testLength / 2];
            int j = 0;
            for (int i = 0; i < testLength; i++)
            {
                if (i % 2 == 0)
                {
                    commandStream.QueueCommand(new NullCommand());
                }
                else
                {
                    Command testFailable = new AlwaysFailsCommand();
                    commandStream.QueueCommand(testFailable);
                    IFailableCommands[j] = testFailable;
                    j++;
                }
            }
            commandStream.ExecuteFullQueue(out var failedCommands);
            Assert.AreEqual(expected: 0, actual: commandStream.GetCommandQueue().Count);
            Assert.IsTrue(commandStream.TryExecuteNext() == ExecuteCode.QueueEmpty);

            Assert.AreEqual(expected: j, actual: failedCommands.Count);
            for (int i = 0; i < j; i++)
            {
                Assert.AreEqual(
                    expected: IFailableCommands[i],
                    actual: failedCommands[i]
                );
            }
        }
    }
}