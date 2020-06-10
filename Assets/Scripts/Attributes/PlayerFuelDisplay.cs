using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBoost.Attributes
{
    public class PlayerFuelDisplay : MonoBehaviour
    {
        PlayerFuelBar fuel;
        UiBar percentValue /*= new UiBar()*/;

        private float defaultValue = 120f;

        void Awake()
        {
            fuel = GameObject.FindWithTag("Player").GetComponent<PlayerFuelBar>();
            percentValue = this.GetComponentInParent<UiBar>();
        }

        void Update()
        {
            float trackFuel = 0f;
            float currentFuel = fuel.GetCurrentFuel(trackFuel);

            if (currentFuel < Mathf.Epsilon)
            {
                currentFuel = 0f; // do not display negative value
            }
            if (currentFuel > defaultValue)
            {
                currentFuel = defaultValue;
            }

            double percentage = percentValue.GetPercentageValue(currentFuel, defaultValue);

            GetComponent<Text>().text = String.Format("{0:0} %", percentage);
        }
    }
}