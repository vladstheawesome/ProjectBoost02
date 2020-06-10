using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBoost.Attributes
{
    // UiBar: Can be a health bar or fuel bar
    public class UiBar : MonoBehaviour
    {
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public void SetMaxBarValue(float value)
        {
            slider.maxValue = value;
            slider.value = value;

            fill.color = gradient.Evaluate(1f); // Health at max green range
        }

        public void SetBarValue(float health)
        {
            slider.value = health;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public float GetPercentageValue(float currentHealth, float defaultValue)
        {
            var valueDividedByDefaultValue = currentHealth / defaultValue;

            var valuePercentage = valueDividedByDefaultValue * 100;

            return valuePercentage;
        }
    }
}
