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
        protected List<ICommand> subCommands = new List<ICommand>();
        /// <summary>
        /// An internal CommandStream to provide more control of the execution of the subCommands of the Composite
        /// </summary>
        protected CommandStream internalStream = new CommandStream();

        /// <summary>
        /// Adds a Command to this objects children 
        /// </summary>
        /// <param name="childCommand"> The Command to be added to the objects children </param>
        protected virtual void AddChild(ICommand childCommand) {
            subCommands.Add(childCommand);
        }

        /// <summary>
        /// Number of child Commands included in this object
        /// </summary>
        public int ChildCount { get => subCommands.Count; }
        
        /// <summary>
        /// Queues all of the child commands into the internal CommandStream and attempts to invoke all of them. Will throw an exception if one of its children fails after attempting to revert all its executed commands.
        /// 
        /// Be aware that if you override this method you will bypass the implemented failsafe's for children of the CompositeCommand failing such as attempting to undo executed commands
        /// </summary>
        /// <exception cref="IrreversibleCompositeFailureException"> Indicates one of the children of the CompositeCommand failed and it executed one or more commands that cannot be undone. TryExecuteNext will catch this exception and throw it upwards </exception>
        /// <exception cref="ReversibleCompositeFailureException"> Indicates one of the children of the CompositeCommand failed but it was able to undo all of its executed commands. TryExecuteNext will catch this exception and handle it by returning false. </exception>
        public override void Execute() {
            internalStream.QueueCommands(subCommands);
            ICommand prevChild;
            ExecuteCode executeCode = internalStream.TryExecuteNext(out prevChild);
            while (executeCode != ExecuteCode.QueueEmpty) {
                if (
                    executeCode == ExecuteCode.Failure 
                    || executeCode == ExecuteCode.CompositeFailure 
                    || executeCode == ExecuteCode.AlreadyRunning
                ) { //? One of the children failed 
                    var reversibleCommands =
                        from com in internalStream.GetCommandHistory()
                        where com is IUndoable
                        select com;
                    var irreversibleCommands =
                        from com in internalStream.GetCommandHistory()
                        where com is not IUndoable
                        select com;
                    
                    internalStream.DropQueue();
                    foreach (var command in reversibleCommands) {
                        internalStream.ForceQueueUndoCommand((IUndoable)command);
                    }
                    internalStream.ExecuteFullQueue(out var failedUndos);

                    if (irreversibleCommands.Count() == 0 && failedUndos.Count == 0) {
                        throw new ReversibleCompositeFailureException(prevChild);
                    } else {
                        throw new IrreversibleCompositeFailureException(prevChild, irreversibleCommands.ToList());
                    }
                } else {
                    executeCode = internalStream.TryExecuteNext(out prevChild);
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
        /// <summary>
        /// The child command that failed
        /// </summary>
        public readonly ICommand failedCommand;
        /// <summary>
        /// The executed commands that could not be undone
        /// </summary>
        public readonly List<ICommand> irreversibleCommands;
        public IrreversibleCompositeFailureException(ICommand failedCommand, List<ICommand> irreversibleCommands) 
        : base($"A CompositeCommand was executed however its child {failedCommand} failed, {irreversibleCommands.Count} executed children could not be undone") {
            this.failedCommand = failedCommand;
            this.irreversibleCommands = irreversibleCommands;
        }
    }
    /// <summary>
    /// An exception that indicates a CompositeCommand is executed but one of its children failed, however the composite was able to undo the commands it had executed
    /// </summary>
    [System.Serializable]
    public class ReversibleCompositeFailureException : System.Exception
    {
        /// <summary>
        /// The child command that failed
        /// </summary>
        public readonly ICommand failedCommand;
        public ReversibleCompositeFailureException(ICommand failedCommand) 
        : base($"A CompositeCommand was executed however its child {failedCommand} failed, all executed children where able to be undone") {
            this.failedCommand = failedCommand;
        }
    }
}