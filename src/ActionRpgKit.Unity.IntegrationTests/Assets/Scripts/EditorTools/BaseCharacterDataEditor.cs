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
[CustomEditor(typeof(BaseCharacterData), true)]
public class BaseCharacterDataEditor : Editor
{

    public BaseCharacter character;

    /// <summary>
    /// Whether to show or hide the Stats.</summary>
    private bool _showStats = true;

    /// <summary>
    /// Whether to show or hide the Skill List.</summary>
    private bool _showSkills = true;

    /// <summary>
    /// Whether to show or hide the Item List.</summary>
    private bool _showInventory = true;

    /// <summary>
    /// Whether to show or hide the Combat related section.</summary>
    private bool _showCombatSection = true;

    /// <summary>
    /// Quantity of the Items to add</summary>
    private int _quantity = 1;

    /// <summary>
    /// The selected row holding the desired Item.</summary>
    private int itemsRow;

    private int magicSkillsRow;
    private int combatSkillsRow;
    private int equippedWeaponRow;
    private int currentAttackSkillRow;

    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields of the ScriptableObject
        //DrawDefaultInspector(); 

        // Initialize the databases
        var db = (GameItemDatabase)AssetDatabase.LoadMainAssetAtPath("Assets/Data/ItemDatabase.asset");
        db.InitDatabase();
        var skillDb = (GameSkillDatabase)AssetDatabase.LoadMainAssetAtPath("Assets/Data/SkillDatabase.asset");
        skillDb.InitDatabase();

        if (target is PlayerCharacterData)
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

        #region Combat
        // Show all the combat related attributes
        _showCombatSection = EditorGUILayout.Foldout(_showCombatSection, "Combat");
        if (_showCombatSection)
        {
            // Current Weapon
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("Weapon:", GUILayout.Width(80));
            var weaponNames = new List<string>() { "None" };
            var weaponIds = new List<int>() { -1 };
            var items = character.Inventory.Items.GetEnumerator();

            int i = 0;
            while (items.MoveNext())
            {
                var item = ItemDatabase.GetItemById(items.Current);
                if (item is WeaponItem)
                {
                    weaponNames.Add(item.Name);
                    weaponIds.Add(item.Id);
                    i++;
                }

            }
            equippedWeaponRow = EditorGUILayout.Popup(weaponIds.IndexOf(character.EquippedWeapon),
                                                      weaponNames.ToArray(), GUILayout.Width(150));
            character.EquippedWeapon = weaponIds[equippedWeaponRow];
            EditorGUILayout.EndHorizontal();

            // Attack Skill
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("Attack Skill:", GUILayout.Width(80));
            var learnedCombatSkillsNames = new List<string>() { "None" };
            var learnedCombatSkillsIds = new List<int>() { -1 };
            for (i = 0; i < character.CombatSkills.Count; i++)
            {
                learnedCombatSkillsNames.Add(SkillDatabase.GetCombatSkillById(character.CombatSkills[i]).Name);
                learnedCombatSkillsIds.Add(character.CombatSkills[i]);

            }
            currentAttackSkillRow = EditorGUILayout.Popup(learnedCombatSkillsIds.IndexOf(character.CurrentAttackSkill),
                                                          learnedCombatSkillsNames.ToArray(),
                                                          GUILayout.Width(150));
            character.CurrentAttackSkill = learnedCombatSkillsIds[currentAttackSkillRow];
            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Debug

        #endregion

        EditorUtility.SetDirty(target);
    }

    /// <summary>
    /// Display the Model in the Editor.</summary>
    void DrawInventory(BaseInventory inventory)
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