using ProjectBoost.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBoost.UIText
{
    public class Timer : MonoBehaviour
    {
        public Text currentTime;
        public Text timeDifference;
        public BestTimeDisplay bestTimeDisplay;

        private float startTime;
        private string bestTime;
        private bool finished = false;

        
        void Awake()
        {
            startTime = Time.time;
            bestTime = bestTimeDisplay.GetCurrentBestTime();
            //bestTime = "0:09.339";
        }

        void Update()
        {
            if (finished)
            {
                return;
            }

            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f3");

            currentTime.text = minutes + ":" + seconds;
        }

        public void Finished()
        {
            finished = true;

            float SecondsAndMicroSecondsBestTime = float.Parse(ExtractSecondsAndMicroSeconds(bestTime));            
            float MinutesAndSecondsBestTime = float.Parse(ExtractMinutesAndSeconds(bestTime));

            float SecondsAndMicroSecondsCurrentTime = float.Parse(ExtractSecondsAndMicroSeconds(currentTime.text));
            float MinutesAndSecondsCurrentTime = float.Parse(ExtractMinutesAndSeconds(currentTime.text));

            var timeDifferenceSecondsAndMicros = SecondsAndMicroSecondsBestTime - SecondsAndMicroSecondsCurrentTime;
            var timeDifferenceMinutesAndSeconds = MinutesAndSecondsBestTime - MinutesAndSecondsCurrentTime;

            // TODO: add minutes value           

            if (timeDifferenceSecondsAndMicros > 0 || timeDifferenceMinutesAndSeconds > 0)
            {
                // We have a lower time
                timeDifference.text = "-" + timeDifferenceSecondsAndMicros.ToString();
                timeDifference.color = Color.green;
            }
            else
            {
                // append + symbol onto the negative time (i.e worse time than current best)
                var appendPlusSymbol = timeDifferenceSecondsAndMicros.ToString().Replace("-", "+");

                timeDifference.text = appendPlusSymbol;
                timeDifference.color = Color.red;
            }

            currentTime.color = Color.yellow;
        }

        private string ExtractMinutesAndSeconds(string minutesSeconds)
        {
            var masc = minutesSeconds.Substring(0, minutesSeconds.IndexOf('.'));
            masc = masc.Replace(":", ".");

            // append 0 to a number like 0.5.00 to have 0.05.00 (to keep calculation consistency)
            var digitsInSecondsOctave = masc.Substring(masc.IndexOf(".") + 1).Length; 

            if (digitsInSecondsOctave == 1 )
            {
                masc = masc.Insert(masc.IndexOf(".") + 1, "0");
            }

            return masc;
        }

        private string ExtractSecondsAndMicroSeconds(string secondsMicroSeconds)
        {
            string smcs = secondsMicroSeconds.Substring(secondsMicroSeconds.Length - 6);

            if (smcs.Substring(0,1) == ":")
            {
                // time in seconds in '05:000' resulting in ':5:000'
                // remove that first colon and append/replace with 0
                smcs = smcs.Replace(":", "0");
            }

            return smcs;
        }
    }
}