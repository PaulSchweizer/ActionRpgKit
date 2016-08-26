using System;
using System.Collections.Generic;

namespace Character.Attribute
{
    public interface IAttribute
    {
        string Name { get; set; }
        float BaseValue { get; set; }
        float Value { get; set; }
        float MaxValue { get; set; }
        float MinValue { get; set; }
        List<Modifier> Modifiers { get; }
        void AddModifier(Modifier modifier);
        void RemoveModifier(Modifier modifier);
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
                for (int i = Modifiers.Count - 1; i >= 0; i--)
                {
                    if (Modifiers[i].IsActive)
                    {
                        value += Modifiers[i].Value;
                    }
                    else
                    {
                        RemoveModifier(Modifiers[i]);
                    }
                }
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
        
        public void AddModifier(Modifier modifier)
        {
            modifier.Activate();
            _modifiers.Add(modifier);
        }
        
        public void RemoveModifier(Modifier modifier)
        {
            Modifiers.Remove(modifier);
        }
    }
    
    public class Modifier 
    {
        private string _name;
        private float _value;
        private float _duration;
        
        public Modifier (string name, 
                         float value, 
                         float duration)
        {
            _name = name;
            Value = value;
            _duration = duration;
        }

        public string Name 
        {
            get 
            {
                return _name;
            }
        }
    
        public float Value 
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
    }
}
