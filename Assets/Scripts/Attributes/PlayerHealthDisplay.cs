using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBoost.Attributes
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        PlayerHealthBar health;
        UiBar percentValue /*= new UiBar()*/;

        private float defaultValue = 100f;

        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<PlayerHealthBar>();
            percentValue = this.GetComponentInParent<UiBar>();
        }

        void Update()
        {
            float trackHealth = 0f;
            float currentHealth = health.GetCurrentHealth(trackHealth);

            if (currentHealth < Mathf.Epsilon)
            {
                currentHealth = 0f; // do not display negative value
            }
            if (currentHealth > defaultValue)
            {
                currentHealth = defaultValue;
            }

            double percentage = percentValue.GetPercentageValue(currentHealth, defaultValue);

            GetComponent<Text>().text = String.Format("{0:0} %", percentage);            
        }
    }
}
