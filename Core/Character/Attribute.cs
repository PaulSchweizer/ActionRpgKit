using System;
using System.Collections.Generic;

namespace Character.Attribute
{
    /// <summary>
    /// Interface for Attributes. 
    /// </summary>
    public interface IAttribute
    {
        /// <summary>
        /// The name of the attribute. 
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// The unmodified base value.
        /// </summary>
        float BaseValue { get; set; }
        
        /// <summary>
        /// The actual, modified value, still within the min, max range.
        /// </summary>
        float Value { get; set; }
        
        /// <summary>
        /// The maximum value.
        /// </summary>
        float MaxValue { get; set; }
        
        /// <summary>
        /// The minimum value.
        /// </summary>
        float MinValue { get; set; }

        /// <summary>
        /// All modifiers on the attribute. 
        /// There is no check to determine whether they are active or not.
        /// </summary>
        List<Modifier> Modifiers { get; }
        
        /// <summary>
        /// Add a new modifier to the attribute and activate it.
        /// </summary>
        void AddModifier(Modifier modifier);

        /// <summary>
        /// Remove a modifier from the attribute.
        /// </summary>
        void RemoveModifier(Modifier modifier);
    }
    
    /// <summary>
    /// Represents a simple float value. 
    /// </summary>
    public class PrimaryAttribute : IAttribute
    {
        private string _name;
        private float _value;
        private float _minValue;
        private float _maxValue;
        private List<Modifier> _modifiers = new List<Modifier>();

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
                _value = Math.Max(MinValue, Math.Min(MaxValue, value));
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
        
        public List<Modifier> Modifiers
        {
            get
            {
                return _modifiers;
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
        private float _endTime;
        
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
        
        public void Activate ()
        {
            _endTime = GameTime.time + _duration;
        }

        public float RemainingTime
        {
            get
            {
                return _endTime - GameTime.time;
            }
        }

        public bool IsActive
        {
            get
            {
                return RemainingTime > 0;
            }
        }
    }
}
