using System;
using System.Collections.Generic;
using ActionRpgKit.Core;
using System.Runtime.Serialization;

namespace ActionRpgKit.Character.Attribute
{

    /// <summary>
    /// Base class for Attributes.</summary>
    [Serializable]
    public abstract class BaseAttribute
    {

        /// <summary>
        /// The base value, needed for proper serialization.</summary>
        public float _value;

        /// <summary>
        /// The minimum value, needed for proper serialization.</summary>
        public float _minValue;

        /// <summary>
        /// The maximum value, needed for proper serialization.</summary>
        public float _maxValue;

        /// <summary>
        /// The name of the attribute.</summary>
        public string Name;

        /// <summary>
        /// The unmodified base value.</summary>
        public abstract float BaseValue { get; set; }

        /// <summary>
        /// The actual, modified value, still within the min, max range.</summary>
        public abstract float Value { get; set; }

        /// <summary>
        /// The maximum value.</summary>
        public abstract float MaxValue { get; set; }

        /// <summary>
        /// The minimum value.</summary>
        public abstract float MinValue { get; set; }

        /// <summary>
        /// All modifiers on the attribute. 
        /// There is no check to determine whether they are active or not.</summary>
        public abstract List<AttributeModifier> Modifiers { get; }

        /// <summary>
        /// Add a new modifier to the attribute and activate it.</summary>
        public abstract void AddModifier(AttributeModifier modifier);

        /// <summary>
        /// Remove a modifier from the attribute.</summary>
        public abstract void RemoveModifier(AttributeModifier modifier);

        /// <summary>
        /// Whether the attribute is modified by a modifier.</summary>
        public abstract bool IsModified { get; }

        /// <summary>
        /// Reset the Attribute to it's maximum value.</summary>
        public abstract void Reset();

        /// <summary>
        /// Emit on any change to the Value.</summary>
        public abstract event ValueChangedHandler OnValueChanged;

        /// <summary>
        /// Emit when the maximum value has been reached.</summary>
        public abstract event MaxReachedHandler OnMaxReached;

        /// <summary>
        /// Emit when the minimum value has been reached.</summary>
        public abstract event MinReachedHandler OnMinReached;
    }

    /// <summary>
    /// Handler operates whenever an IAttribute value changes.</summary>
    /// <param name="sender">The sender</param>
    /// <param name="value">The new value</param>
    public delegate void ValueChangedHandler(BaseAttribute sender, float value);

    /// <summary>
    /// Handler operates whenever an IAttribute reaches it's maximum value.</summary>
    /// <param name="sender">The sender</param>
    public delegate void MaxReachedHandler(BaseAttribute sender);

    /// <summary>
    /// Handler operates whenever an IAttribute reaches it's minimum value.</summary>
    /// <param name="sender">The sender</param>
    public delegate void MinReachedHandler(BaseAttribute sender);
    
    /// <summary>
    /// Represents a simple float value.</summary>
    [Serializable]
    public class PrimaryAttribute : BaseAttribute
    {
        public override event ValueChangedHandler OnValueChanged;
        public override event MaxReachedHandler OnMaxReached;
        public override event MinReachedHandler OnMinReached;

        private List<AttributeModifier> _modifiers = new List<AttributeModifier>();

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

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            //foreach (AttributeModifier modifier in Modifiers)
            //{
            //    Modifiers.Remove(modifier);
            //} 
            _modifiers = new List<AttributeModifier>();
        }

        public override float BaseValue
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
        /// Setting it sets the BaseValue.</summary>
        public override float Value
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

        public override float MaxValue
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

        public override float MinValue
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
        
        public override List<AttributeModifier> Modifiers
        {
            get
            {
                return _modifiers;
            }
        }
        
        public override void AddModifier (AttributeModifier modifier)
        {
            modifier.Activate();
            _modifiers.Add(modifier);
            ValueChanged(Value);
        }

        public override void RemoveModifier (AttributeModifier modifier)
        {
            Modifiers.Remove(modifier);
            ValueChanged(Value);
        }

