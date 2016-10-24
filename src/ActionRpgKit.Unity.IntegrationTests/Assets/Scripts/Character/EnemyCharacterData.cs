using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;
using System.Collections.Generic;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Item;

public class EnemyCharacterData : BaseCharacterData, ISerializationCallbackReceiver
{
    public new Enemy Character;

    #region Serialization

    [SerializeField]
    private float[] _serializedStats;

    [SerializeField]
    private int[] _serializedInventoryItems;

    [SerializeField]
    private int[] _serializedInventoryQuantities;

    /// <summary>
    /// Reset the saved Attribute values.</summary>
    public void OnBeforeSerialize()
    {
        _serializedStats = new float[Character.Stats.Dict.Count];
        int i = 0;
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            _serializedStats[i] = attr.Value.Value;
            i++;
        }
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
    }

    /// <summary>
    /// Save the Values from the Attributes to an internal, serializable list.</summary>
    public void OnAfterDeserialize()
    {
        int i = 0;
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            attr.Value.Value = _serializedStats[i];
            i++;
        }
        Character.Inventory.Items = _serializedInventoryItems;
        Character.Inventory.Quantities = _serializedInventoryQuantities;
    }

    #endregion
}