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
    }

    /// <summary>
    /// Set the position of the ARPG Character to the position of the transform.</summary>
    public virtual void Update()
    {
        Character.Position.Set(transform.position.x, transform.position.z);
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
        throw new NotImplementedException();
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
