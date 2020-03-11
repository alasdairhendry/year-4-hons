using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor ( typeof ( PrefabAssetDatabase ) )]
public class PrefabAssetDatabaseEditor : Editor
{
    private PrefabAssetDatabase t;
    private Vector2 scrollPos = new Vector2 ();

    private void OnEnable ()
    {
        t = (PrefabAssetDatabase)target;
    }

    public override void OnInspectorGUI ()
    {
        EditorExtensions.VerticalScroll ( () =>
        {
            for (int i = 0; i < t.Assets.Count; i++)
            {
                EditorExtensions.Horizontal ( () =>
                {
                    EditorGUILayout.LabelField ( t.Assets[i].assetNameString );
                    t.Assets[i].asset = EditorGUILayout.ObjectField ( t.Assets[i].asset, typeof ( GameObject ), false ) as GameObject;
                    EditorUtility.SetDirty ( t );
                } );
            }
        }, ref scrollPos );
    }
}