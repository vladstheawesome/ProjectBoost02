using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBoost.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public void SetMaxHealth(float health)
        {
            slider.maxValue = health;
            slider.value = health;

            fill.color = gradient.Evaluate(1f); // Health at max green range
        }

        public void SetHealth(float health)
        {
            slider.value = health;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
