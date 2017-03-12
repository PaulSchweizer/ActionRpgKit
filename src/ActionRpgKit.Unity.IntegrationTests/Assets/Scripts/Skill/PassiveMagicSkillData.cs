using UnityEngine;
using ActionRpgKit.Character.Skill;

public class PassiveMagicSkillData : SkillData
{
    /// <summary>
    /// The ARPG Skill object.</summary>
    public PassiveMagicSkill Skill;

    public OffensiveMagicSkill OffensiveSkill;

    public bool IsOffensive;

    /// <summary>
    /// The associated animation when this Skill is being used.</summary>
    public AnimationClip AnimationClip;

    /// <summary>
    /// The index of the attack animation in the blend tree.</summary>
    public float AnimationIndex;

    /// <summary>
    /// The time in seconds inside the clip's range that represents when the 
    /// magic skill has been fully cast.</summary>
    public float CastingTime;
}
