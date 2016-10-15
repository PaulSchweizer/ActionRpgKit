using UnityEngine;
using UnityEditor;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;

public class USkill : ScriptableObject { }


class UPassiveMagicSkill : USkill
{
    public PassiveMagicSkill Skill;
    public UUsableItem[] ItemSequence;
}

class UGenericCombatSkill : USkill
{
    public GenericCombatSkill Skill;
    public UUsableItem[] ItemSequence;
}
