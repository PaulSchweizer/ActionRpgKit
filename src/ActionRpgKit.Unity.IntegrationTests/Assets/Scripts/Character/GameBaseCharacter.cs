using UnityEngine;
using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;
using System.Collections;

public class GameBaseCharacter : MonoBehaviour
{
    // Unity related fields
    public NavMeshAgent NavMeshAgent;
    public Animator Animator;
    private GameCharacterState CurrentState;
    private GameIdleState IdleState = GameIdleState.Instance;
    private GameMoveState MoveState = GameMoveState.Instance;
    private GameAlertState AlertState = GameAlertState.Instance;

    // ActionRpgKit related fields
    public BaseCharacterData CharacterData;

    public virtual BaseCharacter Character
    {
        get
        {
            return CharacterData.Character;
        }
    }

    #region Monobehaviour

    public virtual void Awake()
    {
        // Connect the signals fromt the ActionRpgKit Character
        Character.OnStateChanged += new StateChangedHandler(StateChanged);
        Character.OnMagicSkillTriggered += new MagicSkillTriggeredHandler(MagicSkillTriggered);
        Character.OnCombatSkillTriggered += new CombatSkillTriggeredHandler(CombatSkillTriggered);
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            attr.Value.OnValueChanged += new ValueChangedHandler(StatsChanged);
        }

        //Fix Deserialization Problems
        Character.CombatSkillEndTimes.Clear();
        for (int i = 0; i < Character.CombatSkills.Count; i++)
        {
            Character.CombatSkillEndTimes.Add(-1);
        }

        // NavMeshAgent
        Character.Stats.MovementSpeed.OnValueChanged += new ValueChangedHandler(SpeedChanged);
        SpeedChanged(Character.Stats.MovementSpeed, Character.Stats.MovementSpeed.Value);

        // State
        CurrentState = IdleState;
    }

    /// <summary>
    /// Set the position of the ARPG Character to the position of the transform.</summary>
    public virtual void Update()
    {
        // Set the position of the Arpg Character
        Character.Position.Set(transform.position.x, transform.position.z);

        // Update the current state
        CurrentState.Update(this);
    }

    #endregion

    #region Character Events

    /// <summary>
    /// Update the Speed and dependent values on the navMeshAgent.</summary>
    /// <param name="sender">The MovementSpeed Attribute</param>
    /// <param name="value">The value</param>
    public void SpeedChanged(BaseAttribute sender, float value)
    {
        NavMeshAgent.speed = value;
        NavMeshAgent.angularSpeed = value * 100;
    }

    /// <summary>
    /// Update the visual display of the Stats and trigger other dependent processes.</summary>
    /// <param name="sender">The Attribute</param>
    /// <param name="value">The value of gthe Attribute</param>
    public virtual void StatsChanged(BaseAttribute sender, float value)
    {
    }

    public virtual void StateChanged(ICharacter sender, IState previousState, IState newState)
    {
        CurrentState.Exit(this);
        if (newState is IdleState)
        {
            CurrentState = IdleState;
        }
        else if (newState is MoveState)
        {
            CurrentState = MoveState;
        }
        else if (newState is AlertState)
        {
            CurrentState = AlertState;
        }
        else
        {
            CurrentState = AlertState;
        }
        CurrentState.Enter(this);
    }

    public virtual void MagicSkillTriggered(IMagicUser sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public virtual void CombatSkillTriggered(IFighter sender, int skillId)
    {
        var target = new Vector3(Character.Enemies[0].Position.X, 0, 
                                 Character.Enemies[0].Position.Y);
        transform.LookAt(target);
        StartCoroutine(CombatSkillCountdown(sender, skillId));
    }

    /// <summary>
    /// Run a countdown after a skill has been used.</summary>
    private IEnumerator CombatSkillCountdown(IFighter sender, int skillId)
    {
        float endTime = Character.CombatSkillEndTimes[Character.CombatSkills.IndexOf(skillId)];
        while (Time.time < endTime)
        {
            yield return null;
        }
        Character.UseCombatSkill(skillId);
    }

    #endregion


#if UNITY_EDITOR
    /// <summary>
    /// Draw some Debug helper shapes.</summary>
    void OnDrawGizmos()
    {
        Color color;
        if (Character.CurrentState is IdleState)
        {
            color = Color.green;
        }
        else if (Character.CurrentState is MoveState)
        {
            color = Color.blue;
        }
        else if (Character.CurrentState is AlertState)
        {
            color = Color.yellow;
        }
        else if (Character.CurrentState is ChaseState)
        {
            color = Color.magenta;
        }
        else if (Character.CurrentState is AttackState)
        {
            color = Color.red;
        }
        else if (Character.CurrentState is DyingState)
        {
            color = Color.black;
        }
        else
        {
            color = Color.grey;
        }
        // Draw a circle for sight range
        DebugExtension.DebugCircle(transform.position, color, 
                (float)Math.Sqrt(Character.Stats.AlertnessRange.Value), 0);
        if (UnityEditor.EditorApplication.isPlaying)
        {
            DebugExtension.DebugCircle(transform.position, color, 
                    (float)Math.Sqrt(Character.AttackRange), 0);
        }
    }
#endif

}


