using ActionRpgKit.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FireEffect : BaseEffect
{

    public float EffectTime = 1;

    public ParticleSystem[] Fires; 

    public override void Trigger(IMagicUser sender, int skillId)
    {
        var offset = ActionRpgKitController.Instance.SkillDatabase.GetMagicSkillById(skillId).CastingTime;
        StopCoroutine("EffectCountdown");
        StartCoroutine(EffectCountdown(offset));
    }

    /// <summary>
    /// Run a countdown to remove the effect after a skill has been used.</summary>
    public IEnumerator EffectCountdown(float offset)
    {
        int i = 0;
        foreach (var enemy in GamePlayer.Instance.Character.EnemiesInAttackRange)
        {
            if (i > 1)
            {
                break;
            }
            Fires[i].transform.parent = null;
            Fires[i].transform.position = new Vector3(enemy.Position.X, 0, enemy.Position.Y);
            i += 1;
        }
        Fires[0].transform.parent = GamePlayer.Instance.transform;
        Fires[1].transform.parent = GamePlayer.Instance.transform;

        float startTime = Time.time + offset;
        while (Time.time < startTime)
        {
            yield return null;
        }

        for (int j = 0; j < i; j++)
        {
            Fires[j].Play();
        }

        float endTime = Time.time + EffectTime;
        while (Time.time < endTime)
        {
            yield return null;
        }

        Fires[0].Stop();
        Fires[1].Stop();
    }
}
