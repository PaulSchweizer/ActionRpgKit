using UnityEngine;
using UnityEditor;
using ActionRpgKit.Item;

public class UItem : ScriptableObject { }

public class UUsableItem : UItem
{
    public UsableItem Item;
}

public class UWeaponItem : UItem
{
    public WeaponItem Item;
}