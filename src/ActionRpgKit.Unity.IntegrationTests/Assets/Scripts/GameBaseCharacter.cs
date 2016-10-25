﻿using UnityEngine;
using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;

public class GameBaseCharacter : MonoBehaviour
{
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
        Character.OnStateChanged += new StateChangedHandler(StateChangedTest);
        Character.OnMagicSkillLearned += new MagicSkillLearnedHandler(MagicSkillLearnedTest);
        Character.OnMagicSkillTriggered += new MagicSkillTriggeredHandler(MagicSkillTriggeredTest);
        Character.OnCombatSkillLearned += new CombatSkillLearnedHandler(CombatSkillLearnedTest);
        Character.OnCombatSkillTriggered += new CombatSkillTriggeredHandler(CombatSkillTriggeredTest);
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            attr.Value.OnValueChanged += new ValueChangedHandler(StatsChanged);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Character.Position.Set(transform.position.x, transform.position.y);
        Character.Update();
    }

    #endregion

    #region Character Events

    public virtual void StatsChanged(BaseAttribute sender, float value)
    {
    }

    public virtual void StateChangedTest(ICharacter sender, IState previousState, IState newState)
    {
        throw new NotImplementedException();
    }

    public virtual void MagicSkillLearnedTest(IMagicUser sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public virtual void MagicSkillTriggeredTest(IMagicUser sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public virtual void CombatSkillLearnedTest(IFighter sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public virtual void CombatSkillTriggeredTest(IFighter sender, int skillId)
    {
        throw new NotImplementedException();
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
        DebugExtension.DebugCircle(transform.position, color, Character.Stats.AlertnessRange.Value, 0);
        DebugExtension.DebugCircle(transform.position, color, Character.Stats.AttackRange.Value, 0);
    }
    #endif

}