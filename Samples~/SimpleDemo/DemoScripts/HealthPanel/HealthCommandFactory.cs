using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class HealthCommandFactory : MonoBehaviour
    {
        private int desiredChange;
        [Tooltip("only updates at the start of entering playmode")]
        [SerializeField] private int maximumHealthChange;
        [SerializeField] private TextMeshProUGUI minAmountDisplay;
        [SerializeField] private TextMeshProUGUI maxAmountDisplay;
        [SerializeField] private Button IncreaseButton;
        [SerializeField] private Button DecreaseButton;
        [SerializeField] private Button UndoButton;
        private HealthBar healthBar;

        private Slider valueSlider;
        public ModifyHealthCommand CreateHealthCommand(HealthBar healthBar,bool decrease = true) {
            if (decrease) {
                return new ModifyHealthCommand(-desiredChange, healthBar);
            } else {
                return new ModifyHealthCommand(desiredChange, healthBar);
            }
        }

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
        private void Update() {

        }

    }
}