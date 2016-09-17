using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Core;
using ActionRpgKit.Character;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// Characters populate the game world. 
    /// They are defined by Stats.</summary>
    public interface ICharacter
    {
        /// <summary>
        /// Name of the character.</summary>
        string Name { get; set; }
        
        /// <summary>
        /// Stats describing the Character.</summary>
        BaseStats Stats { get; set; }
        
        /// <summary>
        /// Inventory of the Character.</summary>
        IInventory Inventory { get; set; }

        /// <summary>
        /// The active state of this Character.</summary>
        IState CurrentState { get; set; }
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
    public abstract class BaseCharacter : IGameObject, ICharacter, IMagicUser, IFighter
    {
        public IdleState _idleState;
        public MoveState _moveState;

        private List<IMagicSkill> _magicSkills = new List<IMagicSkill>();
        private List<float> _magicSkillEndTimes = new List<float>();

        private List<IFighter> _enemies = new List<IFighter>();
        private List<ICombatSkill> _combatSkills = new List<ICombatSkill>();
        private List<float> _combatSkillEndTimes = new List<float>();

        public BaseCharacter ()
        {
            _idleState = new IdleState();
            _moveState = new MoveState();
            CurrentState = _idleState;
        }

        public BaseCharacter (string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            string repr = string.Format("### CHARACTER: {0} ########################\n" +
                                "--- Primary Attributes ------------\n" +
                                 "{1}\n{2}\n{3}\n{4}\n" +
                                 "--- Secondary Attributes ------------\n" +
                                 "{5}\n{6}\n",
                                 Name,
                                 Stats.Body.ToString(),
                                 Stats.Mind.ToString(),
                                 Stats.Soul.ToString(),
                                 Stats.Level.ToString(),
                                 Stats.Life.ToString(),
                                 Stats.Magic.ToString());
            repr += "--- Combat Skills ------------\n";
            for (int i = 0; i < CombatSkills.Count; i++)
            {
                repr += string.Format("{0}\n", CombatSkills[i].ToString());
            }
            repr += "--- Magic Skills ------------";
            for (int i = 0; i < MagicSkills.Count; i++)
            {
                repr += string.Format("\n{0}", MagicSkills[i].ToString());
            }
            repr += "\n";
            repr += Inventory.ToString();
            return repr;
        }

        // --------------------------------------------------------------------
        // IGameObject Implementations
        // --------------------------------------------------------------------

        public Position Position { get; set; } = new Position();
        
        // --------------------------------------------------------------------
        // ICharacter Implementations
        // --------------------------------------------------------------------

        public string Name { get; set; }

        public BaseStats Stats { get; set; }

        public abstract IInventory Inventory { get; set; }

        public IState CurrentState { get; set; }

        // --------------------------------------------------------------------
        // IMagicUser Implementations
        // --------------------------------------------------------------------

        public float Magic
        {
            get
            {
                return Stats.Magic.Value;
            }
            set
            {
                Stats.Magic.Value = value;
            }
        }

        public List<IMagicSkill> MagicSkills
        {
            get
            {
                return _magicSkills;
            }
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
            Magic -= magicSkill.Cost;
            _magicSkillEndTimes[MagicSkills.IndexOf(magicSkill)] = GameTime.time + magicSkill.CooldownTime;
            PreUseCountdown(magicSkill);
            return true;
        }

        /// <summary>
        /// A countdown before the MagicSkill takes action.</summary>
        /// <remarks>This can be used for syncing animations or effects.</remarks>
        /// <param name=magicSkill>The Skill to use</param>
        public virtual void PreUseCountdown (IMagicSkill magicSkill)
        {
            //
            // Implement a Coroutine in Monobehaviour
            //
            UseMagicSkill(magicSkill);
        }

        /// <summary>
        /// Triggers the use of the Skill</summary>
        /// <param name=magicSkill>The Skill to use</param>
        private void UseMagicSkill (IMagicSkill magicSkill)
        {
            magicSkill.Use(this);
        }

        /// <summary>
        /// Check if the character knows the magic skill, has enough
        /// magic energy and is not in cooldown of the Skill.</summary>
        /// <param name=magicSkill>The Skill to test</param>
        /// <returns> Whether the Skill van be used.</returns>
        private bool MagicSkillCanBeUsed (IMagicSkill magicSkill)
        {
            if (!MagicSkills.Contains(magicSkill))
            {
                return false;
            }
            if (Magic < magicSkill.Cost)
            {
                return false;
            }
            return GameTime.time >= _magicSkillEndTimes[MagicSkills.IndexOf(magicSkill)];
        }

        // --------------------------------------------------------------------
        // IFighter Implementations
        // --------------------------------------------------------------------

        public float Life
        {
            get
            {
                return Stats.Life.Value;
            }
            set
            {
                Stats.Life.Value = value;
            }
        }

        public List<IFighter> Enemies
        {
            get
            {
                return _enemies;
            }
        }

        public void AddEnemy (IFighter enemy)
        {
            Enemies.Add(enemy);
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
            _combatSkillEndTimes.Add(-1);
        }

        public bool TriggerCombatSkill (ICombatSkill combatSkill)
        {
            if (!CombatSkillCanBeUsed(combatSkill))
            {
                return false;
            }
            _combatSkillEndTimes[CombatSkills.IndexOf(combatSkill)] = GameTime.time + combatSkill.CooldownTime;
            PreUseCountdown(combatSkill);
            return true;
        }

        /// <summary>
        /// A countdown before the CombatSkill takes action.</summary>
        /// <remarks>This can be used for syncing animations or effects.</remarks>
        /// <param name=combatSkill>The Skill to use</param>
        public virtual void PreUseCountdown (ICombatSkill combatSkill)
        {
            //
            // Implement a Coroutine in Monobehaviour
            //
            UseCombatSkill(combatSkill);
        }

        /// <summary>
        /// Subtract the damage from the current life</summary>
        public void OnAttacked (IFighter attacker, float damage)
        {
            Life -= damage;
        }

        /// <summary>
        /// Triggers the use of the Skill</summary>
        /// <param name=combatSkill>The Skill to use</param>
        private void UseCombatSkill (ICombatSkill combatSkill)
        {
            combatSkill.Use(this);
        }

        /// <summary>
        /// Check if the character knows the combat skill, and is not
        // in cooldown of the Skill.</summary>
        /// <param name=combatSkill>The Skill to test</param>
        /// <returns> Whether the Skill van be used.</returns>
        private bool CombatSkillCanBeUsed (ICombatSkill combatSkill)
        {
            if (!CombatSkills.Contains(combatSkill))
            {
                return false;
            }
            return GameTime.time >= _combatSkillEndTimes[CombatSkills.IndexOf(combatSkill)];
        }

    }
    
    /// <summary>
    /// Representation of a Player controllable character.</summary>
    public class Player : BaseCharacter
    {
        IInventory _inventory;

        public Player() : base()
        {
            Stats = new PlayerStats();
            Inventory = new PlayerInventory();
        }

        public Player(string name) : base(name)
        {   
            Stats = new PlayerStats();
            Inventory = new PlayerInventory();
        }

        public override IInventory Inventory
        {
            get
            {
                return _inventory;
            }
            set
            {
                _inventory = value;
            }
        }
    }
    
    /// <summary>
    /// Representation of a Hostile, game controlled character.</summary>
    public class Enemy : BaseCharacter
    {
        IInventory _inventory;

        public Enemy() : base()
        {
            Stats = new EnemyStats();
            Inventory = new SimpleInventory();
        }

        public Enemy(string name) : base(name)
        {   
            Stats = new EnemyStats();
            Inventory = new SimpleInventory();
        }

        public override IInventory Inventory
        {
            get
            {
                return _inventory;
            }
            set
            {
                _inventory = value;
            }
        }
    }
}
