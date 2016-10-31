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
#if UNITY_EDITOR
            enemyData = (EnemyCharacterData)CharacterData;
#endif
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

    public override void StateChanged(ICharacter sender, IState previousState, IState newState)
    {
    }

    public override void MagicSkillTriggered(IMagicUser sender, int skillId)
    {
    }

    #endregion
}
