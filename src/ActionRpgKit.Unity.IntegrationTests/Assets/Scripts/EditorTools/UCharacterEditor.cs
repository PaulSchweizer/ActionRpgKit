#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Character;
using ActionRpgKit.Item;

/// <summary>
/// Editor for UsableItems.</summary>
[CustomEditor(typeof(UPlayerCharacter))]
public class UPlayerCharacterEditor : Editor
{

    /// <summary>
    /// Whether to show or hide the Stats</summary>
    private bool _showStats = true;

    /// <summary>
    /// Whether to show or hide the Skill List</summary>
    private bool _showSkills = true;

    /// <summary>
    /// Whether to show or hide the Item List</summary>
    private bool _showItems = true;

    /// <summary>
    /// Quantity of the Items to add</summary>
    private int _quantity = 1;

    /// <summary>
    /// The selected row holding the desired Item.</summary>
    private int row;

    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields of the ScriptableObject
        //DrawDefaultInspector();
        EditorGUILayout.LabelField("PlayerCharacter", EditorStyles.boldLabel);

        var uPlayerCharacter = (UPlayerCharacter)target;
        var character = uPlayerCharacter.Character;

        #region Stats

        // Show the stats
        _showStats = EditorGUILayout.Foldout(_showStats, "Stats");
        if (_showStats)
        {

            // Basic fields of the Character
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Format("{0} {1}", character.Id, character.Name), EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            // Stats
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel, GUILayout.Width(75));
            EditorGUILayout.LabelField("Value", EditorStyles.boldLabel, GUILayout.Width(75));
            EditorGUILayout.LabelField("Min", EditorStyles.boldLabel, GUILayout.Width(75));
            EditorGUILayout.LabelField("Max", EditorStyles.boldLabel, GUILayout.Width(75));
            EditorGUILayout.EndHorizontal();

            foreach (KeyValuePair<string, BaseAttribute> attr in character.Stats.Dict)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(attr.Key, GUILayout.Width(75));
                attr.Value.Value = EditorGUILayout.FloatField(attr.Value.Value, GUILayout.Width(75));
                attr.Value.MinValue = EditorGUILayout.FloatField(attr.Value.MinValue, GUILayout.Width(75));
                attr.Value.MaxValue = EditorGUILayout.FloatField(attr.Value.MaxValue, GUILayout.Width(75));
                EditorGUILayout.EndHorizontal();
            }

        }

        #endregion

        #region Skills

        // Show all the skills
        _showSkills = EditorGUILayout.Foldout(_showSkills, "Skills");
        if (_showSkills)
        {

        }
        
        #endregion

        #region Inventory

        var db = (UItemDatabase)AssetDatabase.LoadMainAssetAtPath("Assets/Data/ItemDatabase.asset");
        db.InitDatabase();

        // Show Inventory
        _showItems = EditorGUILayout.Foldout(_showItems, 
            string.Format("Inventory {0} Items", character.Inventory.ItemCount));
        if (_showItems)
        {
            var items = new List<UItem>();
            var names = new List<string>();

            // Get all Items from the Directory
            EditorGUILayout.BeginHorizontal("box");

            foreach (var item in ItemDatabase.Items)
            {
                names.Add(item.Name.ToString());
            }

            row = EditorGUILayout.Popup(row, names.ToArray());
            _quantity = EditorGUILayout.IntField(_quantity);

            if (GUILayout.Button("+"))
            {
                character.Inventory.AddItem(row, _quantity);
            }
            EditorGUILayout.EndHorizontal();

            DrawInventory(character.Inventory);
        }

        #endregion

    }

    /// <summary>
    /// Display the Model in the Editor.</summary>
    void DrawInventory(IInventory inventory)
    {
        var items = inventory.Items.GetEnumerator();
        var quantities = inventory.Quantities.GetEnumerator();
        while (items.MoveNext() && quantities.MoveNext())
        {
            EditorGUILayout.BeginHorizontal("box");
            string displayRow = ItemDatabase.GetItemById(items.Current).Name + "\tx " + quantities.Current;
            EditorGUILayout.LabelField(displayRow);

            if (GUILayout.Button("+"))
            {
                inventory.AddItem(items.Current, 1);
                //EditorUtility.SetDirty(inventory);
                EditorGUILayout.EndHorizontal();
            }
            else if (GUILayout.Button("-"))
            {
                inventory.RemoveItem(items.Current, 1);
                //EditorUtility.SetDirty(inventory);
                EditorGUILayout.EndHorizontal();
            }
            else if (GUILayout.Button("X"))
            {
                inventory.RemoveItem(items.Current, quantities.Current);
                //EditorUtility.SetDirty(inventory);
                EditorGUILayout.EndHorizontal();
                break;
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }
         }

        //if (GUILayout.Button("Reset"))
        //{
        //    inventory.Reset();
        //    EditorUtility.SetDirty(inventory);
        //}
    }



}

#endif