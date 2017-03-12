using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "DefeatEnemiesQuest", menuName = "ActionRpgKit/Quest/DefeatEnemies")]
public class DefeatEnemiesQuest : UQuest
{
    public int enemyId;

    public int quantity;

    private int killedQuantity;

    private EnemyDefeatedHandler handler;

    public override void OnStarted()
    {
        killedQuantity = 0;
        handler = new EnemyDefeatedHandler(EnemyDefeated);
        GamePlayer.Instance.playerData.Character.OnEnemyDefeated += handler;
    }

    public void EnemyDefeated(IFighter enemy)
    {
        var character = (BaseCharacter)enemy;
        if (character.Id == enemyId)
        {
            killedQuantity += 1;
        }
    }

    public override bool CheckProgress()
    {
        if (killedQuantity >= quantity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnCompletion()
    {
        GamePlayer.Instance.playerData.Character.OnEnemyDefeated -= handler;
    }
}