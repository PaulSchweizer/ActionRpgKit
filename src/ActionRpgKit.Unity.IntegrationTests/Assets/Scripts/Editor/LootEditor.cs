#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;
using ActionRpgKit.Character;
using ActionRpgKit.Item;
using ActionRpgKit.Character.Skill;

/// <summary>
/// Editor for Characters.</summary>
[CustomEditor(typeof(Loot), true)]
public class LootEditor : Editor
{

    /// <summary>
    /// Whether to show or hide the Item List.</summary>
    private bool _showInventory = true;

    /// <summary>
    /// Quantity of the Items to add</summary>
    private int _quantity = 1;

    /// <summary>
    /// The selected row holding the desired Item.</summary>
    private int itemsRow;

    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields of the ScriptableObject
        DrawDefaultInspector(); 

        // Initialize the databases
        var db = (GameItemDatabase)AssetDatabase.LoadMainAssetAtPath("Assets/Data/ItemDatabase.asset");
        db.InitDatabase();
        var skillDb = (GameSkillDatabase)AssetDatabase.LoadMainAssetAtPath("Assets/Data/SkillDatabase.asset");
        skillDb.InitDatabase();

        #region Inventory

        var loot = target as Loot;

        // Show Inventory
        _showInventory = EditorGUILayout.Foldout(_showInventory,
                                                 string.Format("Inventory {0} Items", 
                                                               loot._serializedInventoryItems.Length));
        if (_showInventory)
        {
            var itemNames = new List<string>();

            // Get all Items from the Directory
            EditorGUILayout.BeginHorizontal("box");

            foreach (var item in ItemDatabase.Items)
            {
                itemNames.Add(item.Name);
            }

            itemsRow = EditorGUILayout.Popup(itemsRow, itemNames.ToArray(), GUILayout.Width(170));
            _quantity = EditorGUILayout.IntField(_quantity, GUILayout.Width(30));

            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                loot.Inventory.AddItem(itemsRow, _quantity);
                EditorUtility.SetDirty(target);
                loot.OnBeforeSerialize();
            }
            EditorGUILayout.EndHorizontal();

            loot.OnAfterSerialize();

            DrawInventory(loot.Inventory, loot);
        }

        #endregion

    }

    /// <summary>
    /// Display the Model in the Editor.</summary>
    void DrawInventory(BaseInventory inventory, Loot loot)
    {
        for (int i = 0; i < loot._serializedInventoryItems.Length; i++)
        {
            int item = loot._serializedInventoryItems[i];
            int quantity = loot._serializedInventoryQuantities[i];

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField(ItemDatabase.GetItemById(item).Name, GUILayout.Width(133));
            EditorGUILayout.LabelField("x" + quantity.ToString(), GUILayout.Width(30));

            if (GUILayout.Button("+", GUILayout.Width(18)))
            {
                inventory.AddItem(item, 1);
                EditorUtility.SetDirty(target);
                loot.OnBeforeSerialize();
                EditorGUILayout.EndHorizontal();
            }
            else if (GUILayout.Button("-", GUILayout.Width(18)))
            {
                inventory.RemoveItem(item, 1);
                EditorUtility.SetDirty(target);
                loot.OnBeforeSerialize();
                EditorGUILayout.EndHorizontal();
            }
            else if (GUILayout.Button("X", GUILayout.Width(18)))
            {
                inventory.RemoveItem(item, quantity);
                EditorUtility.SetDirty(target);
                loot.OnBeforeSerialize();
                EditorGUILayout.EndHorizontal();
                break;
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }
        }
    } 
}

#endif