using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class LoggerCommandFactory : MonoBehaviour
    {
        private string currentMessage = "";
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button logMessageButton;
        [SerializeField] private Button logWarningButton;
        [SerializeField] private Button logErrorButton;

        // Start is called before the first frame update
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}