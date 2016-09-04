using System;
using System.Collections;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;

namespace ActionRpgKit.Character.Skill
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

        public BaseSkill(int id,
                         string name,
                         string description,
                         float preUseTime,
                         float cooldownTime)
        {
            Id = id;
            Name = name;
            Description = description;
            PreUseTime = preUseTime;
            CooldownTime = cooldownTime;
        }

        public int Id
        {
            get;
        }

        public string Name
        {
            get;
        }

        public string Description
        {
            get;
        }

        public float CooldownTime
        {
            get;
        }

        public float PreUseTime
        {
            get;
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
            Cost = cost;
            _duration = duration;
            _modifierValue = modifierValue;
            _modifiedAttributeName = modifiedAttributeName;
        }

        public override string ToString()
        {
            return String.Format("{0} (Cost: {1}, {2} +{3} for {4} sec)", 
                Name, Cost, _modifiedAttributeName, _modifierValue, _duration);
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
            get; set;
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
            Damage = damage;
            MaximumTargets = maximumTargets;
        }

        public override string ToString()
        {
            return String.Format("{0} (Damage: {1}, MaxTargets: {2})", Name, Damage, MaximumTargets);
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
            get;
        }

        public int MaximumTargets
        {
            get;
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
