using ProjectBoost.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    AstronautControl astronautControl;
    public Transform Player;

    void Awake()
    {
        astronautControl = this.transform.GetComponent<AstronautControl>();
    }

    void Update()
    {
        astronautControl = Player.transform.root.GetComponent<AstronautControl>();

        if (Input.GetKey(KeyCode.Space))
        {
            VirtualInputManager.Instance.FloatThrust = true;
        }
        else
        {
            VirtualInputManager.Instance.FloatThrust = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            VirtualInputManager.Instance.RotateForward = true;
        }
        else
        {
            VirtualInputManager.Instance.RotateForward = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            VirtualInputManager.Instance.RotateBackwards = true;
        }
        else
        {
            VirtualInputManager.Instance.RotateBackwards = false;
        }
    }
}
