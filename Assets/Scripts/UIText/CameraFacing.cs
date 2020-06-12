using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.UIText
{
    public class CameraFacing : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
