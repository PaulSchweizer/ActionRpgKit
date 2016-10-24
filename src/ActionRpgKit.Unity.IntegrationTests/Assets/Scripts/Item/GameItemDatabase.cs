using UnityEngine;
using System.Collections.Generic;
using ActionRpgKit.Item;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ActionRpgKit/Create Item Database")]
public class GameItemDatabase : ScriptableObject
{
    [SerializeField]
    public ItemData[] Items;

    /// <summary>
    /// Get all Items and set them to the ActionRpgKit GameItemDatabase.</summary>
    public void InitDatabase()
    {
        List<BaseItem> items = new List<BaseItem>();
        foreach (ItemData itemData in Items)
        {
            if (itemData is UsableItemData)
            {
                var usableItemData = itemData as UsableItemData;
                var baseItem = usableItemData.Item as BaseItem;
                items.Add(baseItem);
            }
            else if (itemData is WeaponItemData)
            {
                var weaponItemData = itemData as WeaponItemData;
                var baseItem = weaponItemData.Item as BaseItem;
                items.Add(baseItem);
            }
        }
        ItemDatabase.Items = items.ToArray();
    }
}
