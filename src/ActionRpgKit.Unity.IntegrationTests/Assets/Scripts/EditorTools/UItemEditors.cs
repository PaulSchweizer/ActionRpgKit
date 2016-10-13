#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using ActionRpgKit.Item;

/// <summary>
/// Editor for UsableItems.</summary>
[CustomEditor(typeof(UUsableItem ))]
public class UUsableItemEditor : Editor
{
    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields on the ScriptableObject
        DrawDefaultInspector();

        var uItem = (UUsableItem)target;
        var item = uItem.Item;

        // Draw the fields on the Item 
        EditorGUILayout.LabelField("UsableItem", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(string.Format("Id: {0}", item.Id));
        EditorGUILayout.LabelField("Name:", item.Name);
        item.Description = EditorGUILayout.TextArea(item.Description, GUILayout.Height(60));
    }
}

/// <summary>
/// Editor for UsableItems.</summary>
[CustomEditor(typeof(UWeaponItem))]
public class UWeaponItemEditor : Editor
{
    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields on the ScriptableObject
        DrawDefaultInspector();

        var uItem = (UWeaponItem)target;
        var item = uItem.Item;

        // Draw the fields on the Item 
        EditorGUILayout.LabelField("WeaponItem", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(string.Format("Id: {0}", item.Id));
        EditorGUILayout.LabelField("Name:", item.Name);
        item.Description = EditorGUILayout.TextArea(item.Description, GUILayout.Height(60));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Damage:");
        item.Damage = EditorGUILayout.FloatField(item.Damage);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Range:");
        item.Range = EditorGUILayout.FloatField(item.Range);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Speed:");
        item.Speed = EditorGUILayout.FloatField(item.Speed);
        EditorGUILayout.EndHorizontal();
    }
}
#endif
