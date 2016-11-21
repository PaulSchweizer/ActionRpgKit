using ActionRpgKit.Character;
using UnityEngine;

/// <summary>
/// A Loot object that can be picked up by the Player.</summary>
public class Loot : MonoBehaviour
{
    /// <summary>
    /// Items in the Loot.</summary>
    public BaseInventory Inventory;

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
            Destroy(gameObject);
        }
    }
}