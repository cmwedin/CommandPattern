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
        [SerializeField] private Button fireButton;
        [SerializeField] private Button altFireButton;
        [SerializeField] private Button undoButton;

        // Start is called before the first frame update
        void Start()
        {
            UpdateButtonText(upButton);
            upButton.onClick.AddListener(async delegate {
                upButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                await InputCommandStream.Instance.RebindKey(InputCommandStream.Instance.upKeyCode);
                UpdateButtonText(upButton);
            });
            UpdateButtonText(downButton);
            downButton.onClick.AddListener(async delegate {
                downButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                await InputCommandStream.Instance.RebindKey(InputCommandStream.Instance.downKeyCode);
                UpdateButtonText(downButton);
            });
            UpdateButtonText(leftButton);
            leftButton.onClick.AddListener(async delegate {
                leftButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                await InputCommandStream.Instance.RebindKey(InputCommandStream.Instance.leftKeyCode);
                UpdateButtonText(leftButton);
            });
            UpdateButtonText(rightButton);
            rightButton.onClick.AddListener(async delegate {
                rightButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                await InputCommandStream.Instance.RebindKey(InputCommandStream.Instance.rightKeyCode);
                UpdateButtonText(rightButton);
            });
            UpdateButtonText(fireButton);
            fireButton.onClick.AddListener(async delegate {
                fireButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                await InputCommandStream.Instance.RebindKey(InputCommandStream.Instance.fireKeyCode);
                UpdateButtonText(fireButton);
            });
            UpdateButtonText(altFireButton);
            altFireButton.onClick.AddListener(async delegate {
                altFireButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                await InputCommandStream.Instance.RebindKey(InputCommandStream.Instance.altFireKeyCode);
                UpdateButtonText(altFireButton);
            });
            UpdateButtonText(undoButton);
            undoButton.onClick.AddListener(async delegate {
                undoButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
                await InputCommandStream.Instance.RebindKey(InputCommandStream.Instance.undoKeyCode);
                UpdateButtonText(undoButton);
            });
        }
        private void UpdateButtonText(Button button) {
            if(button == upButton) {
                upButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Up :{InputCommandStream.Instance.upKeyCode.ToString()}";
            } else if(button == downButton) {
                downButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Down :{InputCommandStream.Instance.downKeyCode.ToString()}";
            } else if(button == rightButton) {
                rightButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Right :{InputCommandStream.Instance.rightKeyCode.ToString()}";
            } else if(button == leftButton) {
                leftButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Left :{InputCommandStream.Instance.leftKeyCode.ToString()}";
            } else if(button == fireButton) {
                fireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Fire :{InputCommandStream.Instance.fireKeyCode.ToString()}";
            } else if(button == altFireButton) {
                altFireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Alt Fire :{InputCommandStream.Instance.altFireKeyCode.ToString()}";
            } else if(button == undoButton) {
                undoButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Undo :{InputCommandStream.Instance.undoKeyCode.ToString()}";
            } 
        }
        
        // Update is called once per frame
        void Update()
        {

        }
    }
}