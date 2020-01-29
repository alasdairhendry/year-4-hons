using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof( InventoryDebugObject ) )]
public class InventoryDebugObjectEditor : Editor
{
    InventoryDebugObject t;

    private void OnEnable ()
    {
        t = (InventoryDebugObject)target;
    }

    int amount = 0;
    int id = 0;
    private Vector2 scrollPos;

    public override void OnInspectorGUI ()
    {
        base.DrawDefaultInspector ();

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox ( "Enter play mode", MessageType.Info );
            return;
        }

        if (t.inventory == null) return;

        amount = EditorGUILayout.IntField ( "Amount", amount );

        if (amount < 1) amount = 1;

        id = EditorGUILayout.IntField ( "ID", id );
        EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button ( "Add" ))
        {
            t.inventory.AddItem ( id, amount );
        }
        if (GUILayout.Button ( "Remove" ))
        {
            t.inventory.RemoveItem ( id, amount );
        }
        EditorGUILayout.EndHorizontal ();

        scrollPos = EditorGUILayout.BeginScrollView ( scrollPos );

        for (int i = 0; i < t.inventory.stacks.Count; i++)
        {
            EditorGUILayout.BeginHorizontal ();

            t.inventory.stacks = t.inventory.stacks.OrderBy ( x => x.ID ).ToList ();

            ItemBaseData item = ItemDatabase.GetItem ( t.inventory.stacks[i].ID );

            EditorGUILayout.LabelField ( item.ID.ToString (), GUILayout.MaxWidth ( 24 ) );
            EditorGUILayout.LabelField ( item.Name.ToString () );
            EditorGUILayout.LabelField ( t.inventory.stacks[i].Amount.ToString (), GUILayout.MaxWidth ( 24 ) );

            if (GUILayout.Button ( "Ping" ))
            {
                id = item.ID;
            }

            EditorGUILayout.EndHorizontal ();
        }

        EditorGUILayout.EndScrollView ();
    }
}
