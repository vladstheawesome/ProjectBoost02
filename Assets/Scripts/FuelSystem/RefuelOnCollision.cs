using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.FuelSystem
{
    public enum FuelAmount { Small, Medium, Large }

    public class RefuelOnCollision : MonoBehaviour
    {
        public FuelAmount fuelAmount;
    }
}
