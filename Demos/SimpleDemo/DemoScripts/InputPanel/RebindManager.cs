using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class RebindManager : MonoBehaviour
    {
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button sprintButton;

        [SerializeField] private Button fireButton;
        [SerializeField] private Button altFireButton;
        [SerializeField] private Button undoButton;
        CommandStream rebindStream = new CommandStream();
        Dictionary<InputType,KeyCode> CurrentBindings { get => InputCommandStream.Instance.InputKeybinds; }

        void Start()
        {
            UpdateButtonText(upButton);
            upButton.onClick.AddListener(delegate {
                upButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                RebindKeyCommand rebindCommand = new RebindKeyCommand(CurrentBindings, InputType.MoveUp);
                rebindCommand.OnRebindFinished += delegate { UpdateButtonText(upButton); };
                rebindStream.QueueCommand(rebindCommand);
            });
            UpdateButtonText(downButton);
            downButton.onClick.AddListener(delegate {
                downButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                RebindKeyCommand rebindCommand = new RebindKeyCommand(CurrentBindings, InputType.MoveDown);
                rebindCommand.OnRebindFinished += delegate { UpdateButtonText(downButton); };
                rebindStream.QueueCommand(rebindCommand);
            });
            UpdateButtonText(leftButton);
            leftButton.onClick.AddListener(delegate {
                leftButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                RebindKeyCommand rebindCommand = new RebindKeyCommand(CurrentBindings, InputType.MoveLeft);
                rebindCommand.OnRebindFinished += delegate { UpdateButtonText(leftButton); };
                rebindStream.QueueCommand(rebindCommand);
            });
            UpdateButtonText(rightButton);
            rightButton.onClick.AddListener(delegate {
                rightButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                RebindKeyCommand rebindCommand = new RebindKeyCommand(CurrentBindings, InputType.MoveRight);
                rebindCommand.OnRebindFinished += delegate { UpdateButtonText(rightButton); };
                rebindStream.QueueCommand(rebindCommand);
            });
            UpdateButtonText(sprintButton);
            sprintButton.onClick.AddListener(delegate {
                sprintButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                RebindKeyCommand rebindCommand = new RebindKeyCommand(CurrentBindings, InputType.Sprint);
                rebindCommand.OnRebindFinished += delegate { UpdateButtonText(sprintButton); };
                rebindStream.QueueCommand(rebindCommand);
            });
            UpdateButtonText(fireButton);
            fireButton.onClick.AddListener(delegate {
                fireButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                RebindKeyCommand rebindCommand = new RebindKeyCommand(CurrentBindings, InputType.Fire);
                rebindCommand.OnRebindFinished += delegate { UpdateButtonText(fireButton); };
                rebindStream.QueueCommand(rebindCommand);
            });
            UpdateButtonText(altFireButton);
            altFireButton.onClick.AddListener(delegate {
                altFireButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                RebindKeyCommand rebindCommand = new RebindKeyCommand(CurrentBindings, InputType.AltFire);
                rebindCommand.OnRebindFinished += delegate { UpdateButtonText(altFireButton); };
                rebindStream.QueueCommand(rebindCommand);
            });
            UpdateButtonText(undoButton);
            undoButton.onClick.AddListener(delegate {
                undoButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                RebindKeyCommand rebindCommand = new RebindKeyCommand(CurrentBindings, InputType.Undo);
                rebindCommand.OnRebindFinished += delegate { UpdateButtonText(undoButton); };
                rebindStream.QueueCommand(rebindCommand);
            });
        }
        private void UpdateButtonText(Button button) {
            if (button == upButton) {
                upButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Up :{CurrentBindings[InputType.MoveUp].ToString()}";
            } else if(button == downButton) {
                downButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Down :{CurrentBindings[InputType.MoveDown].ToString()}";
            } else if(button == rightButton) {
                rightButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Right :{CurrentBindings[InputType.MoveLeft].ToString()}";
            } else if(button == leftButton) {
                leftButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Left :{CurrentBindings[InputType.MoveRight].ToString()}";
            } else if(button == sprintButton) {
                sprintButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Sprint :{CurrentBindings[InputType.Sprint].ToString()}";
            } else if(button == fireButton) {
                fireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Fire :{CurrentBindings[InputType.Fire].ToString()}";
            } else if(button == altFireButton) {
                altFireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Alt Fire :{CurrentBindings[InputType.AltFire].ToString()}";
            } else if(button == undoButton) {
                undoButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Undo :{CurrentBindings[InputType.Undo].ToString()}";
            } 
        }
        
        // Update is called once per frame
        void Update() {
            if(rebindStream.TryExecuteNext(out var topRebind)) {
            }
        }
    }
}