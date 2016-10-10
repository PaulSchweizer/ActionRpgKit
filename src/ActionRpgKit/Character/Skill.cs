using System;
using ActionRpgKit.Character.Attribute;
using ActionRpgKit.Item;

namespace ActionRpgKit.Character.Skill
{
    #region Interfaces

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
        /// A sequence of Items that triggers the Skill.</summary>
        IItem[] ItemSequence { get; set; }

        /// <summary>
        /// Whether the skill matches the given series of items.</summary>
        bool Match (IItem[] items);
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
        /// The maximum range at which the skill works.</summary>
        float Range { get; }

        /// <summary>
        /// The Skill is used and takes effect.</summary>
        void Use (IFighter user);
    }

    #endregion

    #region Abstracts

    /// <summary>
    /// A basic Skill implementation.</summary>
    public abstract class BaseSkill : ISkill
    {

        public BaseSkill(int id,
                         string name,
                         string description,
                         float preUseTime,
                         float cooldownTime,
                         IItem[] itemSequence)
        {
            Id = id;
            Name = name;
            Description = description;
            PreUseTime = preUseTime;
            CooldownTime = cooldownTime;
            ItemSequence = itemSequence;
        }

        public int Id { get; }

        public string Name { get; }

        public string Description { get; }

        public float CooldownTime { get; }

        public float PreUseTime { get; }

        public IItem[] ItemSequence { get; set; }

        public abstract bool Match(IItem[] items);
    }

    #endregion

    #region Implementations

    /// <summary>
    /// A passive MagicSkill adds buffs on the User itself.</summary>
    public class PassiveMagicSkill : BaseSkill, IMagicSkill
    {
        private float _duration;
        private float _modifierValue;
        private string _modifiedAttributeName;

        public PassiveMagicSkill(int id,
                                 string name,
                                 string description,
                                 float preUseTime,
                                 float cooldownTime,
                                 IItem[] itemSequence,
                                 float cost,
                                 float duration,
                                 float modifierValue,
                                 string modifiedAttributeName) : base(id,
                                                                      name,
                                                                      description,
                                                                      preUseTime,
                                                                      cooldownTime,
                                                                      itemSequence)
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

        #region ISkill implementations

        public override bool Match(IItem [] items)
        {
            if (ItemSequence.Length != items.Length)
            {
                return false;
            }
            bool match = true;
            for (int i = 0; i < ItemSequence.Length; i++)
            {
                if (ItemSequence[i] != items[i])
                {
                    match = false;
                    break;
                }
            }
            return match;
        }

        #endregion

        #region IMagicSkill implementations

        public float Cost { get; set; }

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

        #endregion
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
                          IItem[] itemSequence,
                          float damage,
                          int maximumTargets,
                          float range) : base(id,
                                              name,
                                              description,
                                              preUseTime,
                                              cooldownTime,
                                              itemSequence)
        {
            Damage = damage;
            MaximumTargets = maximumTargets;
            Range = range;
        }

        public override string ToString()
        {
            return String.Format("{0} (Damage: {1}, MaxTargets: {2})", Name, Damage, MaximumTargets);
        }

        #region ISkill implementations

        public override bool Match (IItem[] items)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICombatSkill implementations

        public float Damage { get; }

        public int MaximumTargets { get; }

        public float Range { get; }

        /// <summary>
        /// Inform the attacked Characters that they are being attacked.</summary>
        public void Use (IFighter user)
        {
            float damage = Damage;
            if (user.EquippedWeapon != null)
            {
                damage += user.EquippedWeapon.Damage;
            }
            for (int i = Math.Min(MaximumTargets, user.EnemiesInAttackRange.Length) - 1; i >= 0; i--)
            {
                user.EnemiesInAttackRange[i].OnAttacked(user, damage);
            }
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// Holds all the Skills available in the Game.</summary>
    public static class SkillDatabase
    {
        /// <summary>
        /// The index of the array corresponds to the id of the IMagicSkill.</summary>
        public static IMagicSkill[] MagicSkills { get; set;}

        /// <summary>
        /// The index of the array corresponds to the id of the ICombatSkill.</summary>
        public static ICombatSkill[] CombatSkills { get; set; }

        /// <summary>
        /// Retrieve the IMagicSkill by Name.</summary>
        public static IMagicSkill GetMagicSkillByName(string name)
        {
            for (int i=0; i < MagicSkills.Length; i++)
            {
                if (MagicSkills[i].Name == name)
                {
                    return MagicSkills[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieve the ICombatSkill by Name.</summary>
        public static ICombatSkill GetCombatSkillByName(string name)
        {
            for (int i = 0; i < CombatSkills.Length; i++)
            {
                if (CombatSkills[i].Name == name)
                {
                    return CombatSkills[i];
                }
            }
            return null;
        }
    }
}
