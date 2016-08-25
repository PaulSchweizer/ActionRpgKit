using System;
using System.Collections.Generic;

namespace Character
{
    /// <summary>
    /// The Stats describe a Character.
    /// </summary>
    public class Stats
    {
        // Primary Attributes
        public IAttribute Body;
        public IAttribute Mind;
        public IAttribute Soul;
        public IAttribute Experience;

        // Secondary Attributes
        public IAttribute Level;
        public IAttribute Life;
        public IAttribute Magic;
        
        // Skills
        private List<ISkill> _skills;

        /// <summary>
        /// Initialize the Stats with default primary attribute values.
        /// </summary>
        public Stats (float body = 0, float mind = 0, float soul = 0)
        {
            // Primary Attributes
            Body = new Attribute("Body", 0, 999, body);
            Mind = new Attribute("Mind", 0, 999, mind);
            Soul = new Attribute("Soul", 0, 999, soul);
            Experience = new Attribute("Experience");

            // Secondary Attributes
            Level = new SecondaryAttribute("Level",
                x => (int)(Math.Sqrt(x[0].Value / 100)) * 1f, 
                new IAttribute[] { Experience }, 0, 99);
            Life = new VolumeAttribute("Life", 
                x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f, 
                new IAttribute[] { Level, Body }, 0, 999);
            Magic = new VolumeAttribute("Magic", 
                x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f, 
                new IAttribute[] { Level, Soul }, 0, 999);

            // Skills
            _skills = new List<ISkill>();
        }

        public List<ISkill> Skills 
        {
            get
            {
                return _skills;
            }
        }

        public void AddSkill (ISkill skill)
        {
            Skills.Add(skill);
        }

        public override string ToString()
        {
            return String.Format("--- Primary Attributes ------------\n" +
                                 "{0}\n{1}\n{2}\n" +
                                 "--- Secondary Attributes ------------\n" +
                                 "{3}\n{4}\n{5}\n" +
                                 "--- Skills ------------\n",
                                 Body.ToString(), 
                                 Mind.ToString(), 
                                 Soul.ToString(),
                                 Level.ToString(),
                                 Life.ToString(),
                                 Magic.ToString());
        }
    }

    public interface IAttribute
    {
        string Name { get; set; }
        float Value { get; set; }
        float MaxValue { get; set; }
        float MinValue { get; set; }
        List<Modifier> Modifiers { get; }
        void AddModifier(Modifier modifier);
        void RemoveModifier(Modifier modifier);
    }

    public class Attribute : IAttribute
    {
        private string _name;
        private float _value;
        private float _minValue;
        private float _maxValue;
        private List<Modifier> _modifiers = new List<Modifier>();

        public Attribute (string name, 
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

        protected virtual float BaseValue
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

        public override string ToString()
        {
            string repr = String.Format("{0, 10}: {1,3} ({2} - {3})", Name, Value, MinValue, MaxValue);
            for(int i=0; i < Modifiers.Count; i++)
            {
                repr += "\n                " + Modifiers[i].ToString();
            }
            return repr;
        }
    }

    public class SecondaryAttribute : Attribute
    {
        public delegate float Formula(IAttribute[] attributes);
        
        Formula _formula;
        IAttribute[] _attributes;
        
        public SecondaryAttribute (string name, 
                                   Formula formula, 
                                   IAttribute[] attributes,
                                   float minValue = float.MinValue,
                                   float maxValue = float.MaxValue) : 
                                   base(name, minValue, maxValue)
        {
            _formula = formula;
            _attributes = attributes;
        } 
        
        protected override float BaseValue
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

        public VolumeAttribute (string name, 
                                Formula formula, 
                                IAttribute[] attributes,
                                float minValue = 0f,
                                float maxValue = 0f) : 
                                base(name, formula, attributes, minValue, maxValue)
        {
            _currentValue = MaxValue;
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
                return Math.Min(_absoluteMaxValue, base.BaseValue);
            }
            set 
            {
                _absoluteMaxValue = value;
            }
        }

        public override float MinValue
        {
            get
            {
                return 0f;
            }
        }
    }

    public class Modifier 
    {
        private string _name;
        private float _value;
        private float _duration;
        private float _endTime;

        public Modifier (string name, float value, float duration)
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

        public override string ToString()
        {
            return String.Format("[{0}]: {1,3}, {2}/{3} sec)", Name, Value, RemainingTime, _duration);
        }
    }
}
