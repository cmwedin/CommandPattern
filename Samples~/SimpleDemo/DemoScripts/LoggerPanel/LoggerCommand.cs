using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public enum LogType {
        log,warning,error
    }
    public class LoggerCommand : Command
    {
        private LogType logType;
        private string message;

        public LoggerCommand(LogType logType, string message) {
            this.logType = logType;
            this.message = message;
        }

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