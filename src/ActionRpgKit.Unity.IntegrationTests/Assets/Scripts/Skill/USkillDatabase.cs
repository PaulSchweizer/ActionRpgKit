using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ActionRpgKit.Character.Skill;
using System.IO;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "ActionRpgKit/Create Skill Database")]
public class USkillDatabase : ScriptableObject
{
    [SerializeField]
    private UPassiveMagicSkill[] UMagicSkills;

    [SerializeField]
    private UGenericCombatSkill[] UCombatSkills;

    /// <summary>
    /// Get all Items and set them to the ActionRpgKit ItemDatabase.</summary>
    public void InitDatabase()
    {
        List<PassiveMagicSkill> magicSkills = new List<PassiveMagicSkill>();

        List<GenericCombatSkill> combatSkills = new List<GenericCombatSkill>();

        foreach (var uSkill in UMagicSkills)
        {
            magicSkills.Add(uSkill.PassiveMagicSkill);
        }

        foreach (var uSkill in UCombatSkills)
        {
            combatSkills.Add(uSkill.GenericCombatSkill);
        }

        SkillDatabase.CombatSkills = combatSkills.ToArray();
        SkillDatabase.MagicSkills = magicSkills.ToArray();
    }
}
