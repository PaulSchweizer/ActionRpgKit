#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using ActionRpgKit.Character;
using System;

/// <summary>
/// Editor Menu for Character.
/// Re-implement accordingly for each Character type.</summary>
public class CaracterCreator : EditorWindow
{
    /// <summary>
    /// The relative path to the ScriptableObjects Characters.</summary>
    private static string RelativePath = "Assets/Data/Characters/";

    /// <summary>
    /// The absolute path to the ScriptableObjects Characters.</summary>
    private static string AbsolutePath = Application.dataPath + "/Data/Characters/";

    string _name;

    /// <summary>
    /// The itemType.</summary>
    int _characterType;

    /// <summary>
    /// Possible types of items.</summary>
    string[] _characterTypes = new string[] { "Player", "Enemy" };

    [MenuItem("ActionRpgKit/Create New Character")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = GetWindow(typeof(CaracterCreator));
        window.Show();
    }

    void OnGUI()
    {
        _characterType = EditorGUILayout.Popup(_characterType, _characterTypes);
        _name = EditorGUILayout.TextField("Name:", _name);

        // Create the Character
        if (GUILayout.Button(string.Format("Create {0}", _characterTypes[_characterType]), GUILayout.Height(30)))
        {
            if (_name.Length > 0)
            {
                CreateNewCharacter();
            }
        }
    }

    void CreateNewCharacter()
    {
        // Player Character
        if (_characterType == 0)
        {
            var character = CreatePlayerCharacter();
            AssetDatabase.CreateAsset(character, Path.Combine(RelativePath,
                                        string.Format("{0}_{1}.asset", 
                                        character.Character.Id, _name)));
        }

        // Enemy Character
        else if (_characterType == 1)
        {
            var character = CreateEnemyCharacter();
            AssetDatabase.CreateAsset(character, Path.Combine(RelativePath,
                                      string.Format("{0}_{1}.asset",
                                      character.Character.Id, _name)));
        }

        AssetDatabase.SaveAssets();
    }

    PlayerCharacterData CreatePlayerCharacter()
    {
        var character = new Player();
        character.Name = _name;
        character.Id = GetId();
        var scriptableCharacter = ScriptableObject.CreateInstance<PlayerCharacterData>();
        scriptableCharacter.Character = character;
        return scriptableCharacter;
    }

    EnemyCharacterData CreateEnemyCharacter()
    {
        var character = new Enemy();
        character.Name = _name;
        character.Id = GetId();
        var scriptableCharacter = ScriptableObject.CreateInstance<EnemyCharacterData>();
        scriptableCharacter.Character = character;
        return scriptableCharacter;
    }

    /// <summary>
    /// Set the Id to the nmber of already existing Items in the Data folder.</summary>
    int GetId()
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
            if (fileId > biggestId)
            {
                biggestId = fileId;
            }
        }
        return biggestId + 1;
    }

}

#endif