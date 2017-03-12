using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ActionRpgKit.Character.Skill;
using System.IO;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "ActionRpgKit/Create Skill Database")]
public class GameSkillDatabase : ScriptableObject
{
    public GenericCombatSkillData DefaultCombatSkill;

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
            if (skillData.IsOffensive)
            {
                magicSkills.Add(skillData.OffensiveSkill);
            }
            else
            {
                magicSkills.Add(skillData.Skill);
            }
        }

        foreach (var skillData in GenericCombatSkillsData)
        {
            combatSkills.Add(skillData.Skill);
        }

        SkillDatabase.CombatSkills = combatSkills.ToArray();
        SkillDatabase.MagicSkills = magicSkills.ToArray();
    }

    public PassiveMagicSkillData GetMagicSkillById(int skillId)
    {
        foreach (var skillData in PassiveMagicSkillsData)
        {
            if (skillData.Skill.Id == skillId)
            {
                return skillData;
            }
        }
        return null;
    }

    public GenericCombatSkillData GetCombatSkillById (int skillId)
    {
        if (skillId == -1)
        {
            return DefaultCombatSkill;
        }

        foreach (var skillData in GenericCombatSkillsData)
        {
            if (skillData.Skill.Id == skillId)
            {
                return skillData;
            }
        }
        return null;
    }
}
