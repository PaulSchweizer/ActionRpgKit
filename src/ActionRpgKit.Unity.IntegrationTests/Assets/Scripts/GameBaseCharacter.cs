using UnityEngine;
using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;
using System.Collections;
using ActionRpgKit.Character.Skill;

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
        Character.OnStateChanged += new StateChangedHandler(StateChanged);
        Character.OnMagicSkillLearned += new MagicSkillLearnedHandler(MagicSkillLearned);
        Character.OnMagicSkillTriggered += new MagicSkillTriggeredHandler(MagicSkillTriggered);
        Character.OnCombatSkillLearned += new CombatSkillLearnedHandler(CombatSkillLearned);
        Character.OnCombatSkillTriggered += new CombatSkillTriggeredHandler(CombatSkillTriggered);
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            attr.Value.OnValueChanged += new ValueChangedHandler(StatsChanged);
        }

        Character.LearnCombatSkill(0);
        Character.CurrentAttackSkill = 0;
        Character.CombatSkillEndTimes.Clear();
        for(int i = 0; i < Character.CombatSkills.Count; i++)
        {
            Character.CombatSkillEndTimes.Add(-1);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Character.Position.Set(transform.position.x, transform.position.y);
    }

    #endregion

    #region Character Events

    public virtual void StatsChanged(BaseAttribute sender, float value)
    {
    }

    public virtual void StateChanged(ICharacter sender, IState previousState, IState newState)
    {
        throw new NotImplementedException();
    }

    public virtual void MagicSkillLearned(IMagicUser sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public virtual void MagicSkillTriggered(IMagicUser sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public virtual void CombatSkillLearned(IFighter sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public virtual void CombatSkillTriggered(IFighter sender, int skillId)
    {
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
        DebugExtension.DebugCircle(transform.position, color, 
                (float)Math.Sqrt(Character.Stats.AttackRange.Value), 0);
    }
#endif

}
