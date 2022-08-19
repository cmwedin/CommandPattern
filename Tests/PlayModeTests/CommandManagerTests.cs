using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CommandPattern;

public class CommandManagerTests
{
    private GameObject managerObject;
    private SingletonCommandManager CommandManagerInstance { get => SingletonCommandManager.Instance; }

    [SetUp]
    public void SetUp() {
        managerObject = GameObject.Instantiate(new GameObject());
        managerObject.AddComponent<SingletonCommandManager>();
    }
    // A Test behaves as an ordinary method
    [Test]
    public void CommandManagerInstanceTest() {
        Assert.IsNotNull(CommandManagerInstance);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CommandManagerExecutesCommandsTest()
    {
        CommandManagerInstance.QueueCommand(new NullCommand());
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Assert.AreEqual(
            expected: 1,
            actual: CommandManagerInstance.HistoryCount
        );
    }
    [UnityTest]
    public IEnumerator CommandManagerFreezeExecutionTest()
    {
        CommandManagerInstance.ToggleCommandExecution(false);
        CommandManagerInstance.QueueCommand(new NullCommand());
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Assert.IsFalse(CommandManagerInstance.QueueEmpty);
        CommandManagerInstance.ToggleCommandExecution(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(CommandManagerInstance.QueueEmpty);
        Assert.AreEqual(
            expected: 1,
            actual: CommandManagerInstance.HistoryCount
        );
    }
}
