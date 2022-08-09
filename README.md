# Command Pattern
This package contains a collection of classes and interfaces to allow for quick implementation of the command pattern across projects. An out-of-the-box implementation is provided through the [SingletonCommandManager](Runtime/Monobehaviours/SingletonCommandManager.cs) component, for more information see [Simple Implementation](#simple-implementation).

## Installation
To install this package in your Unity project, select the "window/Package Manager" entry in the Unity Inspector toolbar. Then, select the "+" icon in the upper left corner of the opened window, and select "Add package from git url." Paste the following:

    https://github.com/cmwedin/CommandPattern.git

Once you see this package show up in the Package Manager window, it has been successfully installed

## Using this package
### The "Basics"
If you are already familiar with the command pattern or don't care about the abstract design concepts behind it skip ahead to the implementation sections.

In broad strokes the command pattern is a behavioral design pattern in which objects encapsulate the performance of actions. A command is in a sense a reified (or thing-ified more casually) function call, and only necessarily has a single method among its members, Command.Execute().

More technically we can describe four distinct objects associated with the pattern. The Command, the Receiver, the Invoker, and the Client. The [Command](Runtime/Commands/Command.cs) is the fundamental unit of this pattern. A Command has a Receiver object, which during its Execute method it acts upon it in some way (such as invoking the Receiver's public method is modifying its fields). The [Invoker](Runtime/Commands/CommandStream.cs) knows how to execute command (but does not know the specific implementation of the command itself such as it's receiver) and can do bookkeeping such as tracking the history of executed commands. The client object knows about the Invoker, specific implementations of Commands, and the Receiver objects that those Commands could act on. It tells the invoker what Commands to executed and when.

For a more concrete example we can consider a toy model of object we will call tickers, these objects will be out receivers and have an int whose value can be increased or decreased. Our command objects have a ticker to modify, and an amount to modify it by, positive to increase and negative to decrease. The Invoker doesn't know or care what Tickers are or how these commands are changing them, it just knows what commands to execute. The client is less concrete and essentially the wider project, in which determine when to tell the invoker to execute commands and what those commands are, what receivers they act on and how.     

Considering this toy model further we can see an extension of this pattern by observing that given a ticker-command object, we can create a new ticker-command object that acts on the same ticker with the opposite magnitude to precisely counter the changes of the first ticker-command. More broadly we can encapsulate any command which has a separate command that is its precise opposite in the [IUndoable](Runtime/Interfaces/IUndoable.cs) Interface. In our toy example it was possible to represent a ticker-command and its opposite through the same type, but this may not be true generally, as long as they are both adhere to the command abstraction.

Another extension implemented in this library can be arrived at by considering the fact that if the Invoker does not know the details about what any given command is doing, as far as it is concerned there is no difference between executing a single command and executing a group of commands in succession. This is an example of a broader design pattern called [composition](https://en.wikipedia.org/wiki/Composite_pattern), and is reflected in the [CompositeCommand](Runtime/Commands/CompositeCommand.cs) class.   

### Simple Implementation
For the quickest Implementation of this pattern create an empty game object in your scene and attach the "SingletonCommandManager" component to it. You will be able to access this object from anywhere in your project by writing SingletonCommandManager.Instance. Then you need to define what you command objects are. To do this create a class inheriting from the Command class, implementing the abstract method. 

    public override void Execute() {...}
How exactly this class should work is entirely up to you based on the needs of your project. Once you have defined a commands, whenever you need to execute it you can pass an instance of the command object into the QueueCommand(Command command) method of your command manager instance. You can create a new instance each time you need one or if you want to optimize space you can use a look up table and pass the same object multiple times, but this may not be feasible for all use cases.

Once you have commands Queued in your command manager it will execute them every frame. This can be stopped by calling `ToggleCommandExecution()`, or `ToggleCommandExecution(bool onoff)` to set a specific value (true to start executing, false to stop).

You can access the history of executed command by through the command manager instance's `GetCommandHistory()` method. The entries at the end of the list are the most recently executed commands

Once you have these basics down you can create more advanced commands by implementing the IUndoable interface. This interface requires the

    public Command GetUndoCommand()
method. Which should return a Command that if executed you negate the changes cause by the Command implementing this interface's `Execute()` method.

Another more advanced type of command you can use is CompositeCommands. This subclass of Commands can be created from any collection of commands through the `new CompositeCommand(IEnumerable subCommands)` constructor, or by directly inheriting from it. This Command contains a collection of children and when its `Execute()` method is called it calls the `Execute()` methods of all its children in turn. When inheriting from CompositeCommand you can also inherit from IUndoable as well. When doing so it is best practice to ensure all of the child commands also implement IUndoable. Adhering to this practice allows the undo command of a composite command to be simply constructed with the following linq query
    
    var undoComposite = new CompositeCommand(
        from com in subCommands.Cast<IUndoable>()
        select com.GetUndoCommand()
    )

**--- README a WIP ---**
### Advanced Implementation