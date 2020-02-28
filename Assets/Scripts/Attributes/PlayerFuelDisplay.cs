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

        void Awake()
        {
            fuel = GameObject.FindWithTag("Player").GetComponent<PlayerFuelBar>();
        }

        void Update()
        {
            float trackFuel = 0f;
            float currentFuel = fuel.GetCurrentFuel(trackFuel);

            if (currentFuel < Mathf.Epsilon)
            {
                currentFuel = 0f; // do not display negative value
            }
            if (currentFuel > 120)
            {
                currentFuel = 120f;
            }

            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", currentFuel, 120f);
        }
    }
}