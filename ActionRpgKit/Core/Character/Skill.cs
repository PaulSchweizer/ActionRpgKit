using System;
using System.Collections;
using ActionRpgKit.Core.Character;
using ActionRpgKit.Core.Character.Attribute;

namespace ActionRpgKit.Core.Character.Skill
{
    // ------------------------------------------------------------------------
    // Interfaces
    // ------------------------------------------------------------------------

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
        bool Match ();
    }

    /// <summary>
    /// A magic Skill costs magic energy on each use.</summary>
    public interface IMagicSkill : ISkill
    {
        /// <summary>
        /// The energy costs of the Skill.</summary>
        float Cost { get; }

        /// <summary>
        /// The Skill is used and takes effect.</summary>
        void Use (ICharacter user);
    }

    /// <summary>
    /// A Skill to be used as an Attack in Combat.</summary>
    public interface ICombatSkill : ISkill
    {
        /// <summary>
        /// The amount of damage.</summary>
        float Damage { get; }

        /// <summary>
        /// The maximum amount of enemies that can be targeted at once.</summary>
        int MaximumTargets { get; }

        /// <summary>
        /// The Skill is used and takes effect.</summary>
        void Use (IFighter user);
    }

    // ------------------------------------------------------------------------
    // Abstracts
    // ------------------------------------------------------------------------

    /// <summary>
    /// A basic Skill implementation.</summary>
    public abstract class BaseSkill : ISkill
    {
        private int _id;
        private string _name;
        private string _description;
        private float _preUseTime;
        private float _cooldownTime;

        public BaseSkill(int id,
                         string name,
                         string description,
                         float preUseTime,
                         float cooldownTime)
        {
            _id = id;
            _name = name;
            _description = description;
            _preUseTime = preUseTime;
            _cooldownTime = cooldownTime;
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

        public abstract bool Match();
    }

    // ------------------------------------------------------------------------
    // Implementations
    // ------------------------------------------------------------------------

    /// <summary>
    /// A passive MagicSkill adds buffs on the User itself.</summary>
    public class PassiveMagicSkill : BaseSkill, IMagicSkill
    {
        private float _cost;
        private float _duration;
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
                                 string modifiedAttributeName) : base(id,
                                                                      name,
                                                                      description,
                                                                      preUseTime,
                                                                      cooldownTime)
        {
            _cost = cost;
            _duration = duration;
            _modifierValue = modifierValue;
            _modifiedAttributeName = modifiedAttributeName;
        }

        // --------------------------------------------------------------------
        // ISkill implementations
        // --------------------------------------------------------------------

        public override bool Match()
        {
            throw new NotImplementedException();
        }

        // --------------------------------------------------------------------
        // IMagicSkill implementations
        // --------------------------------------------------------------------

        public float Cost
        {
            get
            {
                return _cost;
            }
        }

        /// <summary>
        /// Add the modifier to the modified attribute.</summary>
        public void Use (ICharacter user)
        {
            user.Stats[_modifiedAttributeName].AddModifier(GetModifier());
        }

        /// <summary>
        /// A new TimeBasedModifier is returned everytime it is requested.</summary>
        private IModifier GetModifier ()
        {
            return new TimeBasedModifier(Name, _modifierValue, _duration);
        }
    }

    /// <summary>
    /// Allows to attack with a melee weapon.</summary>
    public class MeleeSkill : BaseSkill, ICombatSkill
    {

        private float _damage;
        private int _maximumTargets;

        public MeleeSkill(int id,
                          string name,
                          string description,
                          float preUseTime,
                          float cooldownTime,
                          float damage,
                          int maximumTargets) : base(id,
                                                     name,
                                                     description,
                                                     preUseTime,
                                                     cooldownTime)
        {
            _damage = damage;
            _maximumTargets = maximumTargets;
        }

        // --------------------------------------------------------------------
        // ISkill implementations
        // --------------------------------------------------------------------

        public override bool Match ()
        {
            throw new NotImplementedException();
        }

        // --------------------------------------------------------------------
        // ICombatSkill implementations
        // --------------------------------------------------------------------

        public float Damage
        {
            get
            {
                return _damage;
            }
        }

        public int MaximumTargets
        {
            get
            {
                return _maximumTargets;
            }
        }

        /// <summary>
        /// Inform the attacked Characters that they are being attacked.</summary>
        public void Use (IFighter user)
        {
            for (int i = Math.Min(MaximumTargets, user.Enemies.Count) - 1; i >= 0; i--)
            {
                user.Enemies[i].OnAttacked(user, Damage);
            }
        }
    }
}
