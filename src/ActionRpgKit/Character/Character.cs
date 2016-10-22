using System;
using System.Collections.Generic;
using ActionRpgKit.Core;
using ActionRpgKit.Item;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Character.Attribute;

namespace ActionRpgKit.Character
{
    #region Interfaces

    /// <summary>
    /// Characters populate the game world. 
    /// They are defined by Stats.</summary>
    public interface ICharacter
    {

        /// <summary>
        /// The active state of this Character.</summary>
        IState CurrentState { get; set; }

        /// <summary>
        /// Change the State to the given state if the given state differs
        /// from the current state.</summary>
        void ChangeState(IState state);
        
        /// <summary>
        /// Event is fired when a character changes state.</summary>
        event StateChangedHandler OnStateChanged;
    }

    /// <summary>
    /// Handler operates whenever an Character's state changes.</summary>
    /// <param name="sender">The sending ICharacter</param>
    /// <param name="previousState">The previos state</param>
    /// <param name="newState">The new state</param>
    public delegate void StateChangedHandler(ICharacter sender, IState previousState, IState newState);
    
    /// <summary>
    /// Character can use Magic.</summary>  
    public interface IMagicUser
    {
        /// <summary>
        /// MagicSkills available for this Character.</summary>
        List<int> MagicSkills { get; }

        /// <summary>
        /// Add a new MagicSkill.</summary>
        void LearnMagicSkill (int skillId);

        /// <summary>
        /// Trigger the given MagicSkill.</summary>
        bool TriggerMagicSkill (int skillId);
        
        /// <summary>
        /// Event is fired when a new MagicSkill is learned.</summary>
        event MagicSkillLearnedHandler OnMagicSkillLearned;
        
        /// <summary>
        /// Event is fired when an MagicSkill is triggered.</summary>
        event MagicSkillTriggeredHandler OnMagicSkillTriggered;
    }
    
    /// <summary>
    /// Handler operates whenever an IMagicUser learns a new MagicSkill.</summary>
    /// <param name="sender">The sending IMagicUser</param>
    /// <param name="skill">The learned MagicSkill</param>
    public delegate void MagicSkillLearnedHandler(IMagicUser sender, int skillId);
    
    /// <summary>
    /// Handler operates whenever an IMagicUser triggers an MagicSkill.</summary>
    /// <param name="sender">The sending IMagicUser</param>
    /// <param name="skill">The triggered MagicSkill</param>
    public delegate void MagicSkillTriggeredHandler(IMagicUser sender, int skillId);
    
    /// <summary>
    /// Character can fight.</summary>  
    public interface IFighter
    {
        /// <summary>
        /// World space position of the IFighter.</summary>
        Position Position { get; set; }

        /// <summary>
        /// The remaining life.</summary>
        float Life { get; set; }

        /// <summary>
        /// Whether the Fighter has been killed.</summary>
        bool IsDead{ get; set; }

        /// <summary>
        /// Targeted enemies of the fighter.</summary>
        List<IFighter> Enemies { get; }

        /// <summary>
        /// All enemies currently in Attack Range.</summary>
        IFighter[] EnemiesInAttackRange { get; }

        /// <summary>
        /// A timestamp representing the next possible moment for an attack.</summary>
        float TimeUntilNextAttack { get; set; }

        /// <summary>
        /// The current Skill that is to be used for an Attack.</summary>
        int CurrentAttackSkill { get; set; }

        /// <summary>
        /// The currently equipped Weapon.</summary>
        int EquippedWeapon { get; set; }

        /// <summary>
        /// Add a new enemy.</summary>
        void AddEnemy(IFighter enemy, int index);

        /// <summary>
        /// Remove an enemy.</summary>
        void RemoveEnemy(IFighter enemy);

        /// <summary>
        /// Whether any enemy is in sight range.
        /// Also checks whether the assigned enemies are still in alertness
        /// range and de-registers them if not.</summary>
        bool EnemyInAlternessRange();

        /// <summary>
        /// Whether the Character is in range for an attack.</summary>
        bool EnemyInAttackRange(IFighter enemy);

        /// <summary>
        /// Whether the Character can attack depends on the time since the
        /// last attack.</summary>
        bool CanAttack();

        /// <summary>
        /// Attack the given IFighter.</summary>
        void Attack(IFighter enemy);

