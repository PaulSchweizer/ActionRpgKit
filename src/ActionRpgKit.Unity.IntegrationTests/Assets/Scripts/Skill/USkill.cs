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

class UMeleeCombatSkill : USkill
{
    public MeleeCombatSkill Skill;
    public UUsableItem[] ItemSequence;
}

class URangedCombatSkill : USkill
{
    public RangedCombatSkill Skill;
    public UUsableItem[] ItemSequence;
}
