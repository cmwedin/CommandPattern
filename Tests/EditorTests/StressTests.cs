using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class StressTests_MayTakeLongToRun {

    //? We use the same command object for all tests to save memory
    Command testNullCommand = new NullCommand();
    // A Test behaves as an ordinary method
    [Test, Timeout(300000)]
    public void VeryLargeNumberOfCommandsFullHistoryTest() {
        //? Test will run out of memory if length somewhere between 2 and 3 hundred million
        int testLength = 100000000;
        CommandStream commandStream = new CommandStream();
        Debug.Log($"current test settings: execute {nameof(testNullCommand)} {testLength} times, records full history");
        for (int i = 0; i < testLength; i++) {
            commandStream.QueueCommand(testNullCommand);
        }
        commandStream.ExecuteFullQueue();
        Assert.IsTrue(commandStream.QueueEmpty);
        //? at one hundred million test takes ~10 seconds
    }
    [Test, Timeout(300000)]
    public void VeryLargeNumberOfCommandsNoHistoryTest() {
        //? Test will also run out of memory if length somewhere between 2 and 3 hundred million
        int testLength = 100000000;
        CommandStream commandStream = new CommandStream(0);
        Debug.Log($"current test settings: execute {nameof(testNullCommand)} {testLength} times, no history recorded.");
        for (int i = 0; i < testLength; i++) {
            commandStream.QueueCommand(testNullCommand);
        }
        commandStream.ExecuteFullQueue();
        Assert.IsTrue(commandStream.QueueEmpty);
        //? at one hundred million test takes ~5 seconds
    }
    [Test, Timeout(300000)]
    public void IntMaxCommandsNoHistoryInLoopExecutionTest() {
        //? As expected if the commands are executed as they are added to the queue and not stored in history we don't run out of memory and this test will run given enough time  
        int testLength = int.MaxValue;
        CommandStream commandStream = new CommandStream(0);
        Debug.Log($"current test settings: execute {nameof(testNullCommand)} {testLength} times, no history recorded, commands executed as they enter queue.");
        for (int i = 0; i < testLength; i++) {
            commandStream.QueueCommand(testNullCommand);
            commandStream.TryExecuteNext();
        }
        Assert.IsTrue(commandStream.QueueEmpty);
        //? Executes in ~100 seconds (less but that closest order of magnitude)
    }
    [Test, Timeout(30000)]
    public void VeryLargeNumberOfCommandsCappedHistoryTest() {
        //? this test has identified significant performance issues in DropHistory and must be run at a much smaller length
        //? Test will also run out of memory if length somewhere between 2 and 3 hundred million
        int testLength = 100000000;
        int historyDepth = testLength / 10;
        CommandStream commandStream = new CommandStream(historyDepth);
        Debug.Log($"current test settings: execute {nameof(testNullCommand)} {testLength} times, history recorded to a depth of {historyDepth}");
        for (int i = 0; i < testLength; i++) {
            commandStream.QueueCommand(testNullCommand);
        }
        commandStream.ExecuteFullQueue();
        Assert.IsTrue(commandStream.QueueEmpty);
        //? at one hundred million test takes ~10 seconds
    }
    [Test, Timeout(300000)]
    public void IntMaxCommandsCappedHistoryInLoopExecutionTest() {
        //? As expected if the commands are executed as they are added to the queue and a limited number are recorded in history we don't run out of memory and this test will run given enough time  
        int testLength = int.MaxValue;
        int historyDepth = 100000000;
        CommandStream commandStream = new CommandStream(historyDepth);
        Debug.Log($"current test settings: execute {nameof(testNullCommand)} {testLength} times, history recorded to a depth of {historyDepth}, commands executed as they enter queue.");
        for (int i = 0; i < testLength; i++) {
            commandStream.QueueCommand(testNullCommand);
            commandStream.TryExecuteNext();
        }
        Assert.IsTrue(commandStream.QueueEmpty);
        //? Executes in ~100 seconds (less but that closest order of magnitude)
    }
}
