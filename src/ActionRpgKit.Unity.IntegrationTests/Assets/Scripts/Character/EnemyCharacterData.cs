using UnityEngine;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;
using System.Collections.Generic;

public class EnemyCharacterData : BaseCharacterData, ISerializationCallbackReceiver
{
    public new Enemy Character;

    #region Serialization

    [SerializeField]
    private string _serializedName;

    [SerializeField]
    private float[] _serializedStats;

    [SerializeField]
    private float[] _serializedStatsMin;

    [SerializeField]
    private float[] _serializedStatsMax;

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
        _serializedName = Character.Name;

        // Stats
        _serializedStats = new float[Character.Stats.Dict.Count];
        _serializedStatsMin = new float[Character.Stats.Dict.Count];
        _serializedStatsMax = new float[Character.Stats.Dict.Count];
        int i = 0;
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            _serializedStats[i] = attr.Value.Value;
            _serializedStatsMin[i] = attr.Value.MinValue;
            _serializedStatsMax[i] = attr.Value.MaxValue;
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
        Character.Name = _serializedName;

        // Stats
        int i = 0;
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            attr.Value.Value = _serializedStats[i];
            attr.Value.MinValue = _serializedStatsMin[i];
            attr.Value.MaxValue = _serializedStatsMax[i];
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

        // Reset the defeated indicator
        Character.IsDefeated = false;

        // Reset the CombatSkill end times
        Character.CombatSkillEndTimes.Clear();
        for (i = 0; i < Character.CombatSkills.Count; i++)
        {
            Character.CombatSkillEndTimes.Add(-1);
        }
    }

    #endregion
}