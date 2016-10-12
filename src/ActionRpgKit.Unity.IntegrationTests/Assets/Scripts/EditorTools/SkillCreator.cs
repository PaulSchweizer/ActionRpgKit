#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;
using System;

/// <summary>
/// Editor Menu for Characters.
/// Re-implement accordingly for each Item type.</summary>
public class SkillCreator : EditorWindow
{
    /// <summary>
    /// The relative path to the ScriptableObjects Items.</summary>
    private static string RelativePath = "Assets/Data/Skills/";

    /// <summary>
    /// The absolute path to the ScriptableObjects Items.</summary>
    private static string AbsolutePath = Application.dataPath + "/Data/Skills/";

    // General Skill
    private string _name;
    private string _description;
    private float _preUseTime;
    private float _cooldownTime; 
    private IItem[] _itemSequence;

    // General Magic
    private float _cost; 

    // General Combat and MeleeCombat
    private float _damage;
    private int _maximumTargets;
    private float _range;

    // PassiveMagic
    private float _duration;
    private float _modifierValue;
    private string _modifiedAttributeName;

    // RangedCombat
    private float _projectileSpeed;
    private float _projectileLifetime;

    /// <summary>
    /// The itemType.</summary>
    int _skillType;

    /// <summary>
    /// Possible types of items.</summary>
    string[] _skillTypes = new string[] { "PassiveMagicSkill", "MeleeCombatSkill", "RangedCombatSkill" };

    [MenuItem("ActionRpgKit/Create New Skill")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = GetWindow(typeof(SkillCreator));
        window.Show();
    }

    void OnGUI()
    {
        _itemType = EditorGUILayout.Popup(_itemType, _itemTypes);
        _name = EditorGUILayout.TextField("Name", _name);
        _description = EditorGUILayout.TextArea(_description, GUILayout.Height(60));
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("PreUseTime:");
        _preUseTime = EditorGUILayout.FloatField(_preUseTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("CooldownTime:");
        _cooldownTime = EditorGUILayout.FloatField(_cooldownTime);
        EditorGUILayout.EndHorizontal();

        // 0 = PassiveMagicSkill
        if (_itemType == 0)
        { 
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Cost:");
            _cost = EditorGUILayout.FloatField(_cost);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Duration:");
            _duration = EditorGUILayout.FloatField(_duration);
            EditorGUILayout.EndHorizontal();
      
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ModifierValue:");
            _modifierValue = EditorGUILayout.FloatField(_modifierValue);
            EditorGUILayout.EndHorizontal();
            
            _modifiedAttributeName = EditorGUILayout.TextField("ModifiedAttributeName", _modifiedAttributeName);
        }
        
        // 1 = MeleeCombatSkill
        else if (_itemType == 1)
        { 
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Damage:");
            _damage = EditorGUILayout.FloatField(_damage);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("MaximumTargets:");
            _maximumTargets = EditorGUILayout.IntField(_maximumTargets);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Range:");
            _range = EditorGUILayout.IntField(_range);
            EditorGUILayout.EndHorizontal();
        }
        
        // 2 = RangedCombatSkill
        else if (_itemType == 2)
        { 
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ProjectileSpeed:");
            _projectileSpeed = EditorGUILayout.FloatField(_projectileSpeed);
            EditorGUILayout.EndHorizontal();
      
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ProjectileLifetime:");
            _projectileLifetime = EditorGUILayout.FloatField(_projectileLifetime);
            EditorGUILayout.EndHorizontal();
        }

        // Create the Skill
        if (GUILayout.Button(string.Format("Create {0}", _itemTypes[_itemType]), GUILayout.Height(30)))
        {
            if (_name != "")
            {
                CreateNewSkill();
            }
        }
    }

    void CreateNewItem()
    {
        // 0 = Passive Magic
        if (_itemType == 0)
        {
            var skill = CreatePassiveMagicSkill();
            AssetDatabase.CreateAsset(skill, Path.Combine(RelativePath,
                                      string.Format("{0}_{1}.asset", skill.Skill.Id, _name)));
        }
        // 1 = MeleeCombatSkill
        else if (_itemType == 1)
        {
            var skill = CreatMeleeCombatSkill();
            AssetDatabase.CreateAsset(skill, Path.Combine(RelativePath,
                                      string.Format("{0}_{1}.asset", skill.Skill.Id, _name)));
        }
        // 2 = RangedCombatSkill
        else if (_itemType == 2)
        {
            var skill = CreatRangedCombatSkill();
            AssetDatabase.CreateAsset(skill, Path.Combine(RelativePath,
                                      string.Format("{0}_{1}.asset", skill.Skill.Id, _name)));
        }
        else
        {
            return;
        }
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Create a UPassiveMagicSkill.</summary>
    /// <returns>A ScriptableItem</returns>
    UUsableItem CreatePassiveMagicSkill()
    {
        var skill = new PassiveMagicSkill();
        skill.Name = _name;
        skill.Description = _description;
        skill.Description = _description;
        skill.PreUseTime = _preUseTime;
        skill.ColldownTime = _cooldownTime; 
        skill.Cost = _cost;
        skill.Duration = _duration;
        skill.ModifierValue = _modifierValue;
        skill.ModifiedAttributeName = _modifiedAttributeName;
        SetId(skill);
        var scriptableSkill = ScriptableObject.CreateInstance<UPassiveMagicSkill>();
        scriptableSkill.Skill = skill;
        return scriptableSkill;
    }
    
    /// <summary>
    /// Create a UPassiveMagicSkill.</summary>
    /// <returns>A ScriptableItem</returns>
    UUsableItem CreateMeleeCombatSkill()
    {
        var skill = new MeleeCombatSkill();
        skill.Name = _name;
        skill.Description = _description;
        skill.Description = _description;
        skill.PreUseTime = _preUseTime;
        skill.ColldownTime = _cooldownTime; 

    private float _damage;
    private int _maximumTargets;
    private float _range;        SetId(skill);
        var scriptableSkill = ScriptableObject.CreateInstance<UMeleeCombatSkill>();
        scriptableSkill.Skill = skill;
        return scriptableSkill;
    }
    
    /// <summary>
    /// Set the Id to the nmber of already existing Items in the Data folder.</summary>
    /// <param name="item">The IItem</param>
    void SetId(IItem item)
    {
        if (!Directory.Exists(AbsolutePath))
        {
            Directory.CreateDirectory(AbsolutePath);
        }
        var path = new DirectoryInfo(AbsolutePath);
        var files = path.GetFiles("*.asset", SearchOption.AllDirectories);
        int biggestId = -1;
        for (int i = 0; i < files.Length; i++)
        {
            int fileId = Int32.Parse(files[i].Name.Split('_')[0]);
            if(fileId > biggestId)
            {
                biggestId = fileId;
            }
        }
        item.Id = biggestId + 1;
    }
}
#endif
