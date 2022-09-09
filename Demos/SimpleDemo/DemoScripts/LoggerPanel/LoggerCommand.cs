using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// an enum indicating to the LoggerCommand what type of log to print
    /// </summary>
    public enum LogType {
        log,warning,error
    }
    /// <summary>
    /// A simple command that prints a message to the console
    /// </summary>
    public class LoggerCommand : Command
    {
        /// <summary>
        /// the type of log to print
        /// </summary>
        private LogType logType;
        /// <summary>
        /// the message to print
        /// </summary>
        private string message;
        /// <summary>
        /// Constructs a LoggerCommand object
        /// </summary>
        public LoggerCommand(LogType logType, string message) {
            this.logType = logType;
            this.message = message;
        }
        /// <summary>
        /// Executes the command
        /// </summary>
        public override void Execute(){
            switch (logType) {
                case LogType.log:
                    Debug.Log(message);
                    break;
                case LogType.warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.error:
                    Debug.LogError($"{ message} : unpause playmode to continue");
                    break;
                default: break;
            }
        }
    }
}