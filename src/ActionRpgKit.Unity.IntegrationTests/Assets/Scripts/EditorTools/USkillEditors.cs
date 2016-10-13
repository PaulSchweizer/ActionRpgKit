#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using ActionRpgKit.Character.Skill;

/// <summary>
/// Editor for UsableItems.</summary>
[CustomEditor(typeof(UPassiveMagicSkill))]
public class UPassiveMagicSkillEditor : Editor
{
    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields of the ScriptableObject
        DrawDefaultInspector();

        var uSkill = (UPassiveMagicSkill)target;
        var skill = uSkill.Skill;

        // Draw the fields of the Skill 
        EditorGUILayout.LabelField("PassiveMagicSkill", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(string.Format("Id: {0}", skill.Id));
        EditorGUILayout.LabelField("Name:", skill.Name);
        skill.Description = EditorGUILayout.TextArea(skill.Description, GUILayout.Height(60));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("PreUseTime:");
        skill.PreUseTime = EditorGUILayout.FloatField(skill.PreUseTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("CooldownTime:");
        skill.CooldownTime = EditorGUILayout.FloatField(skill.CooldownTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Cost:");
        skill.Cost = EditorGUILayout.FloatField(skill.Cost);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Duration:");
        skill.Duration = EditorGUILayout.FloatField(skill.Duration);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ModifierValue:");
        skill.ModifierValue = EditorGUILayout.FloatField(skill.ModifierValue);
        EditorGUILayout.EndHorizontal();

        skill.ModifiedAttributeName = EditorGUILayout.TextField("ModifiedAttributeName:", skill.ModifiedAttributeName);

        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("ItemSequence");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();
    }
}

/// <summary>
/// Editor for UsableItems.</summary>
[CustomEditor(typeof(UMeleeCombatSkill))]
public class UMeleeCombatSkillEditor : Editor
{
    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields of the ScriptableObject
        DrawDefaultInspector();

        var uSkill = target as UMeleeCombatSkill;
        var skill = uSkill.Skill;

        // Draw the fields of the Skill 
        EditorGUILayout.LabelField("MeleeCombatSkill", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(string.Format("Id: {0}", skill.Id));
        EditorGUILayout.LabelField("Name:", skill.Name);
        skill.Description = EditorGUILayout.TextArea(skill.Description, GUILayout.Height(60));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("PreUseTime:");
        skill.PreUseTime = EditorGUILayout.FloatField(skill.PreUseTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("CooldownTime:");
        skill.CooldownTime = EditorGUILayout.FloatField(skill.CooldownTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Damage:");
        skill.Damage = EditorGUILayout.FloatField(skill.Damage);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("MaximumTargets:");
        skill.MaximumTargets = EditorGUILayout.IntField(skill.MaximumTargets);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Range:");
        skill.Range = EditorGUILayout.FloatField(skill.Range);
        EditorGUILayout.EndHorizontal();

        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("ItemSequence");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();
    }
}

/// <summary>
/// Editor for UsableItems.</summary>
[CustomEditor(typeof(URangedCombatSkill))]
public class URangedCombatSkillEditor : Editor
{

    /// <summary>
    /// Draw all the UI elements.</summary>
    public override void OnInspectorGUI()
    {
        // Draw the default fields of the ScriptableObject
        //DrawDefaultInspector();

        var uSkill = target as URangedCombatSkill;
        var skill = uSkill.Skill;

        // Draw the fields of the Skill 
        EditorGUILayout.LabelField("RangedCombatSkill", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(string.Format("Id: {0}", skill.Id));
        EditorGUILayout.LabelField("Name:", skill.Name);
        skill.Description = EditorGUILayout.TextArea(skill.Description, GUILayout.Height(60));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("PreUseTime:");
        skill.PreUseTime = EditorGUILayout.FloatField(skill.PreUseTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("CooldownTime:");
        skill.CooldownTime = EditorGUILayout.FloatField(skill.CooldownTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Damage:");
        skill.Damage = EditorGUILayout.FloatField(skill.Damage);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("MaximumTargets:");
        skill.MaximumTargets = EditorGUILayout.IntField(skill.MaximumTargets);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Range:");
        skill.Range = EditorGUILayout.FloatField(skill.Range);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ProjectileSpeed:");
        skill.ProjectileSpeed = EditorGUILayout.FloatField(skill.ProjectileSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ProjectileLifetime:");
        skill.ProjectileLifetime = EditorGUILayout.FloatField(skill.ProjectileLifetime);
        EditorGUILayout.EndHorizontal();

        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("ItemSequence");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();
    }
}
#endif
