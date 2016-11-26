using ActionRpgKit.Item;
using UnityEngine;

public class UsableItemData : ItemData
{
    [SerializeField]
    public new UsableItem Item;

    public override int Id
    {
        get
        {
            return Item.Id;
        }
    }
}
