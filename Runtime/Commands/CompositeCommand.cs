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
        /// Executes each of the commands included in this object. 
        /// <remark> Default implementation queues all subCommands into the internalStream and executes them until it is empty.</remark>
        /// </summary>
        /// <exception cref="CompositeFailureException"> Indicates one of the commands children failed, If this is possible you should be implementing IFailable. </exception>
        public override void Execute() {
            internalStream.QueueCommands(subCommands);
            Command prevChild;
            while(internalStream.TryExecuteNext(out prevChild)){}
            if(prevChild != null) throw new CompositeFailureException(prevChild);
        }
    }
    /// <summary>
    /// An exception that is thrown when a CompositeCommand is executed but one of its children fails
    /// </summary>
    [System.Serializable]
    public class CompositeFailureException : System.Exception
    {
        public CompositeFailureException(Command failedCommand) : base($"A CompositeCommand was executed however its child {failedCommand} failed. If you are seeing this your composite needs to implement IFailable or its implementation of IFailable.WouldFail contains an error.") { }
        public CompositeFailureException(string message) : base(message) { }
        public CompositeFailureException(string message, System.Exception inner) : base(message, inner) { }
        protected CompositeFailureException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}