#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;
using System;

/// <summary>
/// Editor Menu for Skills.
/// Re-implement accordingly for each Skill type.</summary>
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
    private float _cooldownTime;

    // General Magic
    private float _cost; 

    // Generic Combat
    private float _damage;
    private int _maximumTargets;
    private float _range;

    // Passive Magic
    private float _duration;
    private float _modifierValue;
    private string _modifiedAttributeName;

    /// <summary>
    /// The itemType.</summary>
    int _skillType;

    /// <summary>
    /// Possible types of items.</summary>
    string[] _skillTypes = new string[] { "PassiveMagicSkill", "GenericCombatSkill" };

    [MenuItem("ActionRpgKit/Create New Skill")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = GetWindow(typeof(SkillCreator));
        window.Show();
    }

    void OnGUI()
    {
        _skillType = EditorGUILayout.Popup(_skillType, _skillTypes);
        _name = EditorGUILayout.TextField("Name", _name);
        _description = EditorGUILayout.TextArea(_description, GUILayout.Height(60));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("CooldownTime:");
        _cooldownTime = EditorGUILayout.FloatField(_cooldownTime);
        EditorGUILayout.EndHorizontal();

        // 0 = PassiveMagicSkill
        if (_skillType == 0)
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
        
        // 1 = GenericCombatSkill
        else if (_skillType == 1)
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
            _range = EditorGUILayout.FloatField(_range);
            EditorGUILayout.EndHorizontal();
        }

        // Create the Skill
        if (GUILayout.Button(string.Format("Create {0}", _skillTypes[_skillType]), GUILayout.Height(30)))
        {
            if (_name.Length > 0)
            {
                CreateNewSkill();
            }
        }
    }

    void CreateNewSkill()
    {
        // 0 = Passive Magic
        if (_skillType == 0)
        {
            var skill = CreatePassiveMagicSkill();
            AssetDatabase.CreateAsset(skill, Path.Combine(RelativePath,
                                      string.Format("MagicSkill/{0}_{1}.asset", 
                                                    skill.Skill.Id, 
                                                    _name)));
        }
        // 1 = GenericCombatSkill
        else if (_skillType == 1)
        {
            var skill = CreateGenericCombatSkill();
            AssetDatabase.CreateAsset(skill, Path.Combine(RelativePath,
                                      string.Format("GenericCombatSkill/{0}_{1}.asset",
                                                    skill.Skill.Id, 
                                                    _name)));
        }
        else
        {
            return;
        }
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Create a PassiveMagicSkillData.</summary>
    /// <returns>A ScriptableObject</returns>
    PassiveMagicSkillData CreatePassiveMagicSkill()
    {
        var skill = new PassiveMagicSkill(
                            id: GetId("MagicSkill"),
                            name: _name,
                            description: _description,
                            cooldownTime: _cooldownTime,
                            cost: _cost,
                            itemSequence: new int[] { },
                            duration: _duration,
                            modifierValue: _modifierValue,
                            modifiedAttributeName: _modifiedAttributeName
            );
        var scriptableSkill = ScriptableObject.CreateInstance<PassiveMagicSkillData>();
        scriptableSkill.Skill = skill;
        return scriptableSkill;
    }
    
    /// <summary>
    /// Create a UMeleeCombatSkill.</summary>
    /// <returns>A ScriptableObject</returns>
    GenericCombatSkillData CreateGenericCombatSkill()
    {
        var skill = new GenericCombatSkill(
                            id: GetId("GenericCombatSkill"),
                            name: _name,
                            description: _description,
                            cooldownTime: _cooldownTime, 
                            damage: _damage,
                            itemSequence: new int[] { },
                            maximumTargets: _maximumTargets,
                            range: _range);
        var scriptableSkill = ScriptableObject.CreateInstance<GenericCombatSkillData>();
        scriptableSkill.Skill = skill;
        return scriptableSkill;
    }

    /// <summary>
    /// Set the Id to the nmber of already existing Items in the Data folder.</summary>
    int GetId(string skillType)
    {
        var dir = string.Format("{0}{1}", AbsolutePath, skillType);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var path = new DirectoryInfo(dir);
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
        return biggestId + 1;
    }
}
#endif
