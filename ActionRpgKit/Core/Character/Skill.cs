using System;
using System.Collections;
using Character;
using Character.Attribute;

namespace Character.Skill
{
    /// <summary>
    /// Interface for Skills.</summary>
    public interface ISkill
    {
        /// <summary>
        /// Unique identifier for the Skill. </summary>
        /// <remarks>This can be used to create a database of Skills.</remarks>
        int Id { get; }
        
        /// <summary>
        /// Name of the Skill.</summary>  
        string Name { get; }
        
        /// <summary>
        /// A description.</summary>          
        string Description { get; }
        
        /// <summary>
        /// The time prior to the usage.</summary>  
        float PreUseTime { get; }
        
        /// <summary>
        /// The time it takes until the Skill can be used again.</summary>  
        float CooldownTime { get; }

        /// <summary>
        /// Whether the skill matches the given series of items.</summary>  
        bool Match();
        
        /// <summary>
        /// The Skill is used and takes effect.</summary>  
        void Use(ICharacter user);
    }

    /// <summary>
    /// A magic Skill costs magic energy on each use.</summary>
    public interface IMagicSkill : ISkill
    {
        /// <summary>
        /// The energy costs of the Skill.</summary>  
        float Cost { get; }
    }
    
    /// <summary>
    /// A Skill to be used as an Attack in Combat.</summary>
    public interface ICombatSkill : ISkill
    {
        /// <summary>
        /// The amount of damage.</summary>  
        float Damage { get; }
    }

    /// <summary>
    /// A passive Skill acts on the User itself.</summary>
    public class PassiveMagicSkill : IMagicSkill
    {
        private int _id;
        private string _name;
        private string _description;
        private float _cost;
        private float _duration;
        private float _preUseTime;
        private float _cooldownTime;
        private float _endTime = -1;
        private float _modifierValue;
        private string _modifiedAttributeName;

        public PassiveMagicSkill(int id,
                                 string name,
                                 string description,
                                 float cost,
                                 float duration,
                                 float preUseTime,
                                 float cooldownTime,
                                 float modifierValue,
                                 string modifiedAttributeName)
        {
            _id = id;
            _name = name;
            _description = description;
            _cost = cost;
            _duration = duration;
            _preUseTime = preUseTime;
            _cooldownTime = cooldownTime;
            _modifierValue = modifierValue;
            _modifiedAttributeName = modifiedAttributeName;
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public float Cost
        {
            get
            {
                return _cost;
            }
        }

        /// <summary>
        /// A new TimeBasedModifier is returned everytime it is requested.</summary>  
        private IModifier GetModifier ()
        { 
            return new TimeBasedModifier(Name, _modifierValue, _duration);
        }

        public float CooldownTime
        {
            get
            {
                return _cooldownTime;
            }
        }

        public float PreUseTime
        {
            get
            {
                return _preUseTime;
            }
        }

        public bool Match()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Add the modifier to the modified attribute.</summary>  
        public void Use(ICharacter user)
        {
            user.Stats[_modifiedAttributeName].AddModifier(GetModifier());
        }
    }
    
    public class MeleeSkill : ICombatSkill
    {
        private int _id;
        private string _name;
        private string _description;
        private float _preUseTime;
        private float _cooldownTime;
        private float _damage;

        public MeleeSkill(int id,
                          string name,
                          string description,
                          float preUseTime,
                          float cooldownTime,
                          float damage)
        {
            _id = id;
            _name = name;
            _description = description;
            _preUseTime = preUseTime;
            _cooldownTime = cooldownTime;
            _damage = damage;
        }
        
        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }
        
        public float CooldownTime
        {
            get
            {
                return _cooldownTime;
            }
        }

        public float PreUseTime
        {
            get
            {
                return _preUseTime;
            }
        }
        
        public float Damage 
        { 
            get
            {
                return _damage;
            }
        }

        public bool Match()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Add the modifier to the modified attribute.</summary>  
        public void Use(ICharacter user)
        {
            throw new NotImplementedException();
        }
    }
}
