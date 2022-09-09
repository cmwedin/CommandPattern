using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// Simulates a GameObject implementing IHealth
    /// </summary>
    public class HealthBar : MonoBehaviour, IHealth
    {
        /// <summary>
        /// The maximum health of the HealthBar
        /// </summary>
        [SerializeField] private int maxHealth;
        /// <summary>
        /// The text displaying the what the current health is
        /// </summary>
        private TextMeshProUGUI barText;
        /// <summary>
        /// Property to provide public access to the value of maxHealth
        /// </summary>
        public int MaxHealth { get => maxHealth; }
        /// <summary>
        /// The RectTransform of the green part of the health bar
        /// </summary>
        [SerializeField] private RectTransform filledBar;
        /// <summary>
        /// The current health value
        /// </summary>
        private int health;
        /// <summary>
        /// An event invoked when health is changed, used to update the ui
        /// </summary>
        public event Action onHealthChanged;
        /// <summary>
        /// A property to modify health but restrict changes to between 0 and maximum health an invoke the OnHealthChanged event
        /// </summary>
        public int Health
        {
            get => health;
            set {
                health = Mathf.Clamp(value,0,maxHealth);
                onHealthChanged?.Invoke();
            }
        }
        /// <summary>
        /// Updates the UI so the green portion of the health bar fills the appropriate amount based on the current value of health
        /// </summary>
        private void UpdateHealthBar() {
            barText.text = $"{Health}/{MaxHealth}";
            float filledPortion = (float)Health / (float)maxHealth;
            var barSize = GetComponent<RectTransform>().rect.width;
            var offset = barSize * (1 - filledPortion);
            filledBar.offsetMax = new Vector2(-offset, filledBar.offsetMax.y);
        }
        /// <summary>
        /// subscribes UpdateHealthBar to onHealthChanged, gets the text component for barText, and sets Health to maxHealth
        /// </summary>
        private void Start() {
            onHealthChanged += UpdateHealthBar;
            barText = GetComponentInChildren<TextMeshProUGUI>();
            Health = maxHealth;
        }
    }
}
