using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.ImpactSystem
{
    public enum HealingEffect { Small, Medium, Large }

    public class HealingOnCollision : MonoBehaviour
    {
        public HealingEffect healingEffect;
    }
}
