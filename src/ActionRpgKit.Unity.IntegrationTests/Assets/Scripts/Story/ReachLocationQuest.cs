using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "ReachLocationQuest", menuName = "ActionRpgKit/Quest/ReachLocation")]
public class ReachLocationQuest : UQuest
{
    public float x;

    public float y;

    public float radius;

    public override bool CheckProgress()
    {
        var player_pos = GamePlayer.Instance.transform.position;
        var dist = (x - player_pos.x) * (x - player_pos.x) + (y - player_pos.z) * (y - player_pos.z);
        if (dist < radius * (GamePlayer.Instance.NavMeshAgent.radius * GamePlayer.Instance.NavMeshAgent.radius))
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
        Debug.Log("Location Reached");
    }
}