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
        var uItem = (UUsableItem)target;
        var item = uItem.Item;
        
        // Draw the fields on the Item 
        EditorGUILayout.LabelField(string.Format("Id: {0}", item.Id));
        item.Name = EditorGUILayout.TextField("Name", item.Name);
        item.Description = EditorGUILayout.TextArea(item.Description, GUILayout.Height(60));

        // Draw the default fields on the ScriptableObject
        DrawDefaultInspector();
    }
}
#endif
