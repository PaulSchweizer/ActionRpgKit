using System;
using System.Collections.Generic;
using ActionRpgKit.Core;

namespace ActionRpgKit.Character.Attribute
{
    /// <summary>
    /// Interface for Attributes.</summary>
    public interface IAttribute
    {
        /// <summary>
        /// The name of the attribute.</summary>
        string Name { get; set; }

        /// <summary>
        /// The unmodified base value.</summary>
        float BaseValue { get; set; }

        /// <summary>
        /// The actual, modified value, still within the min, max range.</summary>
        float Value { get; set; }

        /// <summary>
        /// The maximum value.</summary>
        float MaxValue { get; set; }

        /// <summary>
        /// The minimum value.</summary>
        float MinValue { get; set; }

        /// <summary>
        /// All modifiers on the attribute. 
        /// There is no check to determine whether they are active or not.</summary>
        List<IModifier> Modifiers { get; }

        /// <summary>
        /// Add a new modifier to the attribute and activate it.</summary>
        void AddModifier(IModifier modifier);

        /// <summary>
        /// Remove a modifier from the attribute.</summary>
        void RemoveModifier(IModifier modifier);

        /// <summary>
        /// Whether the attribute is modified by a modifier.</summary>
        bool IsModified { get; }

        /// <summary>
        /// Reset the Attribute to it's maximum value.</summary>
        void Reset();

        event ValueChangedHandler OnValueChanged;
        event MaxReachedHandler OnMaxReached;
        event MinReachedHandler OnMinReached;
    }

    /// <summary>
    /// Handler operates whenever an IAttribute value changes.</summary>
    /// <param name="sender">The sender</param>
    /// <param name="value">The new value</param>
    public delegate void ValueChangedHandler(IAttribute sender, float value);
    
    public delegate void MaxReachedHandler(IAttribute sender);
    
    public delegate void MinReachedHandler(IAttribute sender);

    /// <summary>
    /// Represents a simple float value.</summary>
    public class PrimaryAttribute : IAttribute
    {
        
        public event ValueChangedHandler OnValueChanged;
        public event MaxReachedHandler OnMaxReached;
        public event MinReachedHandler OnMinReached;

        private string _name;
        private float _value;
        private float _minValue;
        private float _maxValue;
        private List<IModifier> _modifiers = new List<IModifier>();

        public PrimaryAttribute () {}

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
                ValueChanged(value);
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
        
        public void AddModifier (IModifier modifier)
        {
            modifier.Activate();
            _modifiers.Add(modifier);
            ValueChanged(Value);
        }

        public void RemoveModifier (IModifier modifier)
        {
            Modifiers.Remove(modifier);
            ValueChanged(Value);
        }

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

        protected void ValueChanged (float value)
        {
            if (OnValueChanged != null)
            {
                OnValueChanged(this, value);
            }
            if (value >= MaxValue)
            {
                MaxReached();
            }
            else if (value <= MinValue)
            {
                MinReached();
            }
        }

        protected void MaxReached ()
        {
            if (OnMaxReached != null)
            {
                OnMaxReached(this);
            }
        }
        
        protected void MinReached ()
        {
            if (OnMinReached != null)
            {
                OnMinReached(this);
            }
        }

        public virtual void Reset ()
        {
            Value = MinValue;
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
        /// <summary>
        /// Formula delegate to calculate the base value of the attribute.
        /// </summary> 
        /// <param name="attributes"> A list of attributes</param>
        public delegate float Formula(IAttribute[] attributes);
        
        /// <summary>
        /// Formula to calculate the base value of the attribute. 
        /// </summary> 
        protected Formula _formula;
        
        /// <summary>
        /// Input attributes for the formula.
        /// </summary> 
        protected IAttribute[] _attributes;

        public SecondaryAttribute() {}

        public SecondaryAttribute (string name,
                                   Formula formula,
                                   IAttribute[] attributes,
                                   float minValue = float.MinValue,
                                   float maxValue = float.MaxValue) :
                                   base(name, minValue, maxValue)
        {
            _formula = formula;
            _attributes = attributes;
            for (int i = 0; i < _attributes.Length; i++)
            {
                _attributes[i].OnValueChanged += new ValueChangedHandler(ValueOfFormulatAttributeChanged);
            }
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
        
        public void ValueOfFormulatAttributeChanged (IAttribute sender, float value)
        {
            ValueChanged(value);
        }
    }
    
    /// <summary>
    /// Represents a volume of something, e.g. magic, life.
    /// The base value is derived through a formula and serves as 
    /// the maximum value.
    /// </summary> 
    public class VolumeAttribute : SecondaryAttribute
    {
        /// <summary>
        /// The current value of the attribute.
        /// </summary> 
        private float _currentValue;
        
        /// <summary>
        /// The absolute maximum of the attribute.
        /// </summary> 
        private float _absoluteMaxValue;

        public VolumeAttribute (string name,
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

        /// <summary>
        /// Represents the current value.
        /// </summary> 
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
        
        /// <summary>
        /// The maximum value is congruent with the current base value, but never 
        /// bigger than a absolute maximum. 
        /// </summary> 
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

        public override void Reset()
        {
            Value = MaxValue;
        }
    }

    /// <summary>
    /// The base value is a simple float as opposed to a formula based.
    /// </summary> 
    public class SimpleVolumeAttribute : PrimaryAttribute
    {
        /// <summary>
        /// The current value of the attribute.
        /// </summary> 
        private float _currentValue;

        /// <summary>
        /// The absolute maximum of the attribute.
        /// </summary> 
        private float _absoluteMaxValue;
        
        public SimpleVolumeAttribute(string name,
                                     float minValue = float.MinValue,
                                     float maxValue = float.MaxValue,
                                     float value = 0) : base (name, minValue, maxValue, value)
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

        public override void Reset()
        {
            Value = MaxValue;
        }
    }
    
    /// <summary>
    /// Interface for modifiers that alter an attribute.
    /// </summary> 
    public interface IModifier
    {
        /// <summary>
        /// The name of the modifier.
        /// </summary> 
        string Name { get; }
        
        /// <summary>
        /// The value of the modifier.
        /// </summary> 
        float Value { get; }
        
        /// <summary>
        /// Activating the modifier.
        /// </summary> 
        void Activate();
        
        /// <summary>
        /// Determine whether the modifier is active.
        /// </summary> 
        bool IsActive { get; }
    }

    /// <summary>
    /// Modifier that affects an attribute over a certain period of time.
    /// </summary> 
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
        
        /// <summary>
        /// Set the end time.
        /// </summary> 
        public void Activate()
        {
            _endTime = GameTime.time + _duration;
        }
        
        /// <summary>
        /// The remaining time based on the current game time.
        /// </summary> 
        public float RemainingTime
        {
            get
            {
                return _endTime - GameTime.time;
            }
        }
        
        /// <summary>
        /// Whether there is any remaining time.
        /// </summary> 
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
