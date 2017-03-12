using ActionRpgKit.Character;
using UnityEngine;

/// <summary>
/// A Loot object that can be picked up by the Player.</summary>
public class Loot : MonoBehaviour
{
    /// <summary>
    /// Items in the Loot.</summary>
    public BaseInventory Inventory = new SimpleInventory();

    [SerializeField]
    public int[] _serializedInventoryItems;

    [SerializeField]
    public int[] _serializedInventoryQuantities;

    public void Awake()
    {
        OnAfterSerialize();
    }

    /// <summary>
    /// Collect the Items on collision.
    /// Destroy the Loot Object.</summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Inventory != null)
            {
                GamePlayer.Instance.Character.Inventory.AddInventory(Inventory);
            }
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Reset the saved Attribute values.</summary>
    public void OnBeforeSerialize()
    {
        _serializedInventoryItems = new int[Inventory.ItemCount];
        _serializedInventoryQuantities = new int[Inventory.ItemCount];
        var items = Inventory.Items.GetEnumerator();
        var quantities = Inventory.Quantities.GetEnumerator();
        int i = 0;
        while (items.MoveNext() && quantities.MoveNext())
        {
            _serializedInventoryItems[i] = items.Current;
            _serializedInventoryQuantities[i] = quantities.Current;
            i++;
        }
    }

    public void OnAfterSerialize()
    {
        Inventory.Reset();
        for (int i = 0; i < _serializedInventoryItems.Length; i++)
        {
            Inventory.AddItem(_serializedInventoryItems[i],
                              _serializedInventoryQuantities[i]);
        }
    }
}