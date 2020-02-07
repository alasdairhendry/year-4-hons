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

        EditorGUILayout.LabelField ( "Modified Hit Chance: " + t.GetBaseHitChance.ToString ( "0.000" ) );
        EditorGUILayout.LabelField ( "Modified Base Damage : " + t.GetBaseDamageOutput.ToString ( "0.000" ) );

    }
}
