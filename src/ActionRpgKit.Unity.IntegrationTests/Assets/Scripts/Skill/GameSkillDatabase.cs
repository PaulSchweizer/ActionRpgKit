using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ActionRpgKit.Character.Skill;
using System.IO;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "ActionRpgKit/Create Skill Database")]
public class GameSkillDatabase : ScriptableObject
{
    public PassiveMagicSkillData[] PassiveMagicSkillsData;

    public GenericCombatSkillData[] GenericCombatSkillsData;

    /// <summary>
    /// Get all Items and set them to the ActionRpgKit ItemDatabase.</summary>
    public void InitDatabase()
    {
        List<PassiveMagicSkill> magicSkills = new List<PassiveMagicSkill>();

        List<GenericCombatSkill> combatSkills = new List<GenericCombatSkill>();

        foreach (var skillData in PassiveMagicSkillsData)
        {
            magicSkills.Add(skillData.Skill);
        }

        foreach (var skillData in GenericCombatSkillsData)
        {
            combatSkills.Add(skillData.Skill);
        }

        SkillDatabase.CombatSkills = combatSkills.ToArray();
        SkillDatabase.MagicSkills = magicSkills.ToArray();
    }
}
