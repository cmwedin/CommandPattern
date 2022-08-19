using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SadSapphicGames.CommandPattern {
    /// <summary>
    /// A Command that is composed of multiple child commands, all of which are executed together and leave one record in the CommandStream's history. 
    /// <remark> For more information on this type of object seek external documentation on the composite design pattern </remark>
    /// </summary>
    public abstract class CompositeCommand : Command {
        /// <summary>
        /// The child Commands that will be executed upon executing this object
        /// </summary>
        protected List<Command> subCommands = new List<Command>();
        /// <summary>
        /// An internal CommandStream to provide more control of the execution of the subCommands of the Composite
        /// </summary>
        protected CommandStream internalStream = new CommandStream();

        /// <summary>
        /// Adds a Command to this objects children 
        /// </summary>
        /// <param name="childCommand"> The Command to be added to the objects children </param>
        protected virtual void AddChild(Command childCommand) {
            subCommands.Add(childCommand);
        }

        /// <summary>
        /// Number of child Commands included in this object
        /// </summary>
        public int ChildCount { get => subCommands.Count; }
        
        /// <summary>
        /// Queues all of the child commands into the internal CommandStream and attempts to invoke all of them. Will throw an exception if one of its children fails.
        /// 
        /// Be aware that if you override this method you will bypass the implemented failsafe's for children of the CompositeCommand failing such as attempting to undo executed commands
        /// </summary>
        /// <exception cref="IrreversibleCompositeFailureException"> Indicates one of the children of the CompositeCommand failed and its executed commands cannot be undone. TryExecuteNext will catch this exception and throw it upwards </exception>
        /// <exception cref="ReversibleCompositeCommandException"> Indicates one of the children of the CompositeCommand failed but it was able to undo all of its executed commands. TryExecuteNext will catch this exception and handle it by returning false. </exception>
        public override void Execute() {
            internalStream.QueueCommands(subCommands);
            Command prevChild;
            while(internalStream.TryExecuteNext(out prevChild)) {}
            if(prevChild != null) { //? One of the children failed 
                var canUndo =
                    from com in internalStream.GetCommandHistory()
                    select com is IUndoable;
                if(canUndo.Contains(false)) {
                    throw new IrreversibleCompositeFailureException(prevChild);
                } else {
                    internalStream.DropQueue();
                    foreach (var command in internalStream.GetCommandHistory()) {
                        internalStream.ForceQueueUndoCommand((IUndoable)command);
                    }
                    internalStream.ExecuteFullQueue();
                    throw new ReversibleCompositeCommandException(prevChild);
                }
            }
        }
    }
    /// <summary>
    /// An exception that indicates a CompositeCommand is executed but one of its children failed and the composite cannot undo its executed commands
    /// </summary>
    [System.Serializable]
    public class IrreversibleCompositeFailureException : System.Exception
    {
        public IrreversibleCompositeFailureException(Command failedCommand) : base($"A CompositeCommand was executed however its child {failedCommand} failed and the composite could not undo its executed commands") { }
        public IrreversibleCompositeFailureException(string message) : base(message) { }
        public IrreversibleCompositeFailureException(string message, System.Exception inner) : base(message, inner) { }
        protected IrreversibleCompositeFailureException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// An exception that indicates a CompositeCommand is executed but one of its children failed, however the composite was able to undo the commands it had executed
    /// </summary>
    [System.Serializable]
    public class ReversibleCompositeCommandException : System.Exception
    {
        public ReversibleCompositeCommandException(Command failedCommand) : base($"A CompositeCommand was executed however its child {failedCommand} failed, the executed commands where able to be undone") { }
        public ReversibleCompositeCommandException(string message) : base(message) { }
        public ReversibleCompositeCommandException(string message, System.Exception inner) : base(message, inner) { }
        protected ReversibleCompositeCommandException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}