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
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                enemyData = (EnemyCharacterData)CharacterData;
            }
#endif
            return enemyData.Character;
        }
    }

    #region Monobehaviour

    new void Awake()
    {
        enemyData = (EnemyCharacterData)Instantiate(CharacterData);
        base.Awake();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    #endregion

    #region Character Events

    #endregion
}
