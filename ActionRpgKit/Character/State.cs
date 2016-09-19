﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// Determine the state of a Character.</summary>
    public interface IState
    {
        /// <summary>
        /// Called when entering the State.</summary>
        void EnterState();

        /// <summary>
        /// Called right before changing to the next State.</summary>
        void ExitState();

        /// <summary>
        /// Called to perform the interal calculation of the IState.</summary>
        void UpdateState(BaseCharacter character);
    }

    /// <summary>
    /// The initial State of a Character.</summary>
    public class IdleState : IState
    {
        public void EnterState() { }

        public void ExitState() { }

        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count > 0)
            {
                character.ChangeState(character._alertState);
            }
        }
    }

    public class AlertState : IState
    {
        public void EnterState() { }

        public void ExitState() { }

        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count == 0)
            {
                character.ChangeState(character._idleState);
                return;
            }

            if (character.EnemyInAlternessRange())
            {
                character.ChangeState(character._chaseState);
            }
        }
    }

    public class ChaseState : IState
    {
        public void EnterState() { }

        public void ExitState() { }

        public void UpdateState(BaseCharacter character)
        {
            if (character.Enemies.Count == 0)
            {
                character.ChangeState(character._alertState);
                return;
            }

            if (character.EnemyInAttackRange(character.Enemies[0]))
            {
                character.ChangeState(character._attackState);
            }
        }
    }

    public class AttackState : IState
    {
        public void EnterState() { }

        public void ExitState() { }

        public void UpdateState(BaseCharacter character)
        {

            if (character.Enemies.Count == 0)
            {
                character.ChangeState(character._alertState);
                return;
            }

            // If not in Attack Range any more, chase the enemy
            if (!character.EnemyInAttackRange(character.Enemies[0]))
            {
                character.ChangeState(character._chaseState);
                return;
            }

            // Attack
            if (character.CanAttack())
            {
                character.Attack(character.Enemies[0]);
            }
        }
    }
}
