using ProjectBoost.FuelSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Attributes
{
    public class PlayerFuelBar : MonoBehaviour
    {
        [SerializeField] float maxFuel = 120f;
        [SerializeField] float currentFuel = 0f;

        public UiBar fuelBar;
        
        void Start()
        {
            currentFuel = maxFuel;
            fuelBar.SetMaxBarValue(maxFuel);
        }

        void Update()
        {

        }

        public void UseFuel(float usage)
        {
            currentFuel -= usage;

            fuelBar.SetBarValue(currentFuel);

            GetCurrentFuel(currentFuel);
        }

        public void ReFuel(float refuel)
        {
            currentFuel += refuel;

            if (currentFuel > maxFuel)
            {
                currentFuel = maxFuel;
            }

            fuelBar.SetBarValue(currentFuel);

            GetCurrentFuel(currentFuel);
        }

        public float GetCurrentFuel(float trackFuel)
        {
            trackFuel = currentFuel;
            return trackFuel;
        }

        public float GetMaxFuel(float trackFuel)
        {
            trackFuel = maxFuel;
            return maxFuel;
        }

        public float GetFuelValues(FuelAmount fuel)
        {
            float fuelingValue = 0f;

            if(fuel.ToString() == FuelAmount.Large.ToString())
            {
                fuelingValue = 55f;
            }
            if (fuel.ToString() == FuelAmount.Medium.ToString())
            {
                fuelingValue = 35f;
            }
            if (fuel.ToString() == FuelAmount.Small.ToString())
            {
                fuelingValue = 25f;
            }

            return fuelingValue;
        }
    }
}
