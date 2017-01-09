using ActionRpgKit.Item;
using UnityEngine;

public class WeaponItemData : ItemData
{
    [SerializeField]
    public new WeaponItem Item;

    [SerializeField]
    public GenericCombatSkillData Skill;

    public override int Id
    {
        get
        {
            return Item.Id;
        }
    }
}