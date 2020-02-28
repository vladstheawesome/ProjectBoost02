using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBoost.Attributes
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        PlayerHealthBar health;

        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<PlayerHealthBar>();
        }

        void Update()
        {
            float trackHealth = 0f;
            float currentHealth = health.GetCurrentHealth(trackHealth);

            if (currentHealth < Mathf.Epsilon)
            {
                currentHealth = 0f; // do not display negative value
            }
            if (currentHealth > 100)
            {
                currentHealth = 100f;
            }
            
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", currentHealth, 100f);
        }
    }
}
