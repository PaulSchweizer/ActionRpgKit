using UnityEngine;
using ActionRpgKit.Item;

public class ItemData : ScriptableObject
{
    [SerializeField]
    public BaseItem Item;

    public Sprite Sprite;
}
