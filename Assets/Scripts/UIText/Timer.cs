using ProjectBoost.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBoost.UIText
{
    public class Timer : MonoBehaviour
    {
        public Canvas successMenu;

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

            var minuteValue = MinuteValue(SecondsAndMicroSecondsBestTime, MinutesAndSecondsBestTime, SecondsAndMicroSecondsCurrentTime, MinutesAndSecondsCurrentTime);

            var secondsValue = ExtractSecondsOnly(timeDifferenceSecondsAndMicros);

            var milliSeconds = ExtractMicroSecondsOnly(timeDifferenceSecondsAndMicros);

            if (timeDifferenceSecondsAndMicros > 0 || timeDifferenceMinutesAndSeconds > 0)
            {
                // We beat the best time
                timeDifference.text = "-"+ minuteValue + ":" + secondsValue + "." + milliSeconds;
                timeDifference.color = Color.green;
            }
            else
            {
                timeDifference.text = "+" + minuteValue + ":" + secondsValue + "." + milliSeconds;
                timeDifference.color = Color.red;
            }

            currentTime.color = Color.yellow;

            // Load Success Menu
            Canvas instantiate_ = Instantiate(successMenu) as Canvas;
         }

        private string ExtractMicroSecondsOnly(float timeDifferenceSecondsAndMicros)
        {
            var tdSecondsMicros = timeDifferenceSecondsAndMicros.ToString();

            tdSecondsMicros = tdSecondsMicros.Substring(tdSecondsMicros.IndexOf(".") + 1);
            
            if (tdSecondsMicros.Length > 3)
            {
                var roundedtdSecondsMicros = tdSecondsMicros.Substring(0, 3);
                return roundedtdSecondsMicros;
            }

            return tdSecondsMicros;
        }

        private string ExtractSecondsOnly(float timeDifferenceSecondsAndMicros)
        {
            var tdSecondsMicros = timeDifferenceSecondsAndMicros.ToString();

            tdSecondsMicros = tdSecondsMicros.Substring(0, tdSecondsMicros.IndexOf("."));
            
            if (tdSecondsMicros.Substring(0,1) == "-")
            {
                tdSecondsMicros = tdSecondsMicros.Replace("-", string.Empty);
            }

            if (tdSecondsMicros.Length == 1)
            {
                tdSecondsMicros = "0" + tdSecondsMicros;
            }

              return tdSecondsMicros;
        }

        private string MinuteValue(float SecondsAndMicroSecondsBestTime, float MinutesAndSecondsBestTime, float SecondsAndMicroSecondsCurrentTime, float MinutesAndSecondsCurrentTime)
        {
            string secondsOnlyBestTime = ExtractMinutesAndSeconds(SecondsAndMicroSecondsBestTime.ToString());
            string minutesOnlyBestTime = ExtractMinutesAndSeconds(MinutesAndSecondsBestTime.ToString());

            string secondsOnlyCurrentTime = ExtractMinutesAndSeconds(SecondsAndMicroSecondsCurrentTime.ToString());
            string minutesOnlyCurrentTime = ExtractMinutesAndSeconds(MinutesAndSecondsCurrentTime.ToString());

            float newMinuteValue = 0f;

            //if (float.Parse(secondsOnlyCurrentTime) > float.Parse(secondsOnlyBestTime))
            //{
            //    // Carry the 1
            //    newMinuteValue = (1f + float.Parse(minutesOnlyBestTime)) - float.Parse(minutesOnlyCurrentTime);
            //}
            //else
            //{
                newMinuteValue = (float.Parse(minutesOnlyBestTime)) - float.Parse(minutesOnlyCurrentTime);
            //}

            string stringValue = newMinuteValue.ToString();

            if (stringValue.Length == 1)
            {
                stringValue = "0" + stringValue;
            }
            //var digitsInMinute = stringValue.Substring(stringValue.IndexOf)

            return stringValue;
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