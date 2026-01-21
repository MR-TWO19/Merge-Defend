using System;
using TMPro;
using UnityEngine;

namespace TwoCore
{
    [RequireComponent(typeof(TickInterval))]
    public class UICountDownTimeV2 : MonoBehaviour
    {
        public TextMeshProUGUI timeText;
        public string daysFormat = @"d\d\ hh\:mm\:ss";
        public string hoursFormat = @"hh\:mm";
        public string minutesFormat = @"mm\:ss";
        public string secondsFormat = @"mm\:ss";

        private TickInterval _tickInterval;
        public TickInterval TickInterval { get { if (_tickInterval == null) _tickInterval = GetComponent<TickInterval>(); return _tickInterval; } }

        bool counting;
        float seconds = 0;
        public float Seconds => seconds;

        public event Action OnEndCountdown = delegate { };

        private void OnEnable() => TickInterval.onTick += OnTick;
        private void OnDisable() => TickInterval.onTick -= OnTick;

        void Reset()
        {
            if (!timeText)
            {
                timeText = GetComponent<TextMeshProUGUI>();
                gameObject.name = "UICountDownTime";
            }
        }

        public void StartWithDuration(float seconds)
        {
            if (seconds <= 0) return;

            counting = true;
            this.seconds = Mathf.Max(0, seconds);
            _UpdateUI(seconds);
            TickInterval.Restart();
        }

        protected virtual string ToTimeFormat(float seconds)
        {
            return Helper.ToHourFormat(Mathf.RoundToInt(this.seconds));
        }

        void OnTick()
        {
            if (seconds > 0)
            {
                seconds = Mathf.Max(0, seconds - TickInterval.TimeEslaped);
                _UpdateUI(seconds);
            }
            else if (counting)
            {
                counting = false;
                _UpdateUI(0);
                OnEndCountdown();
            }
        }

        private void _UpdateUI(float seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            string text;
            if (timeSpan.Days > 0)
            {
                text = timeSpan.ToString(daysFormat);
            }
            else if (seconds > 3600)
            {
                text = timeSpan.ToString(hoursFormat);
            }
            else if (seconds > 60)
            {
                text = timeSpan.ToString(minutesFormat);
            }
            else
            {
                text = timeSpan.ToString(secondsFormat);
            }

            timeText.text = text;
        }
    }
}