using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class StressTests {

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
        //? at one hundred million test takes ~10 seconds (little bit less but thats the closest OOM)
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
        //? at one hundred million test takes ~5 seconds
    }
}