        /// <summary>
        /// CombatSkills available for this Character.</summary>
        List<int> CombatSkills { get; }

        /// <summary>
        /// Add a new CombatSkill.</summary>
        void LearnCombatSkill(int skillId);

        /// <summary>
        /// Attack with the given CombatSkill.</summary>
        bool TriggerCombatSkill(int skillId);

        /// <summary>
        /// The Fighter is being attacked.</summary>
        void OnAttacked(IFighter attacker, float damage);

        /// <summary>
        /// Event is fired when a new CombatSkill is learned.</summary>
        event CombatSkillLearnedHandler OnCombatSkillLearned;
        
        /// <summary>
        /// Event is fired when an iCombatSkill is triggered.</summary>
        event CombatSkillTriggeredHandler OnCombatSkillTriggered;
    }

    /// <summary>
    /// Handler operates whenever a IFighter learns a new CombatSkill.</summary>
    /// <param name="sender">The sending IFighter</param>
    /// <param name="skill">The learned CombatSkill</param>
    public delegate void CombatSkillLearnedHandler(IFighter sender, int skillId);
    
    /// <summary>
    /// Handler operates whenever a Character's state changes.</summary>
    /// <param name="sender">The sending IFighter</param>
    /// <param name="skill">The triggered CombatSkill</param>
    public delegate void CombatSkillTriggeredHandler(IFighter sender, int skillId);

    #endregion

    #region Abstracts

    /// <summary>
    /// Base implementation of a Character.</summary>
    [Serializable]
    public abstract class BaseCharacter : IGameObject, ICharacter, IMagicUser, IFighter
    {
        /// <summary>
        /// Id of the Charcter.</summary>
        public int Id;

        /// <summary>
        /// Name of the character.</summary>
        public string Name;

        /// <summary>
        /// Stats describing the Character.</summary>
        public BaseStats Stats;

        /// <summary>
        /// Inventory of the Character.</summary>
        public IInventory Inventory;

        public event StateChangedHandler OnStateChanged;
        public event MagicSkillLearnedHandler OnMagicSkillLearned;
        public event MagicSkillTriggeredHandler OnMagicSkillTriggered;
        public event CombatSkillLearnedHandler OnCombatSkillLearned;
        public event CombatSkillTriggeredHandler OnCombatSkillTriggered;

        [NonSerialized]
        public IdleState _idleState;
        [NonSerialized]
        public AlertState _alertState;
        [NonSerialized]
        public ChaseState _chaseState;
        [NonSerialized]
        public AttackState _attackState;
        [NonSerialized]
        public DyingState _dyingState;

        private List<int> _magicSkills = new List<int>();
        private List<float> _magicSkillEndTimes = new List<float>();

        private List<IFighter> _enemies = new List<IFighter>();
        private List<int> _combatSkills = new List<int>();
        private List<float> _combatSkillEndTimes = new List<float>();

        public BaseCharacter() { }

