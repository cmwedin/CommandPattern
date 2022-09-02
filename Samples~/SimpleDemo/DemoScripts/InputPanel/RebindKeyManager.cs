using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// Manages the buttons for rebinding keys as well as acting as a wrapper for the CommandStream for rebinding keys
    /// </summary>
    public class RebindKeyManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of this class, only used for cancelling rebind commands if their listener is destroyed
        /// </summary>
        private static RebindKeyManager instance;
        public static RebindKeyManager Instance
        {
            get { return instance; }
            set
            {
                if (instance != null && instance != value) { Destroy(value); }
                else { instance = value; }
            }
        }
        /// <summary>
        /// sets the singleton instance
        /// </summary>
        private void Awake() {
            Instance = this;
        }

        /// <summary>
        /// The button to rebind the up input
        /// </summary>
        [SerializeField] private Button upButton;
        /// <summary>
        /// The button to rebind the down input
        /// </summary>
        [SerializeField] private Button downButton;
        /// <summary>
        /// The button to rebind the left input
        /// </summary>
        [SerializeField] private Button leftButton;
        /// <summary>
        /// The button to rebind the right input
        /// </summary>
        [SerializeField] private Button rightButton;
        /// <summary>
        /// The button to rebind the sprint input
        /// </summary>
        [SerializeField] private Button sprintButton;

        /// <summary>
        /// The button to rebind the fire input
        /// </summary>
        [SerializeField] private Button fireButton;
        /// <summary>
        /// The button to rebind the alt-fire input
        /// </summary>
        [SerializeField] private Button altFireButton;
        /// <summary>
        /// The button to rebind the undo input
        /// </summary>
        [SerializeField] private Button undoButton;

        /// <summary>
        /// A dictionary containing all of the buttons, we still need the above reference because dictionaries aren't serializable by unity
        /// </summary>
        /// <remark>
        /// To demonstrate this we have given this field the SerializeField attribute as we have for the buttons
        /// but you will notice unlike the buttons it does not show up in the inspector for this component on the RebindPanel 
        /// </remark>
        [SerializeField] private Dictionary<InputType, Button> buttons;

        /// <summary>
        /// the internal command stream for RebindKeyCommands
        /// </summary>
        CommandStream rebindStream = new CommandStream();
        /// <summary>
        /// Used by KeystrokeListener to cancel its associated RebindKeyCommand in the event it is destroyed before the command completes
        /// </summary>
        public void CancelRebindCommand(RebindKeyCommand command) {
            rebindStream.CancelRunningCommandTask(command);
        }
        /// <summary>
        /// Gets wether or not there are any RebindKeyCommand tasks currently running
        /// </summary>
        bool rebindTaskRunning { get => rebindStream.GetRunningCommandTasks().Count != 0; }
        /// <summary>
        /// QoL property to shorten accessing the InputCommandStream's input dictionary
        /// </summary>
        Dictionary<InputType,KeyCode> CurrentBindings { get => InputCommandStream.Instance.InputKeybinds; }
        Dictionary<InputType, RebindKeyCommand> RebindCommands = new Dictionary<InputType, RebindKeyCommand>();

        /// <summary>
        /// Sets up the reference for the buttons dictionary using the values assigned in the editor 
        /// Then loops through each of the input types and adds a new RebindKeyCommand for that input to the RebindCommands dictionary
        /// Then subscribes a delegate to clear and update the text for that input's button to the start and end of the RebindKeyCommand respectively
        /// And lastly subscribes a delegate to queue the command into the RebindStream once the button for its input is pressed
        /// </summary>
        void Start() {
            buttons = new Dictionary<InputType, Button> {
                {InputType.MoveUp,upButton},
                {InputType.MoveDown,downButton},
                {InputType.MoveLeft,leftButton},
                {InputType.MoveRight,rightButton},
                {InputType.Sprint, sprintButton},
                {InputType.Fire,fireButton},
                {InputType.AltFire,altFireButton},
                {InputType.Undo,undoButton}
            };
            foreach(InputType input in Enum.GetValues(typeof(InputType))) {
                UpdateButtonText(input);
                RebindCommands.Add(input, new RebindKeyCommand(input));
                RebindCommands[input].OnRebindStart += (() => { ClearButtonText(input); });
                RebindCommands[input].OnAnyTaskEnd += (() => { UpdateButtonText(input); });
                buttons[input].onClick.AddListener(() => rebindStream.QueueCommand(RebindCommands[input]));
            }
        }
        /// <summary>
        /// Updates the text on the button for the provided input to use the currently set key-binding
        /// </summary>
        /// <remark
        /// We don't just use the buttons dictionary here because we want more control over the text to use instead of just converting the input enum value to a string
        /// The same is true for the following method
        /// </remark
        /// <param name="input">The input to update the button for</param>
        private void UpdateButtonText(InputType input) {
            if (input == InputType.MoveUp) {
                upButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Up : {CurrentBindings[InputType.MoveUp].ToString()}";
            } else if(input == InputType.MoveDown) {
                downButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Down : {CurrentBindings[InputType.MoveDown].ToString()}";
            } else if(input == InputType.MoveLeft) {
                leftButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Left : {CurrentBindings[InputType.MoveLeft].ToString()}";
            } else if(input == InputType.MoveRight) {
                rightButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Right : {CurrentBindings[InputType.MoveRight].ToString()}";
            } else if(input == InputType.Sprint) {
                sprintButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Sprint : {CurrentBindings[InputType.Sprint].ToString()}";
            } else if(input == InputType.Fire) {
                fireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Fire : {CurrentBindings[InputType.Fire].ToString()}";
            } else if(input == InputType.AltFire) {
                altFireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Alt Fire : {CurrentBindings[InputType.AltFire].ToString()}";
            } else if(input == InputType.Undo) {
                undoButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Undo : {CurrentBindings[InputType.Undo].ToString()}";
            } 
        }
        /// <summary>
        /// Clears the text for the current binding of on an inputs button 
        /// </summary>
        /// <param name="input">the input to clear the text for</param>
        private void ClearButtonText(InputType input) {
            if (input == InputType.MoveUp) {
                upButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Up : ";
            } else if(input == InputType.MoveDown) {
                downButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Down : ";
            } else if(input == InputType.MoveLeft) {
                leftButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Left : ";
            } else if(input == InputType.MoveRight) {
                rightButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Right : ";
            } else if(input == InputType.Sprint) {
                sprintButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Sprint : ";
            } else if(input == InputType.Fire) {
                fireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Fire : ";
            } else if(input == InputType.AltFire) {
                altFireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Alt Fire : ";
            } else if(input == InputType.Undo) {
                undoButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Undo : ";
            } 
        }
        
        /// <summary>
        /// Executes the next command in the RebindStream given that there is not currently a rebind task running
        /// </summary>
        void Update() {
            if (!rebindTaskRunning) {
                ExecuteCode executeCode = rebindStream.TryExecuteNext();
                if (executeCode != ExecuteCode.QueueEmpty) {
                    Debug.Log($"RebindButtonManager.TryExecuteNext has finished with return code {executeCode}");
                }
            }
        }
    }
}