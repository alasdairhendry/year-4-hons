using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor ( typeof ( AudioClipAssetDatabase ) )]
public class AudioClipAssetDatabaseEditor : Editor
{
    private AudioClipAssetDatabase t;
    private Vector2 scrollPos = new Vector2 ();

    private void OnEnable ()
    {
        t = (AudioClipAssetDatabase)target;
    }

    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();
        return;
        EditorExtensions.Horizontal ( () =>
        {

            if (GUILayout.Button ( "Expand All" ))
            {
                for (int i = 0; i < t.Assets.Count; i++)
                {
                    t.Assets[i].foldout = true;
                }
            }

            if (GUILayout.Button ( "Collapse All" ))
            {
                for (int i = 0; i < t.Assets.Count; i++)
                {
                    t.Assets[i].foldout = false;
                }
            }

        } );

        EditorExtensions.VerticalScroll ( () =>
        {
            for (int i = 0; i < t.Assets.Count; i++)
            {
                t.Assets[i].foldout = EditorGUILayout.Foldout ( t.Assets[i].foldout, t.Assets[i].assetNameString, true );

                if (t.Assets[i].foldout)
                {
                    for (int x = 0; x < t.Assets[i].assets.Count; x++)
                    {
                        EditorGUI.BeginChangeCheck ();

                        EditorExtensions.Horizontal ( () =>
                        {
                            t.Assets[i].assets[x] = EditorGUILayout.ObjectField ( t.Assets[i].assets[x], typeof ( AudioClip ), false ) as AudioClip;

                            if (GUILayout.Button ( "x", GUILayout.MaxWidth ( 32 ) ))
                            {
                                t.Assets[i].assets.RemoveAt ( x );
                            }
                        } );

                        EditorGUILayout.Space ();

                        if (GUILayout.Button ( "Add" ))
                        {
                            t.Assets[i].assets.Add ( null );
                        }

                        if (EditorGUI.EndChangeCheck ())
                            EditorUtility.SetDirty ( t );
                    }

                }
            }
        }, ref scrollPos );
    }
}