        public BaseCharacter(BaseStats stats, IInventory inventory)
        {
            Stats = stats;
            Inventory = inventory;
            _idleState = new IdleState();
            _alertState = new AlertState();
            _chaseState = new ChaseState();
            _attackState = new AttackState();
            _dyingState = new DyingState();
            CurrentState = _idleState;
            
            // Connect internal signals
            Stats.Life.OnMinReached += new MinReachedHandler(OnDeath);
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

        #region IGameObject Implementations

        public Position Position { get; set; } = new Position();

        public void Update ()
        {
            CurrentState.UpdateState(this);
        }

        #endregion

        #region ICharacter Implementations

        public IState CurrentState { get; set; }

        /// <summary>
        /// Change the State to the given State if the given State differs
        /// from the current State.</summary>
        public void ChangeState(IState state)
        {
            if (state != CurrentState)
            {
                var previousState = CurrentState;
                CurrentState.ExitState(this);
                CurrentState = state;
                CurrentState.EnterState(this);
                EmitOnStateChanged(previousState, CurrentState);
            }
        }

        protected void EmitOnStateChanged(IState previousState, IState newState)
        {
            var handler = OnStateChanged;
            if (handler != null)
            {
                handler(this, previousState, newState);
            }
        }
        
        protected void EmitOnMagicSkillLearned(int skillId)
        {
            var handler = OnMagicSkillLearned;
            if (handler != null)
            {
                handler(this, skillId);
            }
        }
        
        protected void EmitOnMagicSkillTriggered(int skillId)
        {
            var handler = OnMagicSkillTriggered;
            if (handler != null)
            {
                handler(this, skillId);
            }
        }
        
        protected void EmitOnCombatSkillLearned(int skillId)
        {
            var handler = OnCombatSkillLearned;
            if (handler != null)
            {
                handler(this, skillId);
            }
        }
        
        protected void EmitOnCombatSkillTriggered(int skillId)
        {
            var handler = OnCombatSkillTriggered;
            if (handler != null)
            {
                handler(this, skillId);
            }
        }

        #endregion 
        
        #region IMagicUser Implementations

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

        public List<int> MagicSkills
        {
            get
            {
                return _magicSkills;
            }
        }

        public void LearnMagicSkill(int skillId)
        {
            if (!MagicSkills.Contains(skillId))
            {
                _magicSkills.Add(skillId);
                _magicSkillEndTimes.Add(-1);
                EmitOnMagicSkillLearned(skillId);
            }
        }

        public bool TriggerMagicSkill(int skillId)
        {
            if (!MagicSkillCanBeUsed(skillId))
            {
                return false;
            }
            var magicSkill = SkillDatabase.GetMagicSkillById(skillId);
            Magic -= magicSkill.Cost;
            _magicSkillEndTimes[MagicSkills.IndexOf(skillId)] = GameTime.time + magicSkill.CooldownTime;
            PreUseCountdown(magicSkill);
            EmitOnMagicSkillTriggered(skillId);
            return true;
        }

        /// <summary>
        /// A countdown before the MagicSkill takes action.</summary>
        /// <remarks>This can be used for syncing animations or effects.</remarks>
        /// <param name=magicSkill>The Skill to use</param>
        public virtual void PreUseCountdown(MagicSkill magicSkill)
        {
            //
            // Implement a Coroutine in Monobehaviour
            //
            UseMagicSkill(magicSkill);
        }

        /// <summary>
        /// Triggers the use of the Skill</summary>
        /// <param name=magicSkill>The Skill to use</param>
        private void UseMagicSkill(MagicSkill magicSkill)
        {
            magicSkill.Use(this);
        }

        /// <summary>
        /// Check if the character knows the magic skill, has enough
        /// magic energy and is not in cooldown of the Skill.</summary>
        /// <param name=magicSkill>The Skill to test</param>
        /// <returns> Whether the Skill van be used.</returns>
        private bool MagicSkillCanBeUsed(int skillId)
        {
            if (!MagicSkills.Contains(skillId))
            {
                return false;
            }
            var magicSkill = SkillDatabase.GetMagicSkillById(skillId);
            if (Magic < magicSkill.Cost)
            {
                return false;
            }
            return GameTime.time >= _magicSkillEndTimes[MagicSkills.IndexOf(skillId)];
        }

        #endregion 

        #region IFighter Implementations

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

        public bool IsDead { get; set; }

        public List<IFighter> Enemies
        {
            get
            {
                return _enemies;
            }
        }

        public float TimeUntilNextAttack { get; set; }

        public int CurrentAttackSkill { get; set; }

        public int EquippedWeapon { get; set; } = -1;

        public void AddEnemy(IFighter enemy, int index=0)
        {
            if (!Enemies.Contains(enemy))
            {
                Enemies.Insert(index, enemy);
            }
        }

        public void RemoveEnemy(IFighter enemy)
        {
            if (Enemies.Contains(enemy))
            {
                Enemies.Remove(enemy);
            }
        }

        public bool EnemyInAlternessRange()
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                var enemy = Enemies[i];
                if (Position.SquaredDistanceTo(Enemies[i].Position) > Stats.AlertnessRange.Value)
                {
                    RemoveEnemy(Enemies[i]);
                    continue;
                }
                Enemies.Remove(enemy);
                Enemies.Insert(0, enemy);
                return true;
            }
            return false;
        }

        public bool EnemyInAttackRange(IFighter enemy)
        {
            float range = Stats.AttackRange.Value;
            if (EquippedWeapon > -1)
            {
                range += ItemDatabase.GetWeaponItemById(EquippedWeapon).Range;
            }
            return Position.SquaredDistanceTo(enemy.Position) <= range;
        }

