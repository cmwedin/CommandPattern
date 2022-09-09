using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// Simulates the client sending commands to the CommandStream wrapper
    /// </summary>
    public class HealthCommandFactory : MonoBehaviour
    {
        /// <summary>
        /// The currently set amount to change the health bar by
        /// </summary>
        private int desiredChange;
        /// <summary>
        /// The maximum amount to change the health bar by
        /// </summary>
        [SerializeField,Tooltip("only updates at the start of entering playmode")] private int maximumHealthChange;
        /// <summary>
        /// The display for the minimum change
        /// </summary>
        [SerializeField] private TextMeshProUGUI minAmountDisplay;
        /// <summary>
        /// The display for the maximum change
        /// </summary>
        [SerializeField] private TextMeshProUGUI maxAmountDisplay;
        /// <summary>
        /// The button to send a command to increase health
        /// </summary>
        [SerializeField] private Button IncreaseButton;
        /// <summary>
        /// The button to send a command to decrease health
        /// </summary>
        [SerializeField] private Button DecreaseButton;
        /// <summary>
        /// The button to undo the last command
        /// </summary>
        [SerializeField] private Button UndoButton;
        /// <summary>
        /// the health bar we are modifying
        /// </summary>
        private HealthBar healthBar;
        /// <summary>
        /// The slider to adjust the desired change
        /// </summary>
        private Slider valueSlider;
        /// <summary>
        /// Creates a ModifyHealthCommand using the current desired change
        /// </summary>
        /// <param name="healthBar">The health bar to modify</param>
        /// <param name="decrease">Wether the command should increase or decrease the health bar by desired change</param>
        /// <returns>the requested command</returns>
        public ModifyHealthCommand CreateHealthCommand(HealthBar healthBar,bool decrease = true) {
            if (decrease) {
                return new ModifyHealthCommand(-desiredChange, healthBar);
            } else {
                return new ModifyHealthCommand(desiredChange, healthBar);
            }
        }
        /// <summary>
        /// Sets the references of any objects not set in the inspector and the test on the value slider. 
        /// Subscribes a delegate to set the value of desiredChange to the value of the Slider when its value changes.
        /// Subscribes delegates to queue the relevant commands to the increase/decrease/undo buttons
        /// </summary>
        private void Start() {
            valueSlider = GetComponentInChildren<Slider>();
            valueSlider.maxValue = maximumHealthChange;
            minAmountDisplay.text = valueSlider.minValue.ToString();
            maxAmountDisplay.text = valueSlider.maxValue.ToString();
            desiredChange = (int)valueSlider.value;
            valueSlider.onValueChanged.AddListener(delegate {desiredChange = (int)valueSlider.value;});

            healthBar = GetComponentInChildren<HealthBar>();
            IncreaseButton?.onClick.AddListener(delegate { HealthCommandStream.Instance.QueueHealthCommand(CreateHealthCommand(healthBar, false)); });
            DecreaseButton?.onClick.AddListener(delegate { HealthCommandStream.Instance.QueueHealthCommand(CreateHealthCommand(healthBar, true)); });
            UndoButton?.onClick.AddListener(delegate { HealthCommandStream.Instance.UndoPrev(); });
        }
    }
}