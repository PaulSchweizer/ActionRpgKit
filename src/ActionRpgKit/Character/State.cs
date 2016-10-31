using System;

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
        public static readonly IdleState Instance = new IdleState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count > 0)
            {
                character.ChangeState(character.AlertState);
            }
        }
    }

    [Serializable]
    public sealed class AlertState : IState
    {
        public static readonly AlertState Instance = new AlertState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count == 0)
            {
                character.ChangeState(character.IdleState);
                return;
            }

            character.ChangeState(character.ChaseState);
        }
    }

    [Serializable]
    public sealed class ChaseState : IState
    {
        public static readonly ChaseState Instance = new ChaseState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

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

    [Serializable]
    public sealed class AttackState : IState
    {

        public static readonly AttackState Instance = new AttackState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

        public void UpdateState(BaseCharacter character)
        {

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

    [Serializable]
    public sealed class DyingState : IState
    {
        public static readonly DyingState Instance = new DyingState();

        public void EnterState(BaseCharacter character) { }

        public void ExitState(BaseCharacter character) { }

        public void UpdateState(BaseCharacter character) { }
    }

    #endregion
}
