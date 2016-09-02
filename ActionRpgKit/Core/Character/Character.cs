using System;
using System.Collections.Generic;
using Character.Skill;
using Character.Stats;

namespace Character
{
    /// <summary>
    /// Characters populate the game world. 
    /// They are defined by Stats.</summary>
    public interface ICharacter
    {
        /// <summary>
        /// Name of the character.</summary>
        string Name { get; }
        
        /// <summary>
        /// Stats describing the Character.</summary>
        BaseStats Stats { get; set; }
    }

    /// <summary>
    /// Character can use Magic.</summary>  
    public interface IMagicUser
    {
        /// <summary>
        /// MagicSkills available for this Character.</summary>
        List<IMagicSkill> MagicSkills { get; }

        /// <summary>
        /// Add a new MagicSkill.</summary>
        void LearnMagicSkill (IMagicSkill magicSkill);

        /// <summary>
        /// Trigger the given MagicSkill.</summary>
        bool TriggerMagicSkill (IMagicSkill magicSkill);
    }
    
    /// <summary>
    /// Character can fight.</summary>  
    public interface IFighter
    {
        /// <summary>
        /// The remaining life.</summary>
        float Life { get; set; }

        /// <summary>
        /// Targeted enemies of the fighter.</summary>
        List<IFighter> Enemies { get; }

        /// <summary>
        /// Add a new enemy.</summary>
        void AddEnemy (IFighter enemy);

        /// <summary>
        /// CombatSkills available for this Character.</summary>
        List<ICombatSkill> CombatSkills { get; }

        /// <summary>
        /// Add a new CombatSkill.</summary>
        void LearnCombatSkill (ICombatSkill combatSkill);

        /// <summary>
        /// Attack with the given CombatSkill.</summary>
        bool TriggerCombatSkill (ICombatSkill combatSkill);

        /// <summary>
        /// The Fighter is being attacked.</summary>
        void OnAttacked (IFighter attacker, float damage);
    }
    
    /// <summary>
    /// Base implementation of a Character.</summary>
    public class BaseCharacter : ICharacter, IMagicUser, IFighter
    {
        private string _name;
        private BaseStats _stats;
        private List<IMagicSkill> _magicSkills = new List<IMagicSkill>();
        private List<float> _magicSkillEndTimes = new List<float>();
        
        private List<ICombatSkill> _combatSkills = new List<ICombatSkill>();

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

        // ------------------------------------------------------
        // IMagicUser Implementations
        // ------------------------------------------------------

        public List<IMagicSkill> MagicSkills
        {
            get
            {
                return _magicSkills;
            }
        }

        /// <summary>
        /// Check if the character knows the magic skill, has enough 
        /// magic energy and is not in cooldown of the Skill.</summary>
        /// <param name=magicSkill>The Skill to test</param>
        /// <returns> Whether the Skill van be used.</returns>
        private bool MagicSkillCanBeUsed(IMagicSkill magicSkill)
        {
            if (!MagicSkills.Contains(magicSkill))
            {
                return false;
            }
            if (Stats.Magic.Value < magicSkill.Cost)
            {
                return false;
            }
            return GameTime.time >= _magicSkillEndTimes[MagicSkills.IndexOf(magicSkill)];
        }

        public void LearnMagicSkill (IMagicSkill magicSkill)
        {
            _magicSkills.Add(magicSkill);
            _magicSkillEndTimes.Add(-1);
        }

        public bool TriggerMagicSkill (IMagicSkill magicSkill)
        {
            if (!MagicSkillCanBeUsed(magicSkill))
            {
                return false;
            }
            Stats.Magic.Value -= magicSkill.Cost;
            _magicSkillEndTimes[MagicSkills.IndexOf(magicSkill)] = GameTime.time + magicSkill.CooldownTime;
            PreUseCountdown(magicSkill);
            return true;
        }

        /// <summary>
        /// A countdown before the Skill takes action.</summary>
        /// <remarks>This can be used for syncing animations or effects.</remarks>
        /// <param name=magicSkill>The Skill to use</param>
        public virtual void PreUseCountdown(IMagicSkill magicSkill)
        {
            //
            // Implement a Coroutine in Monobehaviour
            //
            UseMagicSkill(magicSkill);
        }
        
        /// <summary>
        /// Triggers the use of the Skill</summary>
        /// <param name=magicSkill>The Skill to use</param>
        private void UseMagicSkill(IMagicSkill magicSkill)
        {
            magicSkill.Use(this);
        }
        
        // ------------------------------------------------------
        // IFighter Implementations
        // ------------------------------------------------------
        
        public List<IFighter> Enemies 
        { 
            get
            {
                throw new NotImplementedException();
            }
        }
        
        public List<ICombatSkill> CombatSkills 
        {
            get
            {
                return _combatSkills;
            }
        }
        
        public void LearnCombatSkill (ICombatSkill combatSkill)
        {
            _combatSkills.Add(combatSkill);
        }

        public bool TriggerCombatSkill (ICombatSkill combatSkill)
        {
            throw new NotImplementedException();
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
