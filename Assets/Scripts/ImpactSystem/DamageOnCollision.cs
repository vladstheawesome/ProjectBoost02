using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.ImpactSystem
{
    public enum DamageImpact { Small, Medium, Large, Maximum }

    public class DamageOnCollision : MonoBehaviour
    {
        public DamageImpact damageImpact;        
    }
}
