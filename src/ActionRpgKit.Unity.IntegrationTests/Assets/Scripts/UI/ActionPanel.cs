using SlotSystem;
using System;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    public Slot SlotA;
    public Slot SlotB;
    public Slot SlotC;
    public Slot SlotD;

    public float MaxInbetweenTime;

    private int[] _triggeredItems = new int[] { };
    private float _triggerTime;

    /// <summary>
    /// An Action Slot has been clicked by the Player.
    /// Has to be connected to the click event of the Slot.</summary>
    /// <param name="slot">The clicked Slot.</param>
    public void SlotTriggered (Slot slot)
    {
        if (slot.Item == null)
        {
            return;
        }

        var item = (UsableItemData)slot.Item.Item;

        // Clear the item list if the time difference is too big
        if (Time.time - _triggerTime > MaxInbetweenTime)
        {
            Array.Clear(_triggeredItems, 0, _triggeredItems.Length);
            Array.Resize(ref _triggeredItems, 1);
        }
        else
        {
            Array.Resize(ref _triggeredItems, _triggeredItems.Length + 1);
        }
        _triggeredItems[_triggeredItems.Length - 1] = item.Item.Id;

        // Remove the item from the Inventory if it is not a permanent Item.
        if (item.Item.DestroyOnUse)
        {
            GamePlayer.Instance.Character.Inventory.RemoveItem(item.Item.Id);
        }

        // Add the item and check if the sequence matches any of the available Skills
        _triggerTime = Time.time;

        for(int i = 0; i < GamePlayer.Instance.Character.MagicSkills.Count; i++)
        {
            var skillId = GamePlayer.Instance.Character.MagicSkills[i];
            var skill = ActionRpgKitController.Instance.SkillDatabase.GetMagicSkillById(skillId).Skill;
            if (skill.Match(_triggeredItems))
            {
                GamePlayer.Instance.Character.TriggerMagicSkill(skillId);
                return;
            }
        }
    }
}