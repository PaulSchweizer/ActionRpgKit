using System;
using System.Collections.Generic;
using Character.Skill;
using Character.Stats;

namespace Character
{
    /// <summary>
    /// Characters populate the game world. 
    /// They are defined by Stats and can make use of their 
    /// learned Skills.</summary>
    public interface ICharacter
    {
        /// <summary>
        /// Name of the character.</summary>
        string Name { get; }
        
        /// <summary>
        /// Stats describing the Character.</summary>
        BaseStats Stats { get; set; }

        /// <summary>
        /// Skills available for this Character.</summary>
        List<ISkill> Skills { get; }

        /// <summary>
        /// Add a new Skill to the list of available Skills.</summary>
        void LearnSkill (ISkill skill);

        /// <summary>
        /// Trigger the given Skill.</summary>
        bool TriggerSkill (ISkill skill);
    }
    
    /// <summary>
    /// Base implementation of a Character.</summary>
    public class BaseCharacter : ICharacter
    {
        private string _name;
        private BaseStats _stats;
        private List<ISkill> _skills = new List<ISkill>();
        private List<float> _skillEndTimes = new List<float>();

        public BaseCharacter (string name)
        {
            _name = name;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public BaseStats Stats
        {
            get
            {
                return _stats;
            }
            set
            {
                _stats = value;
            }
        }

        public List<ISkill> Skills
        {
            get
            {
                return _skills;
            }
        }

        /// <summary>
        /// Check if the character knows the skill, has enough 
        /// magic energy and is not in cooldown of the Skill.</summary>
        /// <param name=skill>The Skill to test</param>
        /// <returns> Whether the Skill van be used.</returns>
        private bool SkillCanBeUsed(ISkill skill)
        {
            if (!Skills.Contains(skill))
            {
                return false;
            }
            if (Stats.Magic.Value < skill.Cost)
            {
                return false;
            }
            return GameTime.time >= _skillEndTimes[Skills.IndexOf(skill)];
        }

        public void LearnSkill (ISkill skill)
        {
            _skills.Add(skill);
            _skillEndTimes.Add(-1);
        }

        public bool TriggerSkill (ISkill skill)
        {
            if (!SkillCanBeUsed(skill))
            {
                return false;
            }
            Stats[skill.EnergyAttributeName].Value -= skill.Cost;
            _skillEndTimes[Skills.IndexOf(skill)] = GameTime.time + skill.CooldownTime;
            PreUseCountdown(skill);
            return true;
        }

        /// <summary>
        /// A countdown before the Skill takes action.</summary>
        /// <remarks>This can be used for syncing animations or effects.</remarks>
        /// <param name=skill>The Skill to use</param>
        public virtual void PreUseCountdown(ISkill skill)
        {
            //
            // Implement a Coroutine in Monobehaviour
            //
            UseSkill(skill);
        }
        
        /// <summary>
        /// Triggers the use of the Skill</summary>
        /// <param name=skill>The Skill to use</param>
        private void UseSkill(ISkill skill)
        {
            skill.Use(this);
        }
    }
    
    /// <summary>
    /// Representation of a Player controllable character.</summary>
    public class Player : BaseCharacter
    {
        public Player(string name) : base(name)
        {   
            Stats = new PlayerStats();
        }
    }
    
    /// <summary>
    /// Representation of a Hostile, game controlled character.</summary>
    public class Enemy : BaseCharacter
    {
        public Enemy(string name) : base(name)
        {   
            Stats = new EnemyStats();
        }
    }

}
