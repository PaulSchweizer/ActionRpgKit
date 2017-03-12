using ActionRpgKit.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BerettaEffect : BaseEffect
{
    public GameObject Fire;
    public float EffectTime;

    public void Awake()
    {
        Fire.SetActive(false);
    }

    public override void Trigger(IFighter sender, int skillId)
    {
        var offset = ActionRpgKitController.Instance.SkillDatabase.GenericCombatSkillsData[skillId].HitTime;
        StopCoroutine("EffectCountdown");
        StartCoroutine(EffectCountdown(offset));
    }

    /// <summary>
    /// Run a countdown to remove the effect after a skill has been used.</summary>
    public IEnumerator EffectCountdown(float offset)
    {
        float startTime = Time.time + offset;
        while (Time.time < startTime)
        {
            yield return null;
        }
        Fire.SetActive(true);
        float endTime = Time.time + EffectTime;
        while (Time.time < endTime)
        {
            yield return null;
        }
        Fire.SetActive(false);
    }
}
