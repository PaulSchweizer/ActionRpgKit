#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Character;
using ActionRpgKit.Item;
using ActionRpgKit.Character.Skill;

/// <summary>
/// Editor for Characters.</summary>
[CustomEditor(typeof(BaseCharacterData), true)]
public class BaseCharacterDataEditor : Editor
{

    public BaseCharacter character;

    /// <summary>
    /// Whether to show or hide the Stats</summary>
    private bool _showStats = true;

    /// <summary>
    /// Whether to show or hide the Skill List</summary>
    private bool _showSkills = true;

    /// <summary>
    /// Whether to show or hide the Item List</summary>
    private bool _showInventory = true;

    /// <summary>
    /// Quantity of the Items to add</summary>
    private int _quantity = 1;

    /// <summary>
    /// The selected row holding the desired Item.</summary>
    private int itemsRow;

    private int magicSkillsRow;
    private int combatSkillsRow;

    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields of the ScriptableObject
        //DrawDefaultInspector();
        
        if(target is PlayerCharacterData)
        {
            var uBaseCharacter = target as PlayerCharacterData;
            character = uBaseCharacter.Character;
            EditorGUILayout.LabelField(string.Format("{0} {1} (PlayerCharacter)", character.Id, character.Name), EditorStyles.boldLabel);
        }
        else if (target is EnemyCharacterData)
        {
            var uBaseCharacter = target as EnemyCharacterData;
            character = uBaseCharacter.Character;
            EditorGUILayout.LabelField(string.Format("{0} {1} (EnemyCharacter)", character.Id, character.Name), EditorStyles.boldLabel);
        }
        else
        {
            return;
        }

        #region Stats

        // Show the Stats
        _showStats = EditorGUILayout.Foldout(_showStats, "Stats");
        if (_showStats)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("Value", EditorStyles.boldLabel, GUILayout.Width(50));
            EditorGUILayout.LabelField("Min", EditorStyles.boldLabel, GUILayout.Width(50));
            EditorGUILayout.LabelField("Max", EditorStyles.boldLabel, GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();

            foreach (KeyValuePair<string, BaseAttribute> attr in character.Stats.Dict)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(attr.Key, GUILayout.Width(80));
                attr.Value.Value = EditorGUILayout.FloatField(attr.Value.Value, GUILayout.Width(50));
                attr.Value.MinValue = EditorGUILayout.FloatField(attr.Value.MinValue, GUILayout.Width(50));
                attr.Value.MaxValue = EditorGUILayout.FloatField(attr.Value.MaxValue, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
            }
        }

        #endregion

        #region Skills

        // Show all the skills
        _showSkills = EditorGUILayout.Foldout(_showSkills, "Skills");
        if (_showSkills)
        {
            var skillDb = (GameSkillDatabase)AssetDatabase.LoadMainAssetAtPath("Assets/Data/SkillDatabase.asset");
            skillDb.InitDatabase();

            var magicSkillsNames = new List<string>();

            // Get all MagicSkills from the Directory
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("MagicSkills:", GUILayout.Width(80));
            foreach (var skill in SkillDatabase.MagicSkills)
            {
                magicSkillsNames.Add(skill.Name);
            }
            magicSkillsRow = EditorGUILayout.Popup(magicSkillsRow, 
                                                   magicSkillsNames.ToArray(),
                                                   GUILayout.Width(120));
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                character.LearnMagicSkill(magicSkillsRow);
            }
            EditorGUILayout.EndHorizontal();

            var combatSkillsNames = new List<string>();

            // Get all CombatSkills from the Directory
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("CombatSkills:", GUILayout.Width(80));
            foreach (var skill in SkillDatabase.CombatSkills)
            {
                combatSkillsNames.Add(skill.Name);
            }
            combatSkillsRow = EditorGUILayout.Popup(combatSkillsRow, 
                                                    combatSkillsNames.ToArray(),
                                                    GUILayout.Width(120));
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                character.LearnCombatSkill(combatSkillsRow);
            }
            EditorGUILayout.EndHorizontal();

            DrawSkills(character);
        }
        
        #endregion

        #region Inventory

        // Show Inventory
        _showInventory = EditorGUILayout.Foldout(_showInventory, 
            string.Format("Inventory {0} Items", character.Inventory.ItemCount));
        if (_showInventory)
        {
            var db = (GameItemDatabase)AssetDatabase.LoadMainAssetAtPath("Assets/Data/ItemDatabase.asset");
            db.InitDatabase();

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
                character.Inventory.AddItem(itemsRow, _quantity);
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
            EditorGUILayout.LabelField(ItemDatabase.GetItemById(items.Current).Name, GUILayout.Width(133));
            EditorGUILayout.LabelField("x" + quantities.Current.ToString(), GUILayout.Width(30));

            if (GUILayout.Button("+", GUILayout.Width(18)))
            {
                inventory.AddItem(items.Current, 1);
                EditorGUILayout.EndHorizontal();
            }
            else if (GUILayout.Button("-", GUILayout.Width(18)))
            {
                inventory.RemoveItem(items.Current, 1);
                EditorGUILayout.EndHorizontal();
            }
            else if (GUILayout.Button("X", GUILayout.Width(18)))
            {
                inventory.RemoveItem(items.Current, quantities.Current);
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

    void DrawSkills(BaseCharacter character)
    {
        EditorGUILayout.LabelField("Magic Skills", GUILayout.Width(200));
        foreach (var skill in character.MagicSkills)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField(SkillDatabase.GetMagicSkillById(skill).Name, GUILayout.Width(200));
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                character.MagicSkills.Remove(skill);
                EditorGUILayout.EndHorizontal();
                break;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.LabelField("Combat Skills", GUILayout.Width(200));
        foreach (var skill in character.CombatSkills)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField(SkillDatabase.GetCombatSkillById(skill).Name, GUILayout.Width(200));
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                character.CombatSkills.Remove(skill);
                EditorGUILayout.EndHorizontal();
                break;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}

#endif