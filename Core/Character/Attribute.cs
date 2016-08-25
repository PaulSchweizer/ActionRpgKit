using System;
using System.Collections.Generic;

namespace Character
{
    public interface IAttribute
    {
        string Name { get; set; }
        float BaseValue { get; set; }
        float Value { get; set; }
        float MaxValue { get; set; }
        float MinValue { get; set; }
    }
  
    public class PrimaryAttribute : IAttribute
    {
        private string _name;
        private float _value;
        private float _minValue;
        private float _maxValue;

        public PrimaryAttribute (string name, 
                                 float minValue = float.MinValue, 
                                 float maxValue = float.MaxValue,
                                 float value = 0)
        {
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public virtual float BaseValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public virtual float Value
        {
            get
            {
                float value = BaseValue;
                return Math.Max(MinValue, Math.Min(MaxValue, value));
            }
            set
            {
                BaseValue = value;
            }
        }

        public virtual float MaxValue
        {
            get
            {
                return _maxValue;
            }

            set
            {
                _maxValue = value;
            }
        }

        public virtual float MinValue
        {
            get
            {
                return _minValue;
            }

            set
            {
                _minValue = value;
            }
        }
    }
}
