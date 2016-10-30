using System;
using ActionRpgKit.Character.Attribute;
using ActionRpgKit.Item;

namespace ActionRpgKit.Character.Skill
{

    #region Abstracts

    /// <summary>
    /// A basic Skill implementation.</summary>
    [Serializable]
    public abstract class BaseSkill
    {

        public int Id;

        public string Name;

        public string Description;

        public float CooldownTime;

        public float PreUseTime;

        public int[] ItemSequence;

        public BaseSkill() { }

        public BaseSkill(int id,
                         string name,
                         string description,
                         float preUseTime,
                         float cooldownTime,
                         int[] itemSequence)
        {
            Id = id;
            Name = name;
            Description = description;
            PreUseTime = preUseTime;
            CooldownTime = cooldownTime;
            ItemSequence = itemSequence;
        }

        public abstract bool Match(int[] items);
    }

    /// <summary>
    /// A magic Skill costs magic energy on each use.</summary>
    [Serializable]
    public abstract class MagicSkill : BaseSkill
    {
        /// <summary>
        /// The energy costs of the Skill.</summary>
        public float Cost;

        /// <summary>
        /// The Skill is used and takes effect.</summary>
        public abstract void Use(BaseCharacter user);

        public MagicSkill() { }

        public MagicSkill(int id,
                          string name,
                          string description,
                          float preUseTime,
                          float cooldownTime,
                          int[] itemSequence,
                          float cost): base(id,
                                            name,
                                            description,
                                            preUseTime,
                                            cooldownTime,
                                            itemSequence)
        {
            Cost = cost;
        }
    }

    /// <summary>
    /// A Skill to be used as an Attack in Combat.</summary>
    [Serializable]
    public abstract class CombatSkill : BaseSkill
    {
        /// <summary>
        /// The amount of damage.</summary>
        public float Damage;

        /// <summary>
        /// The maximum amount of enemies that can be targeted at once.</summary>
        public int MaximumTargets;

        /// <summary>
        /// The maximum range at which the skill works.</summary>
        public float Range;

        /// <summary>
        /// The Skill is used and takes effect.</summary>
        public abstract void Use(IFighter user);

        public CombatSkill() { }

        public CombatSkill(int id,
                          string name,
                          string description,
                          float preUseTime,
                          float cooldownTime,
                          int[] itemSequence,
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
    }

    #endregion

    #region Implementations

    /// <summary>
    /// A passive MagicSkill adds buffs on the User itself.</summary>
    [Serializable]
    public class PassiveMagicSkill : MagicSkill
    {

        public float Duration;
        public float ModifierValue;
        public string ModifiedAttributeName;

        public PassiveMagicSkill(int id,
                                 string name,
                                 string description,
                                 float preUseTime,
                                 float cooldownTime,
                                 int[] itemSequence,
                                 float cost,
                                 float duration,
                                 float modifierValue,
                                 string modifiedAttributeName) : base(id,
                                                                      name,
                                                                      description,
                                                                      preUseTime,
                                                                      cooldownTime,
                                                                      itemSequence, 
                                                                      cost)
        {
            
            Duration = duration;
            ModifierValue = modifierValue;
            ModifiedAttributeName = modifiedAttributeName;
        }

        public override string ToString()
        {
            return String.Format("{0} (Cost: {1}, {2} +{3} for {4} sec)",
                Name, Cost, ModifiedAttributeName, ModifierValue, Duration);
        }

        #region ISkill implementations

        public override bool Match(int[] items)
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

        /// <summary>
        /// Add the modifier to the modified attribute.</summary>
        public override void Use(BaseCharacter user)
        {
            user.Stats.Dict[ModifiedAttributeName].AddModifier(GetModifier());
        }

        /// <summary>
        /// A new TimeBasedModifier is returned everytime it is requested.</summary>
        private AttributeModifier GetModifier()
        {
            return new TimeBasedModifier(Name, ModifierValue, Duration);
        }

        #endregion
    }

    /// <summary>
    /// Allows to attack with a melee weapon.</summary>
    [Serializable]
    public class GenericCombatSkill : CombatSkill
    {

        public GenericCombatSkill(int id,
                                  string name,
                                  string description,
                                  float preUseTime,
                                  float cooldownTime,
                                  int[] itemSequence,
                                  float damage,
                                  int maximumTargets,
                                  float range) : base(id,
                                                      name,
                                                      description,
                                                      preUseTime,
                                                      cooldownTime,
                                                      itemSequence,
                                                      damage,
                                                      maximumTargets,
                                                      range)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} (Damage: {1}, MaxTargets: {2})", Name, Damage, MaximumTargets);
        }

        #region ISkill implementations

        public override bool Match(int[] items)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CombatSkill implementations

        /// <summary>
        /// Inform the attacked Characters that they are being attacked.</summary>
        public override void Use(IFighter user)
        {
            float damage = Damage;
            if (user.EquippedWeapon > -1)
            {
                damage += ItemDatabase.GetWeaponItemById(user.EquippedWeapon).Damage;
            }
            for (int i = Math.Min(MaximumTargets, user.EnemiesInAttackRange.Count) - 1; i >= 0; i--)
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
        /// The index of the array corresponds to the id of the MagicSkill.</summary>
        public static MagicSkill[] MagicSkills { get; set; }

        /// <summary>
        /// The index of the array corresponds to the id of the CombatSkill.</summary>
        public static CombatSkill[] CombatSkills { get; set; }

        /// <summary>
        /// Retrieve the MagicSkill by it's Id.</summary>
        public static MagicSkill GetMagicSkillById(int id)
        {
            return MagicSkills[id];
        }

        /// <summary>
        /// Retrieve the CombatSkill by it's Id.</summary>
        public static CombatSkill GetCombatSkillById(int id)
        {
            return CombatSkills[id];
        }

        /// <summary>
        /// Retrieve the MagicSkill by Name.</summary>
        public static MagicSkill GetMagicSkillByName(string name)
        {
            for (int i = 0; i < MagicSkills.Length; i++)
            {
                if (MagicSkills[i].Name == name)
                {
                    return MagicSkills[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieve the CombatSkill by Name.</summary>
        public static CombatSkill GetCombatSkillByName(string name)
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
