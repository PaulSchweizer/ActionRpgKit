using System;
using UnityEngine;
using System.Collections;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;
using System.Collections.Generic;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Item;

public class UPlayerCharacter : UBaseCharacter, ISerializationCallbackReceiver
{
    public Player Character;

    #region Serialization

    [SerializeField] [HideInInspector]
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
            Debug.Log(items.Current);
            _serializedInventoryItems[i] = items.Current.Id;
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

        for(i = 0; i < _serializedInventoryItems.Length; i++)
        {
            Character.Inventory.AddItem(ItemDatabase.GetItemById(_serializedInventoryItems[i]),
                                        _serializedInventoryQuantities[i]);
        }
    }

    #endregion
}