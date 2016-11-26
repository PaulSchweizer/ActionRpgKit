using UnityEngine;
using ActionRpgKit.Item;

public class ItemData : ScriptableObject
{
    [SerializeField]
    public BaseItem Item;

    public virtual int Id
    {
        get
        {
            return Item.Id;
        }
    }

    public Sprite Sprite;
}
