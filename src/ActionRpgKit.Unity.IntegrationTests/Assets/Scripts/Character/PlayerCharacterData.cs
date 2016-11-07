using UnityEngine;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;
using System.Collections.Generic;

public class PlayerCharacterData : BaseCharacterData, ISerializationCallbackReceiver
{
    public new Player Character;

    #region Serialization

    [SerializeField]
    private float[] _serializedStats;

    [SerializeField]
    private int[] _serializedCombatSkills;

    [SerializeField]
    private int[] _serializedMagicSkills;

    [SerializeField]
    private int[] _serializedInventoryItems;

    [SerializeField]
    private int[] _serializedInventoryQuantities;

    [SerializeField]
    private int _equippedWeapon;

    [SerializeField]
    private int _currentAttackSkill;

    /// <summary>
    /// Reset the saved Attribute values.</summary>
    public void OnBeforeSerialize()
    {
        // Stats
        _serializedStats = new float[Character.Stats.Dict.Count];
        int i = 0;
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            _serializedStats[i] = attr.Value.Value;
            i++;
        }

        // Skills
        if (Character.CombatSkills.Count > 0)
        {
            _serializedCombatSkills = new int[Character.CombatSkills.Count];
            for (int j = 0; j < Character.CombatSkills.Count; j++)
            {
                _serializedCombatSkills[j] = Character.CombatSkills[j];
            }
        }
        if (Character.MagicSkills.Count > 0)
        {
            _serializedMagicSkills = new int[Character.MagicSkills.Count];
            for (int k = 0; k < Character.MagicSkills.Count; k++)
            {
                _serializedMagicSkills[k] = Character.MagicSkills[k];
            }
        }

        // Inventory
        _serializedInventoryItems = new int[Character.Inventory.ItemCount];
        _serializedInventoryQuantities = new int[Character.Inventory.ItemCount];
        var items = Character.Inventory.Items.GetEnumerator();
        var quantities = Character.Inventory.Quantities.GetEnumerator();
        i = 0;
        while (items.MoveNext() && quantities.MoveNext())
        {
            _serializedInventoryItems[i] = items.Current;
            _serializedInventoryQuantities[i] = quantities.Current;
            i++;
        }

        // Equipped Weapon
        _equippedWeapon = Character.EquippedWeapon;

        // Current Attack Skill
        _currentAttackSkill = Character.CurrentAttackSkill;
    }

    /// <summary>
    /// Save the Values from the Attributes to an internal, serializable list.</summary>
    public void OnAfterDeserialize()
    {
        // Stats
        int i = 0;
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            attr.Value.Value = _serializedStats[i];
            i++;
        }

        // Skills
        for (int k = 0; k < _serializedCombatSkills.Length; k++)
        {
            Character.LearnCombatSkill(_serializedCombatSkills[k]);
        }
        for (int k = 0; k < _serializedMagicSkills.Length; k++)
        {
            Character.LearnMagicSkill(_serializedMagicSkills[k]);
        }

        // Inventory
        for (i = 0; i < _serializedInventoryItems.Length; i++)
        {
            Character.Inventory.AddItem(_serializedInventoryItems[i],
                                        _serializedInventoryQuantities[i]);
        }

        // Equipped Weapon
        Character.EquippedWeapon = _equippedWeapon;

        // Current Attack Skill
        Character.CurrentAttackSkill = _currentAttackSkill;

        Character.IsDead = false;
    }

    #endregion
}