using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBoost.Attributes
{
    public class BestTimeDisplay : MonoBehaviour
    {

        string bestTime;

        void Awake()
        {
            // TODO: value to be retrieved from stored file
            GetCurrentBestTime();
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Text>().text = string.Format("{0:0}", bestTime);
        }

        public string GetCurrentBestTime()
        {
            bestTime = "0:26.039";

            return bestTime;
        }
    }
}
