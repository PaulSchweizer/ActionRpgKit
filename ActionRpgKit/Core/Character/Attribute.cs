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
        List<IModifier> Modifiers { get; }

        /// <summary>
        /// Add a new modifier to the attribute and activate it.
        /// </summary>
        void AddModifier(IModifier modifier);

        /// <summary>
        /// Remove a modifier from the attribute.
        /// </summary>
        void RemoveModifier(IModifier modifier);

        /// <summary>
        /// Whether the attribute is modified by a modifier.
        /// </summary>
        bool IsModified { get; }
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
        private List<IModifier> _modifiers = new List<IModifier>();

        public PrimaryAttribute()
        {
        }

        public PrimaryAttribute(string name,
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

        /// <summary>
        /// The final value with all modifiers applied to it.
        /// Setting it sets the BaseValue.
        /// </summary>
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

        public List<IModifier> Modifiers
        {
            get
            {
                return _modifiers;
            }
        }

        public void AddModifier(IModifier modifier)
        {
            modifier.Activate();
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(IModifier modifier)
        {
            Modifiers.Remove(modifier);
        }

        /// <summary>
        /// Check if any active modifiers exist. 
        /// </summary> 
        public bool IsModified
        {
            get
            {
                for (int i = Modifiers.Count - 1; i >= 0; i--)
                {
                    if (Modifiers[i].IsActive)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override string ToString()
        {
            string repr = String.Format("{0, -10}: {1,-3} ({2} - {3})", Name, Value, MinValue, MaxValue);
            for (int i = 0; i < Modifiers.Count; i++)
            {
                repr += "\n            + " + Modifiers[i].ToString();
            }
            return repr;
        }
    }

    /// <summary>
    /// The Value is calculated through a given formula.
    /// </summary> 
    public class SecondaryAttribute : PrimaryAttribute
    {
        public delegate float Formula(IAttribute[] attributes);

        protected Formula _formula;
        protected IAttribute[] _attributes;

        public SecondaryAttribute(string name,
                                   Formula formula,
                                   IAttribute[] attributes,
                                   float minValue = float.MinValue,
                                   float maxValue = float.MaxValue) :
                                   base(name, minValue, maxValue)
        {
            _formula = formula;
            _attributes = attributes;
        }

        public SecondaryAttribute()
        {
        }

        public override float BaseValue
        {
            get
            {
                return _formula(_attributes);
            }
            set
            {
                base.BaseValue = value;
            }
        }
    }

    public class VolumeAttribute : SecondaryAttribute
    {

        private float _currentValue;
        private float _absoluteMaxValue;

        public VolumeAttribute(string name,
                               Formula formula,
                               IAttribute[] attributes,
                               float minValue = float.MinValue,
                               float maxValue = float.MaxValue) : base ()
        {
            Name = name;
            _formula = formula;
            _attributes = attributes;
            MinValue = minValue;
            MaxValue = maxValue;

            _currentValue = BaseValue;
            _absoluteMaxValue = maxValue;
        }

        public override float Value
        {
            get
            {
                return _currentValue;
            }
            set
            {
                _currentValue = Math.Max(MinValue, Math.Min(MaxValue, value));
            }
        }

        public override float MaxValue
        {
             get
             {
                 return Math.Min(_absoluteMaxValue, BaseValue);
             }
             set 
             {
                 _absoluteMaxValue = value;
             }
         }
    }

    public class SimpleVolumeAttribute : PrimaryAttribute
    {

        private float _currentValue;
        private float _absoluteMaxValue;
        
        public SimpleVolumeAttribute(string name,
                                     float minValue = float.MinValue,
                                     float maxValue = float.MaxValue,
                                     float value = 0)) : base (name, minValue, maxValue, value)
        {
            _currentValue = BaseValue;
            _absoluteMaxValue = maxValue;
        }

        public override float Value
        {
            get
            {
                return _currentValue;
            }
            set
            {
                _currentValue = Math.Max(MinValue, Math.Min(MaxValue, value));
            }
        }

        public override float MaxValue
        {
             get
             {
                 return Math.Min(_absoluteMaxValue, BaseValue);
             }
             set 
             {
                 _absoluteMaxValue = value;
             }
         }
    }

    public interface IModifier
    {
        string Name { get; }
        float Value { get; }
        void Activate();
        bool IsActive { get; }
    }

    public class TimeBasedModifier : IModifier
    {
        private string _name;
        private float _value;
        private float _duration;
        private float _endTime;

        public TimeBasedModifier(string name,
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

        public void Activate()
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

        public override string ToString()
        {
            return String.Format("[{0}]: {1,3}, {2}/{3} sec", Name, Value, RemainingTime, _duration);
        }
    }
}
