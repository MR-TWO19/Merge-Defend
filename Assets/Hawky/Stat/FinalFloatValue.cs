using System;
using System.Collections.Generic;
using System.Linq;

namespace Hawky.Stat
{
    public class FinalFloatValue : IFloatValue
    {
        private float BaseValue { get; }
        private List<IFloatValue> PercentValues { get; } = new List<IFloatValue>();
        private List<IFloatValue> Values { get; } = new List<IFloatValue>();

        public float Value => BaseValue + PercentValues.Sum(p => p.Value) * BaseValue + Values.Sum(v => v.Value);
        public Action OnValueChanged { get; set; }
        public FinalFloatValue(float baseValue)
        {
            BaseValue = baseValue;
        }

        public void AddPercentValue(IFloatValue percentValue)
        {
            var oldValue = Value;
            PercentValues.Add(percentValue);
            var newValue = Value;
            if (oldValue != newValue)
            {
                OnValueChanged?.Invoke();
            }
        }

        public void RemovePercentValue(IFloatValue percentValue)
        {
            var oldValue = Value;
            PercentValues.Remove(percentValue);
            var newValue = Value;
            if (oldValue != newValue)
            {
                OnValueChanged?.Invoke();
            }
        }

        public void AddValue(IFloatValue value)
        {
            var oldValue = Value;
            Values.Add(value);
            var newValue = Value;
            if (oldValue != newValue)
            {
                OnValueChanged?.Invoke();
            }
        }   

        public void RemoveValue(IFloatValue value)
        {
            var oldValue = Value;
            Values.Remove(value);
            var newValue = Value;
            if (oldValue != newValue)
            {
                OnValueChanged?.Invoke();
            }
        }

        public void Reset()
        {
            PercentValues.Clear();
            Values.Clear();
        }
    }
}