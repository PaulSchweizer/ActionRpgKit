using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour, ISlotChanged
{
    public Slot SlotA;
    public Slot SlotB;
    public Slot SlotC;
    public Slot SlotD;
    public Slot[] Slots;
    public Slot WeaponSlot;

    public float MaxInbetweenTime;

    public Color DefaultColor = new Color(0, 0, 0, 1);
    public Color ActivatedColor;

    public bool Enabled;

    public List<int> _triggeredItems = new List<int>();
    private float _triggerTime;

    /// <summary>
    /// An Action Slot has been clicked by the Player.
    /// Has to be connected to the click event of the Slot.</summary>
    /// <param name="slot">The clicked Slot.</param>
    public void SlotTriggered (Slot slot)
    {
        if (!Enabled)
        {
            return;
        }
        if (slot.Item == null)
        {
            return;
        }

        var item = (UsableItemData)slot.Item.Item;
        StartCoroutine(CooldownEffect(Time.time + MaxInbetweenTime, slot.Item.Background));

        // Clear the item list if the time difference is too big
        if (Time.time - _triggerTime > MaxInbetweenTime)
        {
            _triggeredItems = new List<int>();
        }
        _triggeredItems.Add(item.Item.Id);

        // Remove the item from the Inventory if it is not a permanent Item.
        if (item.Item.DestroyOnUse)
        {
            GamePlayer.Instance.Character.Inventory.RemoveItem(item.Item.Id);
        }

        // Add the item and check if the sequence matches any of the available Skills
        _triggerTime = Time.time;

        for (int i = 0; i < GamePlayer.Instance.Character.MagicSkills.Count; i++)
        {
            var skillId = GamePlayer.Instance.Character.MagicSkills[i];

            var skill = ActionRpgKitController.Instance.SkillDatabase.GetMagicSkillById(skillId).Skill;

            if (skill.Match(_triggeredItems.ToArray()))
            {
                GamePlayer.Instance.Character.TriggerMagicSkill(skillId);
                return;
            }
        }
    }

    /// <summary>
    /// Update the Character in case the Item is placed in a special
    /// Slot, like the Weapon Slot.</summary>
    public void SlotChanged(Slot slot, SlottableItem newItem, SlottableItem previousItem)
    {
        if (slot == WeaponSlot)
        {
            if (newItem == null)
            {
                GamePlayer.Instance.Character.EquippedWeapon = -1;
                GamePlayer.Instance.Character.CurrentAttackSkill = -1;
                return;
            }
            if (newItem.Item is WeaponItemData)
            {
                var itemData = (WeaponItemData)newItem.Item;
                var item = itemData.Item;
                GamePlayer.Instance.Character.EquippedWeapon = item.Id;
                GamePlayer.Instance.Character.CurrentAttackSkill = itemData.Skill.Skill.Id;
            }
        }
    }

    public void Attack()
    {
        if (GamePlayer.Instance.Character.CanAttack() 
            && GamePlayer.Instance.Character.TargetedEnemy != null 
            && GamePlayer.Instance.Character.CurrentState != GamePlayer.Instance.Character.MoveState)
        {
            var dist = GamePlayer.Instance.Character.Position.SquaredDistanceTo(GamePlayer.Instance.Character.TargetedEnemy.Position);
            if (dist > GamePlayer.Instance.Character.Stats.AttackRange.Value * GamePlayer.Instance.Character.Stats.AttackRange.Value)
            {
                return;
            }

            GamePlayer.Instance.Character.Attack(GamePlayer.Instance.Character.TargetedEnemy);
            if (WeaponSlot.IsFree)
            {
                StartCoroutine(CooldownEffect(GamePlayer.Instance.Character.TimeUntilNextAttack, WeaponSlot.GetComponent<Image>()));
            }
            else
            {
                StartCoroutine(CooldownEffect(GamePlayer.Instance.Character.TimeUntilNextAttack, WeaponSlot.Item.Background));
            }
        }
    }

    public IEnumerator CooldownEffect(float endTime, Image image)
    {
        float d = endTime - Time.time;

        while (Time.time < endTime)
        {
            if (image != null)
            {
                float value = (endTime - Time.time) / d;
                image.color = Color.Lerp(DefaultColor, ActivatedColor, value);
            }
            yield return null;
        }
        if (image != null)
        {
            image.color = DefaultColor;
        }
    }
}