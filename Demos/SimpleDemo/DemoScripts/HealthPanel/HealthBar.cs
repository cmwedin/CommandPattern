using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        private TextMeshProUGUI barText;
        public int MaxHealth { get => maxHealth; }
        [SerializeField] private RectTransform filledBar;
        private int health;
        public int Health
        {
            get => health;
            set {
                health = Mathf.Max(Mathf.Min(value,maxHealth), 0);
                UpdateHealthBar();
            }
        }
        
        private void UpdateHealthBar() {
            barText.text = $"{Health}/{MaxHealth}";
            float filledPortion = (float)Health / (float)maxHealth;
            var barSize = GetComponent<RectTransform>().rect.width;
            var offset = barSize * (1 - filledPortion);
            filledBar.offsetMax = new Vector2(-offset, filledBar.offsetMax.y);
        }

        private void Start() {
            barText = GetComponentInChildren<TextMeshProUGUI>();
            Health = maxHealth;
        }
    }
}
