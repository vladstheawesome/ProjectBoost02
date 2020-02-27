using ProjectBoost.ImpactSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Attributes
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100f;
        [SerializeField] float currentHealth = 0f;

        public HealthBar healthBar;

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    TakeDamage(20);
            //}
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);

            GetCurrentHealth(currentHealth);
        }

        public void GetHealing(float healing)
        {
            currentHealth += healing;
            
            // make sure health does not exceed 100f
            if(currentHealth > 100f)
            {
                currentHealth = 100f;
            }

            healthBar.SetHealth(currentHealth);

            GetCurrentHealth(currentHealth);
        }

        public float GetCurrentHealth(float trackHealth)
        {
            trackHealth = currentHealth;
            return trackHealth;
        }

        public float GetDamageValues(DamageImpact impact)
        {
            float damageValue = 0f;

            if (impact.ToString() == DamageImpact.Large.ToString())
            {
                damageValue = 60f;
            }
            if (impact.ToString() == DamageImpact.Medium.ToString())
            {
                damageValue = 40f;
            }
            if (impact.ToString() == DamageImpact.Small.ToString())
            {
                damageValue = 25f;
            }
            if (impact.ToString() == DamageImpact.Maximum.ToString())
            {
                // all non moving environments (ground / platform ) instantly kill the player
                damageValue = 100f; 
            }

            return damageValue;
        }

        public float GetHealingValues(HealingEffect healing)
        {
            float healingValue = 0f;

            if (healing.ToString() == HealingEffect.Large.ToString())
            {
                healingValue = 50f;
            }
            if (healing.ToString() == HealingEffect.Medium.ToString())
            {
                healingValue = 30f;
            }
            if (healing.ToString() == HealingEffect.Small.ToString())
            {
                healingValue = 22f;
            }

            return healingValue;
        }
    }
}
