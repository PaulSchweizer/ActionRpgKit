using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ActionRpgKit.Item;
using System.IO;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ActionRpgKit/Create Item Database")]
public class UItemDatabase : ScriptableObject
{
    [SerializeField]
    public UItem[] Items;

    /// <summary>
    /// Get all Items and set them to the ActionRpgKit ItemDatabase.</summary>
    public void InitDatabase()
    {
        List<BaseItem> items = new List<BaseItem>();
        foreach (UItem uitem in Items)
        {
            if (uitem is UUsableItem)
            {
                var uUsableItem = uitem as UUsableItem;
                var iItem = uUsableItem.Item as BaseItem;
                items.Add(iItem);
            }
            else if (uitem is UWeaponItem)
            {
                var uWeaponItem = uitem as UWeaponItem;
                var iItem = uWeaponItem.Item as BaseItem;
                items.Add(iItem);
            }
        }
        ItemDatabase.Items = items.ToArray();
    }
}
