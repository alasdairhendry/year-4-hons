using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor ( typeof ( NPCData ) )]
public class NPCDataEditor : Editor
{
    NPCData t;

    private void OnEnable ()
    {
        t = (NPCData)target;
    }

    public override void OnInspectorGUI ()
    {
        base.DrawDefaultInspector ();

        EditorGUILayout.LabelField ( "Modified Max Health: " + NPCCombatStats.GetMaxHealth(t) );
        EditorGUILayout.Space ();
        EditorGUILayout.LabelField ( "Modified Melee Hit Chance: " + NPCCombatStats.GetMeleeHitChance ( t ) );
        EditorGUILayout.LabelField ( "Modified Melee Damage : " + NPCCombatStats.GetMeleeDamageOutput ( t ) );
        EditorGUILayout.Space ();
        EditorGUILayout.LabelField ( "Modified Gun Hit Chance: " + NPCCombatStats.GetGunHitChance ( t ) );
        EditorGUILayout.LabelField ( "Modified Gun Damage : " + NPCCombatStats.GetGunDamageOutput ( t ) );

    }
}
