using UnityEngine;
using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;

public class GameEnemy : GameBaseCharacter
{

    private EnemyCharacterData enemyData;

    public override BaseCharacter Character
    {
        get
        {
            return enemyData.Character;
        }
    }

    #region Monobehaviour

    new void Awake()
    {
        enemyData = (EnemyCharacterData)CharacterData;
        base.Awake();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    #endregion

    #region Character Events

    public override void StatsChanged(BaseAttribute sender, float value)
    {
    }

    public override void StateChangedTest(ICharacter sender, IState previousState, IState newState)
    {
        throw new NotImplementedException();
    }

    public override void MagicSkillTriggeredTest(IMagicUser sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public override void CombatSkillTriggeredTest(IFighter sender, int skillId)
    {
        throw new NotImplementedException();
    }

    #endregion
}
