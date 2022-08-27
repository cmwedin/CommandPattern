# Command Pattern
This package contains a collection of classes and interfaces to allow for quick implementation of the command pattern across projects. An out-of-the-box implementation is provided through the [SingletonCommandManager](Runtime/Monobehaviours/SingletonCommandManager.cs) component, for more information see [Simple Implementation](#simple-implementation).

## Installation
To install this package in your Unity project, select the "window/Package Manager" entry in the Unity Inspector toolbar. Then, select the "+" icon in the upper left corner of the opened window, and select "Add package from git url." Paste the following:

    https://github.com/cmwedin/CommandPattern.git

Once you see this package show up in the Package Manager window, it has been successfully installed

## Using this package
Full documentation of this package can be found [here](https://cmwedin.github.io/CommandPatternDocumentation/annotated.html)
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

### Advanced Implementation

For more complicated implementation rather than simply using the SingletonCommandManager you should create your own wrapper of the [CommandStream](Runtime/Commands/CommandStream.cs) class. This class can be interacted with in many of the same ways as the SingletonCommandManager instance can, which functions my exposing many of the methods of its private CommandStream object. You can create a CommandStream using the constructor `new CommandStream([historyDepth = PositiveInfinity])` which has an optional argument to limit the depth to which the CommandStream stores its previously executed commands. The default value of this argument is `Single.PositiveInfinity` meaning that no mater how big the HistoryCount (number of recorded commands) is `HistoryCount < HistoryDepth` will always evaluate to true and the commandHistory list will never drop its oldest elements. Note that setting historyDepths to values between Int.MaxValue and Single.PositiveInfinity may cause unexpected behavior. If you want to limit the history depth at all the limit should be below Int.MaxValue, otherwise leave the constructor empty. If you set HistoryDepth to zero the process of recording commands will be skipped entirely for some slight performance enhancements.

Commands queued into the CommandStream can be executed through the CommandStream's `TryExecuteNext()` method. There is also a version of this method that returns the nextCommand through an out parameter, `TryExecuteNext(out Command topCommand)`, that is recommended for more advanced implementations (such as tracking the execution of [IUndoable](#iundoable-commands) to maintain an Undo stack). This is return an `ExecuteCode` enum value (the definition of which can be found at the top of the CommandStream.cs file) that indicates what happened when attempting to execute the next command. This enum can have the following values

- `ExecuteCode.Success` indicating the top command in the queue was executed successfully
- `ExecuteCode.Failure` indicating the top command was not executed because it was `IFailable` and `WouldFail()` returned true
- `ExecuteCode.QueueEmpty` indicates no command was execute because there where none in the command queue
- `ExecuteCode.AwaitingCompletion` indicates the top command was an AsyncCommand that is awaiting something to finish running - for more information see [AsyncCommands](#asynccommands)
- `ExecuteCode.CompositeFailure` If you see this something went wrong, IT indicates the top command was a composite that started execution but one of its children failed; however, it was able to undo its executed children otherwise the CommandStream would throw an exception. This occuring means either the CompositeCommand that was executed needs to implement `IFailable` or alternatively its `WouldFail()` method contains an error. See [this section](#a-note-on-ifailable-compositecommands) for more info.

### Example Implementation
For an example of an more advanced implementation of this packages lets consider using it for an input manager supporting button remapping. First we need to define a collection of commands we want the user to be able to preform through their inputs. These Commands might be something like, MoveLeft/Right/Up/Down, Shoot, Jump, Reload, ChangeWeapon, anything like that. Once we have them all defined, we create a lookup table in which each button allowed as a valid input is mapped to a desired Command. This table can be changed as needed to support remapping. Then, we are ready to create a InputManager component that has a CommandStream. In the components Update method whenever unity detects a input we get that input's mapped command from the lookup table and queue it into the CommandStream. Then, we execute the next command in the stream. And as simple as that we have designed our input system has been implemented. This system is also resilient to future changes, as none of your input responses have been hardcoded, showcase the power of the command pattern. If at any point we decide we want to say, replace cardinal movement in any direction, it's just a matter of switching out the appropriate commands.     

### Advanced Features

#### IUndoable Commands
Once you have these basics down you can create more advanced commands by implementing the [IUndoable](Runtime/Interfaces/IUndoable.cs) interface. This interface requires the

    public Command GetUndoCommand()
method. Which should return a Command that if executed you negate the changes cause by the Command implementing this interface's `Execute()` method. You can queue an IUndoable commands undo into a CommandStream using the `CommandStream.QueueUndoCommand(IUndoable commandToUndo)` method. This method will not let you undo a command if it does not exist it the CommandStream's history. You can bypass this restriction by using the `CommandStream.ForceUQueueUndoCommand(IUndoable commandToUndo)` method, which would be equivalent to directly calling `CommandStream.QueueCommand(commandToUndo.GetUndoCommand())`. 

#### IFailable Commands
It may be the case that a Command you need for your project could potentially fail to execute properly. In this situation you should implement the [IFailable](Runtime/Interfaces/IFailable.cs) Interface. This interface requires that your command implement the 

    public bool WouldFail()
method, which should determine if the command would be able to execute or not based on current state. When invoking `TryExecuteNext()` on a CommandStream, it will first check if the next command in it's queue is IFailable, and if so if it would fail if attempted to be executed. If it is `IFailable` and `WouldFail()` returns true, the `TryExecuteNext()` will return false and the command will be removed from the queue without being executed or recorded. When working with IFailable commands it is recommended you use the `TryExecuteNext(out Command nextCommand)` overload to be able to identify if the method returned false because nextCommand would fail, or because the Queue is empty (in which case nextCommand would be null). 

When creating Command's that implement `IFailable` it is recommended that you throw an exception at the start of the `Execute()` method of a command if `WouldFail()` is true. While this package's CommandStream should not invoke the Execute method if an `IFailable` command that would fail, doing this will provide resiliency in the case that it becomes necessary for you to develop your own Invoker to suit your project's needs. However, if the process of determining if a command would fail is expensive it may be better to trust the invoker to not invoke commands that would fail in the first place as this failsafe practice means `WouldFail()` will run twice. Alternatively an IFailable command can cache the result of `WouldFail()` in a private field to verify its validity before execution without preforming duplicate work.
    
#### CompositeCommands & SimpleComposites

Another more advanced type of command you can use is [CompositeCommands](Runtime/Commands/CompositeCommand.cs). This abstract subclass of Command encapsulates multiple commands into a single object and can be thought of as an Invoker that is also a Command (its execute method being to invoke all the Commands it has as children). When its `Execute()` method is invoked it queues all of the commands in its subCommands list into an internal CommandStream and executes commands from that CommandStream until it is empty. If the internal CommandStream ever fails to execute its next command before it is empty the CompositeCommand will throw a exception indicating that it couldn't execute all of its children. A note on overriding a CompositeCommand's execute method: this can be useful to do when implementing more complicated Composite's in which a step by step approach is needed. While it is possible to bypass the internal CommandStream and invoke each child's Execute method directly, this is not recommended as using the CommandStream gives you more control over how the subCommands are executed. As such when overriding this method the first thing it is recommended that you do is invoke `internalStream.QueueCommands(subCommands)`. After this you can execute the queue however is needed for your purpose. Keep in mind when overriding a CompositeCommand execute method it will no longer throw an exception if one of its children fail and you will need to handle this situation yourself if it is possible to occur (although if it is you should be implementing IFailable to prevent the composite from being executed in the first place). 

A [SimpleComposite](Runtime/Commands/SimpleComposite.cs) is a concrete type of CompositeCommand's that can be created from any collection of Commands which does not contain a Command that is IFailable. If the reasoning for this restriction is not obvious an explanation can be insightful into the concepts behind the command pattern.

<br/>
<details><summary>Sidebar: Why cant SimpleComposite's contain IFailable children?</summary>
A command upon execution changes some aspect of the state of its receiver object, and by extension the state of the client containing those objects. A SimpleComposite, similar to the Invoker object (in fact it may be helpful to think of a SimpleComposite as an Invoker that pretends to be a Command, indeed CompositeCommands have their own internal CommandStream), does not know the implementation details of its child objects. This means that if one of its command might fail, it can only determine if that Command would fail (as the IFailable interface exposes this information to it) based on the state if its receiver object before any of its children have been executed. It is possible that one of its children might modify the state of the receiver in such a way the failable command is no longer able to succeed. The SimpleComposite would have no way to determine if this would be the case. It could check if the IFailable child would fail after executing each of its other children; however, these Commands may not be IUndoable. This would be incompatible with a Composite executing either all of its children or none of them, as once it executes the command and realizes its IFailable child can no longer be executed, it has no way to revert the changes of the executed command. The simplest solution to this is to require that the children of a SimpleComposite never be IFailable in the first place.    
</details>
<br/>

When inheriting from CompositeCommand you can also inherit from IUndoable as well. When doing so it is best practice to ensure all of the child commands also implement IUndoable. Adhering to this practice allows the undo command of a composite command to be simply constructed with the following linq query
    
    var undoComposite = new SimpleComposite(
        from com in subCommands.Cast<IUndoable>()
        select com.GetUndoCommand()
    )

Although this assumes that none of the undo commands are IFailable. If this is not the case a more nuanced approach to constructing the Undo command will be needed.
##### A Note on IFailable CompositeCommands
While a SimpleComposite cannot contain any IFailable children, this is not necessarily true of all CompositeCommands; however, doing this requires special care to avoid introducing errors where the CompositeCommand becomes failable partway through execution. **In short, if a CompositeCommand is IFailable, any IFailable child it contains must be independent of all other children.** By Independent here we mean that the execution of one command does not interfere in any way with the execution of another. They act on different receiver objects, or they modify different aspect of the same receiver. Following this practice means that the composite can check if its IFailable children would fail and be certain that executing any one of its children will not cause that to change, therefore it will be able to execute all of them. While it may be possible to create CompositeCommands that breaks this rule without introducing errors through clever use of IUndoable commands, this is not recommended unless you are very confident in what you are doing.   

#### AsyncCommands
The final advanced topic we will cover in this readme is asynchronous commands. While implementing a command you might find that you need to wait for some over piece of code to complete before you can finish executing the command, but don't want to hang your program while you do so. In this situation it is best to use the [AsyncCommand](Runtime/Commands/AsyncCommand.cs) abstract class over the base Command abstract class. When you inherit from this class you will no longer be able to override Execute(), and instead will need to implement your command's logic in an override of the ```public abstract Task ExecuteAsync()``` method (remember to mark your override as async, that isn't part of a methods signature and can't be included in and abstract method definition). The reason for this is that Execute() return's void, and in general it is a bad idea to make a method that returns void async (if you are unclear as to why this is bad consider reviewing [best practices in asynchronous programing](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming) before implementing this type of command). Instead, when a CommandStream executes and AsyncCommand it's Execute method will invoke your override for ExecuteAsync, this will run up to your first ```await``` in the method, at which point it will return a [Task](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-6.0), representing the execution of the reminder of method after we are ready to continue. At this point, control has returned back to the Execute method even though the ExecuteAsync method has yet to fully complete, this is the point of asynchronous code. The execute method will store the Task returned by ExecuteAsync in the backing field of a publicly getable property. Then, it will continue by invoking a private method that will await the completion of said Task and invoke a ```OnTaskCompleted``` event to inform any subscribes of said event that the command has been fully executed. This method will again, upon being told to await the completion of the ExecuteAsync's return task, give control back to the Execute method. 

At this point, the Execute method will complete, and control will be returned to ```CommandStream.TryExecuteNext()```; however, you might be concerned here. We have left the AsyncCommand objects Execute method, but the command hasn't actually finished executing yet! This is why its important not to make methods that return void async, because there is important information we need in the Task returned by ExecuteAsync that if we returned void we would be discarding. When ```TryExecuteNext``` regains control, it will move on to its bookkeeping phase, where it will see that the Command it just executed is IAsyncCommand and (assuming the Task hasn't already completed) add the CommandTask to its list of currently running tasks. It will then subscribe a delegate to remove this task from the list to the AsyncCommands OnTaskCompleted event. This allows the wrapper of the CommandStream, or any other object with a reference to it, to get its list of currently running tasks and examine their status (note that until the await statement is finished the status of these tasks will be awaiting activation, they represent the remainder of ExecuteAsync after the await, not the method in its entirety).  

<br/>
<details><summary>Sidebar: Isn't it bad to invoke asynchronous methods from synchronous ones? </summary>
If you read the article on best practices in asynchronous programing linked above, or are already familiar with asynchronous programing, you may be concerned that this implementation doesn't conform to the practice listed in the article to "async all the way." If you are, that is good! It shows that you understand how async code functions which is important when working with more advanced implementation of the command pattern such as this. It is true that when possible it is best to let an asynchronous method "grow" through the codebase; however, allow me to present a couple of arguments here explaining the reasoning behind why we do not do so in this package.

<br/>

- **Compatibility with synchronous commands**
  
  We want a CommandStream to be able to execute Commands that are both synchronous and asynchronous. Because of this, the CommandStream requires objects it is given as Commands to conform to the ICommand interface, meaning they have a method with the signature ```public void Execute()```, which it will invoke when it is time to execute that object. To make ```ExecuteAsync()``` method "async all the way," we would need to it invoking method, ```Execute()``` async as well. The only way to do this while also conforming to the ICommand interface you be to make the ```Execute()``` method ```async void```, which is arguably worse practice. Alternatively, we would have to implement separate overloads for queueing AsyncCommands as well as executing them separately from synchronous commands as to truly be "async all the way" ```public bool TryExecuteNext()``` would need to become ```public async Task<bool> TryExecuteNextAsync()```. Due to this, I am currently willing to accept the partially async implementation for the time being. 

- **We aren't blocking synchronous methods based on asynchronous tasks**
  
  one of the primary reasons to allow async methods to grow throughout the codebase is not doing so can cause deadlocks to occur where the synchronous code cannot continue until the asynchronous code has run, such as waiting for its result, and the asynchronous code cannot resume while the synchronous code is blocked (this is due to more complicated aspects of how async methods resume their execution in certain environments beyond the scope of this readme but explained in the article linked above). However in our case, we are not blocking any of the synchronous methods that eventually invoke ExecuteAsync. When we reach the point in our asynchronous code where we need to await some condition TryExecuteNext will simply do some bookkeeping and return `ExecuteCode.AwaitingCompletion`, allowing the CommandStream object to continue normally. Furthermore, because the Execute method of a command represents and action with no return value, we wont encounter situations where we need to wait on a task to give us its result in synchronous code. If a situation arises where we need to prevent commands from being executed while async commands are running we can add a conditional in our CommandStream wrapper to not invoke TryExecuteNext if the runningCommandTask count isn't zero without introducing any blocking (simply by returning early or skipping the code that would invoke it.)   
</details>
<br/>