public abstract class GameCharacterState
{
    /// <summary>
    /// Called when entering the State.</summary>
    public virtual void Enter(GameBaseCharacter character) { }

    /// <summary>
    /// Called every frame when the State is active.</summary>
    public abstract void Update(GameBaseCharacter character);

    /// <summary>
    /// Called before changing to the next State.</summary>
    public virtual void Exit(GameBaseCharacter character) { }
}


public class GameIdleState : GameCharacterState
{
    public static GameIdleState Instance = new GameIdleState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.NavMeshAgent.Stop();
        character.Animator.SetBool("Idle", true);
    }

    /// <summary>
    /// Bring the movement speed down to a still stand.</summary>
    public override void Update(GameBaseCharacter character)
    {
        if (character.Animator.GetFloat("MovementSpeed") > 0)
        {
            character.Animator.SetFloat("MovementSpeed", 
                Math.Max(0, character.Animator.GetFloat("MovementSpeed") - Time.deltaTime * 6));
        }
    }

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("Idle", false);
    }
}


public class GameMoveState : GameCharacterState
{
    public static GameMoveState Instance = new GameMoveState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("Move", true);
        character.Character.IsMoving = true;
    }

    /// <summary>
    /// Bring the movement speed down to a still stand.</summary>
    public override void Update(GameBaseCharacter character)
    {
        if (character.NavMeshAgent.pathPending)
        {
            return;
        }
        if (character.Animator.GetFloat("MovementSpeed") < 1)
        {
            character.Animator.SetFloat("MovementSpeed", 
                Math.Min(1, character.Animator.GetFloat("MovementSpeed") + Time.deltaTime * 6));
        }

        if (character.NavMeshAgent.remainingDistance <= character.NavMeshAgent.stoppingDistance)
        {
            if (!character.NavMeshAgent.hasPath || Mathf.Abs(character.NavMeshAgent.velocity.sqrMagnitude) < float.Epsilon)
            {
                character.Character.IsMoving = false;
            }
        }
    }

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("Move", false);
        character.Character.IsMoving = false;
    }
}


public class GameAlertState : GameCharacterState
{
    public static GameAlertState Instance = new GameAlertState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.NavMeshAgent.Stop();
        character.Animator.SetBool("Alert", true);
    }

    /// <summary>
    /// Bring the movement speed down to a still stand.</summary>
    public override void Update(GameBaseCharacter character)
    {
        if (character.Animator.GetFloat("MovementSpeed") > 0)
        {
            character.Animator.SetFloat("MovementSpeed", 
                Math.Max(0, character.Animator.GetFloat("MovementSpeed") - Time.deltaTime * 6));
        }
    }

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("Alert", false);
    }
}