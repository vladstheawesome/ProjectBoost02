using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Control
{
    public class ManualInput : MonoBehaviour
    {
        private AstronautControl astronautControl;

        void Awake()
        {
            astronautControl = this.gameObject.GetComponent<AstronautControl>();
        }

        // Update is called once per frame
        void Update()
        {
            if (VirtualInputManager.Instance.FloatThrust)
            {
                astronautControl.FloatThrust = true;
            }
            else
            {
                astronautControl.FloatThrust = false;
            }

            if (VirtualInputManager.Instance.RotateForward)
            {
                astronautControl.RotateForward = true;
            }
            else
            {
                astronautControl.RotateForward = false;
            }

            if (VirtualInputManager.Instance.RotateBackwards)
            {
                astronautControl.RotateBackwards = true;
            }
            else
            {
                astronautControl.RotateBackwards = false;
            }
        }
    }
}