        public override bool IsModified
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
            EmitValueChanged(value);
            if (value >= MaxValue)
            {
                EmitMaxReached();
            }
            else if (value <= MinValue)
            {
                EmitMinReached();
            }
        }
        
        protected void EmitValueChanged(float value)
        {
            var handler = OnValueChanged;
            if (handler != null)
            {
                handler(this, value);
            }
        }

        protected void EmitMaxReached ()
        {
            var handler = OnMaxReached;
            if (handler != null)
            {
                OnMaxReached(this);
            }
        }
        
        protected void EmitMinReached ()
        {
            var handler = OnMinReached;
            if (handler != null)
            {
                OnMinReached(this);
            }
        }

        public override void Reset ()
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
    /// The Value is calculated through a given formula.</summary> 
    [Serializable]
    public class SecondaryAttribute : PrimaryAttribute
    {
        /// <summary>
        /// Formula delegate to calculate the base value of the attribute.</summary> 
        /// <param name="attributes"> A list of attributes</param>
        public delegate float Formula(BaseAttribute[] attributes);
        
        /// <summary>
        /// Formula to calculate the base value of the attribute. </summary> 
        public Formula _formula;

        /// <summary>
        /// Storing the used attributes.</summary>
        protected BaseAttribute[] _attributes;

        /// <summary>
        /// Storing the used handlers.</summary>
        protected ValueChangedHandler[] _handlers;

        public SecondaryAttribute() { }

        public SecondaryAttribute (string name,
                                   Formula formula,
                                   BaseAttribute[] attributes,
                                   float minValue = float.MinValue,
                                   float maxValue = float.MaxValue) : base(name, 
                                                                           minValue, 
                                                                           maxValue)
        {
            _formula = formula;
            _attributes = attributes;
            _handlers = new ValueChangedHandler[attributes.Length];
            for (int i = 0; i < Attributes.Length; i++)
            {
                _handlers[i] = new ValueChangedHandler(ValueOfFormulatAttributeChanged);
                Attributes[i].OnValueChanged += _handlers[i];
            }
        }

        public override float BaseValue
        {
            get
            {
                return _formula(Attributes);
            }
            set
            {
                base.BaseValue = value;
            }
        }

        /// <summary>
        /// The input Attributes for the Formula have to be updated when they
        /// are being changed. 
        /// This is important for deserialization.</summary> 
        public BaseAttribute[] Attributes
        {
            get
            {
                return _attributes;
            }
            set
            {
                for (int i = 0; i < _attributes.Length; i++)
                {
                    _attributes[i].OnValueChanged -= _handlers[i];
                }
                _attributes = value;
                for (int i = 0; i < _attributes.Length; i++)
                {
                    _attributes[i].OnValueChanged += _handlers[i];
                }
            }
        }

        public void ValueOfFormulatAttributeChanged(BaseAttribute sender, float value)
        {
            ValueChanged(value);
        }
    }

    /// <summary>
    /// Represents a volume of something, e.g. magic, life.
    /// The base value is derived through a formula and serves as 
    /// the maximum value.</summary> 
    [Serializable]
    public class VolumeAttribute : SecondaryAttribute
    {
        /// <summary>
        /// The current value of the attribute.</summary> 
        private float _currentValue;
        
        /// <summary>
        /// The absolute maximum of the attribute.</summary> 
        private float _absoluteMaxValue;

        public VolumeAttribute() { }

        public VolumeAttribute(string name,
                                Formula formula,
                                BaseAttribute[] attributes,
                                float minValue = float.MinValue,
                                float maxValue = float.MaxValue) : base ()
        {
            Name = name;
            _formula = formula;
            _attributes = attributes;
            _handlers = new ValueChangedHandler[attributes.Length];
            for (int i = 0; i < Attributes.Length; i++)
            {
                _handlers[i] = new ValueChangedHandler(ValueOfFormulatAttributeChanged);
                Attributes[i].OnValueChanged += _handlers[i];
            }
            MinValue = minValue;
            MaxValue = maxValue;
            _currentValue = BaseValue;
            _absoluteMaxValue = maxValue;
        }

        public new void ValueOfFormulatAttributeChanged(BaseAttribute sender, float value)
        {
            ValueChanged(Value);
            Value = Value;
            MaxValue = BaseValue;
        }

        /// <summary>
        /// Represents the current value.</summary> 
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
        /// bigger than a absolute maximum.</summary> 
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
    /// The base value is a simple float as opposed to a formula based.</summary> 
    [Serializable]
    public class SimpleVolumeAttribute : PrimaryAttribute
    {
        /// <summary>
        /// The current value of the attribute.</summary> 
        private float _currentValue;
        
        public SimpleVolumeAttribute(string name,
                                     float minValue = float.MinValue,
                                     float maxValue = float.MaxValue,
                                     float value = 0) : base (name, minValue, maxValue, value)
        {
            _currentValue = BaseValue;
        }

        /// <summary>
        /// Set the value to the max.</summary> 
        public override void Reset()
        {
            Value = MaxValue;
        }
    }

    /// <summary>
    /// Interface for modifiers that alter an attribute.</summary> 
    [Serializable]
    public abstract class AttributeModifier
    {
        /// <summary>
        /// The name of the modifier.</summary> 
        public string Name { get; set; }
        
        /// <summary>
        /// The value of the modifier.</summary> 
        public float Value { get; set; }
        
        /// <summary>
        /// Activating the modifier.</summary> 
        public abstract void Activate();
        
        /// <summary>
        /// Determine whether the modifier is active.</summary> 
        public abstract bool IsActive { get; }
    }

    /// <summary>
    /// Modifier that affects an attribute over a certain period of time.</summary> 
    [Serializable]
    public class TimeBasedModifier : AttributeModifier
    {
        /// <summary>
        /// Determining the lifetime of the modifier.</summary> 
        private float _duration;

        /// <summary>
        /// The absolute end time for the modifier.</summary> 
        private float _endTime;

        public TimeBasedModifier(string name,
                                 float value,
                                 float duration)
        {
            Name = name;
            Value = value;
            _duration = duration;
        }
        
        /// <summary>
        /// Set the end time.</summary> 
        public override void Activate()
        {
            _endTime = GameTime.time + _duration;
        }
        
        /// <summary>
        /// The remaining time based on the current game time.</summary> 
        public float RemainingTime
        {
            get
            {
                return _endTime - GameTime.time;
            }
        }
        
        /// <summary>
        /// Whether there is any remaining time.</summary> 
        public override bool IsActive
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
