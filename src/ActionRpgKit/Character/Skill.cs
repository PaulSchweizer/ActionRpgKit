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
        /// <summary>
        /// Used to register in the Skill Database.</summary>
        public int Id;

        /// <summary>
        /// The name of the Skill.</summary>
        public string Name;

        /// <summary>
        /// A description of the Skill.</summary>
        public string Description;

        /// <summary>
        /// The time until the skill can be used the next time.</summary>
        public float CooldownTime;

        /// <summary>
        /// The sequene of Items needed to trigger the Skill.</summary>
        public int[] ItemSequence;

        public BaseSkill() { }

        public BaseSkill(int id,
                         string name,
                         string description,
                         float cooldownTime,
                         int[] itemSequence)
        {
            Id = id;
            Name = name;
            Description = description;
            CooldownTime = cooldownTime;
            ItemSequence = itemSequence;
        }

        /// <summary>
        /// Check the given item sequence if it matches.</summary>
        /// <param name="items">The sequence of Items</param>
        /// <returns>Whether it matches or not.</returns>
        public virtual bool Match(int[] items)
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
                          float cooldownTime,
                          int[] itemSequence,
                          float cost): base(id,
                                            name,
                                            description,
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
                          float cooldownTime,
                          int[] itemSequence,
                          float damage,
                          int maximumTargets,
                          float range) : base(id,
                                              name,
                                              description,
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
        /// <summary>
        /// The curation of the Skill.</summary>
        public float Duration;

        /// <summary>
        /// The modifer value applied to the Attribute.</summary>
        public float ModifierValue;

        /// <summary>
        /// The name of the modified attribute.</summary>
        public string ModifiedAttributeName;

        public PassiveMagicSkill(int id,
                                 string name,
                                 string description,
                                 float cooldownTime,
                                 int[] itemSequence,
                                 float cost,
                                 float duration,
                                 float modifierValue,
                                 string modifiedAttributeName) : base(id,
                                                                      name,
                                                                      description,
                                                                      cooldownTime,
                                                                      itemSequence, 
                                                                      cost)
        {
            Duration = duration;
            ModifierValue = modifierValue;
            ModifiedAttributeName = modifiedAttributeName;
        }

        /// <summary>
        /// Pretty representation of the Skill.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} (Cost: {1}, {2} +{3} for {4} sec)",
                Name, Cost, ModifiedAttributeName, ModifierValue, Duration);
        }

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
                                  float cooldownTime,
                                  int[] itemSequence,
                                  float damage,
                                  int maximumTargets,
                                  float range) : base(id,
                                                      name,
                                                      description,
                                                      cooldownTime,
                                                      itemSequence,
                                                      damage,
                                                      maximumTargets,
                                                      range) {}

        /// <summary>
        /// Pretty representation of the Skill.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} (Damage: {1}, MaxTargets: {2})", Name, Damage, MaximumTargets);
        }

        #region CombatSkill implementations

        /// <summary>
        /// Inform the attacked Characters that they are being attacked.</summary>
        public override void Use(IFighter user)
        {
            float damage = Damage + user.Damage;
            for (int i = 0; i < Math.Min(MaximumTargets, user.EnemiesInAttackRange.Count); i++)
            {
                if (i == 0)
                {
                    user.TargetedEnemy.OnAttacked(user, damage);
                }
                else
                {
                    user.EnemiesInAttackRange[i].OnAttacked(user, damage);
                }
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
        /// A default combat skill that is being used when there is no other 
        /// Skill available.</summary>
        private static CombatSkill DefaultCombatSkill = 
            new GenericCombatSkill(id: -1, name: "__Default__", 
                                   description: "The default combat Skill", 
                                   cooldownTime: 1, itemSequence: new int[] {}, 
                                   damage: 0, maximumTargets: 1, range: 1);

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
            if (id == -1)
            {
                return DefaultCombatSkill;
            }
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
            if (name == "__Default__")
            {
                return DefaultCombatSkill;
            }
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