        public IFighter[] EnemiesInAttackRange
        {
            get
            {
                var enemiesInAttackRange = new List<IFighter>();
                for(int i = 0; i < Enemies.Count; i++)
                {
                    if(EnemyInAttackRange(Enemies[i]))
                    {
                        enemiesInAttackRange.Add(Enemies[i]);
                    }
                }
                return enemiesInAttackRange.ToArray();
            }
        }

        public bool CanAttack()
        {
            return GameTime.time > TimeUntilNextAttack;
        }

        public void Attack(IFighter enemy)
        {
            TriggerCombatSkill(CurrentAttackSkill);
        }

        public List<int> CombatSkills
        {
            get
            {
                return _combatSkills;
            }
        }

        public void LearnCombatSkill(int skillId)
        {
            if (!CombatSkills.Contains(skillId))
            {
                _combatSkills.Add(skillId);
                _combatSkillEndTimes.Add(-1);
                EmitOnCombatSkillLearned(skillId);
            }
        }

        public bool TriggerCombatSkill(int skillId)
        {
            if (!CombatSkillCanBeUsed(skillId))
            {
                return false;
            }
            var combatSkill = SkillDatabase.GetCombatSkillById(skillId);
            float endTime = GameTime.time + combatSkill.CooldownTime;
            if (EquippedWeapon > -1)
            {
                endTime += 1 / ItemDatabase.GetWeaponItemById(EquippedWeapon).Speed;
            }  
            _combatSkillEndTimes[CombatSkills.IndexOf(skillId)] = endTime;
            PreUseCountdown(combatSkill);
            EmitOnCombatSkillTriggered(skillId);
            return true;
        }

        /// <summary>
        /// A countdown before the CombatSkill takes action.</summary>
        /// <remarks>This can be used for syncing animations or effects.</remarks>
        /// <param name=combatSkill>The Skill to use</param>
        public virtual void PreUseCountdown(CombatSkill combatSkill)
        {
            //
            // Implement a Coroutine in Monobehaviour
            //
            UseCombatSkill(combatSkill);
        }

        /// <summary>
        /// Subtract the damage from the current life</summary>
        public void OnAttacked(IFighter attacker, float damage)
        {
            Life -= damage;
            AddEnemy(attacker);
        }

        /// <summary>
        /// Triggers the use of the Skill</summary>
        /// <param name=combatSkill>The Skill to use</param>
        private void UseCombatSkill(CombatSkill combatSkill)
        {
            combatSkill.Use(this);
        }

        /// <summary>
        /// Check if the character knows the combat skill, and is not
        /// in cooldown of the Skill.</summary>
        /// <param name=combatSkill>The Skill to test</param>
        /// <returns> Whether the Skill van be used.</returns>
        private bool CombatSkillCanBeUsed(int skillId)
        {
            if (!CombatSkills.Contains(skillId))
            {
                return false;
            }
            if (EnemiesInAttackRange.Length == 0)
            {
                return false;
            }
            return GameTime.time >= _combatSkillEndTimes[CombatSkills.IndexOf(skillId)];
        }
        
        /// <summary>
        /// The Character has just been killed.</summary>
        private void OnDeath(BaseAttribute sender)
        {
            ChangeState(_dyingState);
            IsDead = true;
            for (int i=0; i<Enemies.Count; i++)
            {
                Enemies[i].RemoveEnemy(this);
            }
        }

        #endregion
    }

    #endregion

    #region Implementations

    /// <summary>
    /// Representation of a Player controllable character.</summary>
    [Serializable]
    public class Player : BaseCharacter
    {

        public Player() : base(new PlayerStats(), new PlayerInventory())
        {
            Inventory.Items = new List<int>();
        }

        public Player(string name) : base(new PlayerStats(), new PlayerInventory())
        {
            Name = name;
            Inventory.Items = new List<int>();
        }
    }

    /// <summary>
    /// Representation of a Hostile, game controlled character.</summary>
    [Serializable]
    public class Enemy : BaseCharacter
    {

        public Enemy() : base(new EnemyStats(), new SimpleInventory())
        {
            Inventory.Items = new int[] { };
        }

        public Enemy(string name) : base(new EnemyStats(), new SimpleInventory())
        {
            Name = name;
            Inventory.Items = new int[] { };
        }
    }

    #endregion
}
