using System;
using ActionRpgKit.Core;

namespace ActionRpgKit.Character
{

    #region Interfaces

    /// <summary>
    /// Determine the state of a Character.</summary>
    public interface IState
    {
        /// <summary>
        /// Called when entering the State.</summary>
        void EnterState(BaseCharacter character);

        /// <summary>
        /// Called right before changing to the next State.</summary>
        void ExitState(BaseCharacter character);

        /// <summary>
        /// Called to perform the interal calculation of the IState.</summary>
        void UpdateState(BaseCharacter character);
    }

    #endregion

    #region Implementations

    /// <summary>
    /// The initial State of a Character.</summary>
    [Serializable]
    public sealed class IdleState : IState
    {
        /// <summary>
        /// Keeping the class a singleton.</summary>
        public static readonly IdleState Instance = new IdleState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

        /// <summary>
        /// Check if Enemies are in AlertnessRange.</summary>
        /// <param name="character">The Character</param>
        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count > 0)
            {
                character.ChangeState(character.AlertState);
            }
            else
            {
                if (character.IsMoving)
                {
                    character.ChangeState(character.MoveState);
                }
            }
        }
    }

    /// <summary>
    /// The Character is moving just from point A to point B.</summary>
    [Serializable]
    public sealed class MoveState : IState
    {
        /// <summary>
        /// Keeping the class a singleton.</summary>
        public static readonly MoveState Instance = new MoveState();

        /// <summary>
        /// No actions need to be taken on enter.</summary>
        /// <param name="character"></param>
        public void EnterState(BaseCharacter character) { }

        /// <summary>
        /// Start moving to the desired location.</summary>
        /// <param name="character"></param>
        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count > 0)
            {
                character.ChangeState(character.AlertState);
            }
            else
            {
                if (!character.IsMoving)
                {
                    character.ChangeState(character.IdleState);
                }
            }
        }

        /// <summary>
        /// No actions need to be taken on exit.</summary>
        /// <param name="character"></param>
        public void ExitState(BaseCharacter character) { }
    }

    /// <summary>
    /// The Character is aware of Enemies in his close vicinity.
    /// A timer controls when the Character will detect and then start to
    /// chase the Enemy.</summary>
    [Serializable]
    public sealed class AlertState : IState
    {
        /// <summary>
        /// Keeping the class a singleton.</summary>
        public static readonly AlertState Instance = new AlertState();

        /// <summary>
        /// Determine the end time of the alertness period.</summary>
        /// <param name="character"></param>
        public void EnterState(BaseCharacter character)
        {
            character.AlertnessEndTime = GameTime.time + 10 - character.AlertnessLevel;
        }

        /// <summary>
        /// Start chasing the Enemies in range.</summary>
        /// <param name="character"></param>
        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count == 0)
            {
                character.ChangeState(character.IdleState);
                return;
            }
            if (GameTime.time >= character.AlertnessEndTime)
            {
                character.ChangeState(character.ChaseState);
            }
        }

        /// <summary>
        /// No actions need to be taken on exit.</summary>
        /// <param name="character"></param>
        public void ExitState(BaseCharacter character) { }
    }

    /// <summary>
    /// The Character chases after an Enemy.</summary>
    [Serializable]
    public sealed class ChaseState : IState
    {
        /// <summary>
        /// Keeping the class a singleton.</summary>
        public static readonly ChaseState Instance = new ChaseState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

        /// <summary>
        /// Chase the Enemy until he is in attack range.</summary>
        /// <param name="character">The Character</param>
        /// <todo>Include a maximum chase time.</todo>
        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count == 0)
            {
                character.ChangeState(character.AlertState);
                return;
            }

            if (character.EnemiesInAttackRange.Contains(character.Enemies[0]))
            {
                character.ChangeState(character.AttackState);
            }
        }
    }

    /// <summary>
    /// An Enemy is in attack range.</summary>
    [Serializable]
    public sealed class AttackState : IState
    {
        /// <summary>
        /// Keeping the class a singleton.</summary>
        public static readonly AttackState Instance = new AttackState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

        /// <summary>
        /// Attack the first Enemy in range.</summary>
        /// <param name="character"></param>
        public void UpdateState(BaseCharacter character)
        {
            if (character.IsMoving)
            {
                character.ChangeState(character.MoveState);
                return;
            }

            if (character.Enemies.Count == 0)
            {
                character.ChangeState(character.AlertState);
                return;
            }

            // If not in Attack Range any more, chase the enemy
            if (!character.EnemiesInAttackRange.Contains(character.Enemies[0]))
            {
                character.ChangeState(character.ChaseState);
                return;
            }

            // Attack
            if (character.CanAttack())
            {
                character.Attack(character.Enemies[0]);
            }
        }
    }

    /// <summary>
    /// The Character has been defeated.</summary>
    [Serializable]
    public sealed class DyingState : IState
    {
        /// <summary>
        /// Keeping the class a singleton.</summary>
        public static readonly DyingState Instance = new DyingState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

        public void UpdateState(BaseCharacter character) { }
    }

    #endregion
}
