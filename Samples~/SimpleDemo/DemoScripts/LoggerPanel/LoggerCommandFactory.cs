using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class LoggerCommandFactory : MonoBehaviour
    {
        /// <summary>
        /// The current message to construct a LoggerCommand with
        /// </summary>
        private string currentMessage = "";
        /// <summary>
        /// The input field in which the message is entered
        /// </summary>
        [SerializeField] private TMP_InputField inputField;
        /// <summary>
        /// The button to create and queue a command logging the enter message as a message
        /// </summary>
        [SerializeField] private Button logMessageButton;
        /// <summary>
        /// The button to create and queue a command logging the enter message as a warning
        /// </summary>
        [SerializeField] private Button logWarningButton;
        /// <summary>
        /// The button to create and queue a command logging the enter message as an error
        /// </summary>
        [SerializeField] private Button logErrorButton;

        /// <summary>
        /// Subscribes a delegate to update currentMessage when an edit to the input field is finished
        /// as well as delegates to create and queue the relevant LoggerCommand to the ui buttons 
        /// </summary>
        void Start()
        {
            inputField.onEndEdit.AddListener(delegate {
                currentMessage = inputField.text;
            });
            logMessageButton.onClick.AddListener(delegate {
                LoggerCommandStream.Instance.QueueCommand(new LoggerCommand(LogType.log, currentMessage));
            });
            logWarningButton.onClick.AddListener(delegate { 
                LoggerCommandStream.Instance.QueueCommand(new LoggerCommand(LogType.warning, currentMessage));
            });
            logErrorButton.onClick.AddListener(delegate { 
                LoggerCommandStream.Instance.QueueCommand(new LoggerCommand(LogType.error, currentMessage));
            });
        }
    }
}