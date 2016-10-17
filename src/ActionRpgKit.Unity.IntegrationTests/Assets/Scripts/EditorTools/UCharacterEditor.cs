#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;
using ActionRpgKit.Character.Stats;

/// <summary>
/// Editor for UsableItems.</summary>
[CustomEditor(typeof(UPlayerCharacter))]
public class UPlayerCharacterEditor : Editor
{
    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields of the ScriptableObject
        DrawDefaultInspector();

        var uPlayerCharacter = (UPlayerCharacter)target;
        var character = uPlayerCharacter.Character;

        // Draw the fields of the Skill 
        EditorGUILayout.LabelField(string.Format("Id: {0}", character.Id));
        EditorGUILayout.LabelField("Name:", character.Name);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel, GUILayout.Width(75));
        EditorGUILayout.LabelField("Value", EditorStyles.boldLabel, GUILayout.Width(75));
        EditorGUILayout.LabelField("Min", EditorStyles.boldLabel, GUILayout.Width(75));
        EditorGUILayout.LabelField("Max", EditorStyles.boldLabel, GUILayout.Width(75));
        EditorGUILayout.EndHorizontal();

        foreach (KeyValuePair<string, IAttribute> attr in character.Stats)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(attr.Key, GUILayout.Width(75));
            attr.Value.Value = EditorGUILayout.FloatField(attr.Value.Value, GUILayout.Width(75));
            attr.Value.MinValue = EditorGUILayout.FloatField(attr.Value.MinValue, GUILayout.Width(75));
            attr.Value.MaxValue = EditorGUILayout.FloatField(attr.Value.MaxValue, GUILayout.Width(75));
            EditorGUILayout.EndHorizontal();
        }
    }
}

#endif