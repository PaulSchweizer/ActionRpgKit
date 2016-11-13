using UnityEngine;
using ActionRpgKit.Character.Skill;

/// <summary>
/// A Generic Combat Skill.</summary>
public class GenericCombatSkillData : SkillData
{
    /// <summary>
    /// The ARPG Skill object.</summary>
    public GenericCombatSkill Skill;

    /// <summary>
    /// The associated animation when this Skill is being used.</summary>
    public AnimationClip AnimationClip;

    /// <summary>
    /// The index of the attack animation in the blend tree.</summary>
    public float AnimationIndex;

    /// <summary>
    /// The time in seconds inside the clip's range that represents the actual 
    /// hit of this attack.</summary>
    public float HitTime;
}
