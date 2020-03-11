using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DropTable))]
public class DropTableEditor : Editor
{
    DropTable t;

    private void OnEnable ()
    {
        t = (DropTable)target;
    }

    public override void OnInspectorGUI ()
    {
        EditorGUILayout.BeginVertical ();

        EditorExtensions.Horizontal ( () => {
            if (GUILayout.Button ( "Add" ))
            {
                Loot loot = new Loot ();
                if (t.loot.Count > 0)
                {
                    loot.itemID = t.loot[t.loot.Count - 1].itemID + 1;
                    loot.amount = t.loot[t.loot.Count - 1].amount;
                    loot.amountRange = t.loot[t.loot.Count - 1].amountRange;
                    loot.range = t.loot[t.loot.Count - 1].range;
                    loot.weight = t.loot[t.loot.Count - 1].weight;
                }
                t.loot.Add ( loot );
            }

            if (GUILayout.Button ( "Expand All" ))
            {
                for (int i = 0; i < t.loot.Count; i++)
                {
                    t.loot[i].foldout = true;
                }
            }

            if (GUILayout.Button ( "Collapse All" ))
            {
                for (int i = 0; i < t.loot.Count; i++)
                {
                    t.loot[i].foldout = false;
                }
            }
        } );


        for (int i = 0; i < t.loot.Count; i++)
        {
            Loot loot = t.loot[i];
            string name = "";
            float percentageNormalised = (((float)loot.weight / (float)t.GetOverallWeighting ()));
            float percentageChance = percentageNormalised * 100.0f;

            float numerator = percentageChance / percentageChance;
            float denominator = 100.0f / percentageChance;

            string dropChance = " (" + numerator + "/" + denominator + ")";
            string percentageChanceString = " (" + percentageChance.ToString ( "0.00" ) + "%)";
            if(loot.weight == 0)
            {
                dropChance = "(ALWAYS DROPS)";
                percentageChanceString = "";
            }

            string amount = " [" + serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amount" ).intValue.ToString () + "] ";

            if (serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "range" ).boolValue)
            {
                amount = " [" + serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ).vector2IntValue.x + " - " + serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ).vector2IntValue.y + "] ";
            }

            if (ItemDatabase.ItemExists(serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "itemID" ).intValue) == false)
            {
                name = "Null" + dropChance + percentageChanceString;
            }
            else
            {
                name = ItemDatabase.GetItem ( serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "itemID" ).intValue ).Name + amount + dropChance + percentageChanceString;
            }

            t.loot[i].foldout = EditorGUILayout.Foldout ( t.loot[i].foldout, name, true );

            if (t.loot[i].foldout == false) continue;

            EditorGUILayout.BeginVertical ( "Box" );

            EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "itemID" ) );

            if (!serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "range" ).boolValue)
            {
                EditorGUILayout.BeginHorizontal ();
                EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amount" ) );
                GUILayout.FlexibleSpace ();
                EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "range" ), GUIContent.none );
                EditorGUILayout.EndHorizontal ();
            }

            if (serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "range" ).boolValue)
            {
                EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ) );
                EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "range" ) );
            }

            if (serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ).vector2IntValue.x <= 0)
            {
                serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ).vector2IntValue = new Vector2Int ( 1, serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ).vector2IntValue.y );
            }

            if (serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ).vector2IntValue.y <= 0)
            {
                serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ).vector2IntValue = new Vector2Int ( serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amountRange" ).vector2IntValue.x, 1 );
            }

            if (serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amount" ).intValue <= 0)
            {
                serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "amount" ).intValue = 1;
            }

            EditorGUILayout.Space ();

            EditorGUILayout.BeginHorizontal ();
            EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "loot" ).GetArrayElementAtIndex ( i ).FindPropertyRelative ( "weight" ) );
            EditorGUILayout.EndHorizontal ();

            if (GUILayout.Button ( "Remove" ))
            {
                t.loot.RemoveAt ( i );
            }

            serializedObject.ApplyModifiedProperties ();
            EditorGUILayout.EndVertical ();
        }

        EditorGUILayout.EndVertical ();

        if (GUILayout.Button ( "1000 rolls debug" ))
        {
            t.Roll1000Times ();
        }
    }
}
