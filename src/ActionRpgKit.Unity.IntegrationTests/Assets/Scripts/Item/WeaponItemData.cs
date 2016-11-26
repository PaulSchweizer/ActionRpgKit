using ActionRpgKit.Item;
using UnityEngine;

public class WeaponItemData : ItemData
{
    [SerializeField]
    public new WeaponItem Item;

    public override int Id
    {
        get
        {
            return Item.Id;
        }
    }
}