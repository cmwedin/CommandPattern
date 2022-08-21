using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class HealthCommandFactory : MonoBehaviour
    {
        public static HealthCommandFactory Instance { get; private set; }
        private int desiredChange;
        [SerializeField] private int maximumHealthChange;
        private Slider valueSlider;
        public Command CreateHealthCommand(HealthBar healthBar,bool decrease = true) {
            if (decrease) {
                return new ModifyHealthCommand(-desiredChange, healthBar);
            } else {
                return new ModifyHealthCommand(desiredChange, healthBar);
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start() {
            valueSlider = GetComponentInChildren<Slider>();
            valueSlider.maxValue = maximumHealthChange;
            desiredChange = (int)valueSlider.value;
            valueSlider.onValueChanged.AddListener(delegate {desiredChange = (int)valueSlider.value;});
        }
        private void Update() {
            valueSlider.maxValue = maximumHealthChange;
        }

    }
}