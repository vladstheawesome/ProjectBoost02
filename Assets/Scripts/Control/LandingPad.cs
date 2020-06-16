using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Control
{
    public class LandingPad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            print("Finished");
            GameObject.Find("Player").SendMessage("Finished");
        }
    }
}
