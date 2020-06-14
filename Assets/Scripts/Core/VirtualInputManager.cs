using ProjectBoost.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectBoost.Control
{
    public class VirtualInputManager : Singleton<VirtualInputManager>
    {
        public bool FloatThrust;
        public bool RotateForward;
        public bool RotateBackwards;
    }
}
