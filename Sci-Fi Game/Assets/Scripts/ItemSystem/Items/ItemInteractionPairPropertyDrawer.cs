using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer ( typeof ( InventoryItemInteraction.ItemInteractionPair ) )]
public class ItemInteractionPairPropertyDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        property.FindPropertyRelative ( "foldout" ).boolValue = EditorGUILayout.Foldout ( property.FindPropertyRelative ( "foldout" ).boolValue,
            new GUIContent ( string.Format ( "[{0}] > [{1}] > [{2}]",
            ItemDatabase.GetItem ( property.FindPropertyRelative ( "primaryItemID" ).intValue ).Name,
           property.FindPropertyRelative ( "usesWorldInteraction" ).boolValue ? property.FindPropertyRelative ( "interactableType" ).enumNames[property.FindPropertyRelative ( "interactableType" ).enumValueIndex] : ItemDatabase.GetItem ( property.FindPropertyRelative ( "secondaryItemID" ).intValue ).Name,
            property.FindPropertyRelative ( "resultsInAction" ).boolValue ? "Action" : ((property.FindPropertyRelative ( "resultingItemIDs" ).arraySize > 0) ? ItemDatabase.GetItem ( property.FindPropertyRelative ( "resultingItemIDs" ).GetArrayElementAtIndex ( 0 ).intValue ).Name : "No Item") ) ), true );

        if (!property.FindPropertyRelative ( "foldout" ).boolValue) return;

        EditorGUI.BeginProperty ( position, label, property );

        // Draw label

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        GUILayout.BeginVertical ( "Box" );
        EditorGUILayout.BeginHorizontal ();

        EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "primaryItemID" ), new GUIContent ( "Primary Item" ) );
        property.FindPropertyRelative ( "primaryItemID" ).intValue = EditorGUILayout.Popup ( property.FindPropertyRelative ( "primaryItemID" ).intValue, ItemDatabase.GetStrings () );

        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.Space ();
        EditorGUILayout.LabelField ( "Interaction", EditorStyles.boldLabel );

        EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "usesWorldInteraction" ), new GUIContent ( "Uses World Item" ) );

        if (property.FindPropertyRelative ( "usesWorldInteraction" ).boolValue)
        {
            property.FindPropertyRelative ( "interactableType" ).enumValueIndex = EditorGUILayout.Popup ( new GUIContent ( "Interactable Type" ), property.FindPropertyRelative ( "interactableType" ).enumValueIndex, property.FindPropertyRelative ( "interactableType" ).enumNames );
            property.FindPropertyRelative ( "removesSecondary" ).boolValue = false;
            property.FindPropertyRelative ( "multiDirectional" ).boolValue = false;
        }
        else
        {
            EditorGUILayout.BeginHorizontal ();

            EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "secondaryItemID" ), new GUIContent ( "Secondary Item" ) );
            property.FindPropertyRelative ( "secondaryItemID" ).intValue = EditorGUILayout.Popup ( property.FindPropertyRelative ( "secondaryItemID" ).intValue, ItemDatabase.GetStrings () );

            EditorGUILayout.EndHorizontal ();
        }

        EditorGUILayout.Space ();
        EditorGUILayout.LabelField ( "Result", EditorStyles.boldLabel );

        EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "resultsInAction" ), new GUIContent ( "Result is Action" ) );

        if (property.FindPropertyRelative ( "resultsInAction" ).boolValue)
        {
            EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "resultingUnityEvent" ), new GUIContent ( "Result is Action" ) );
        }
        else
        {

            int x = property.FindPropertyRelative ( "resultingItemIDs" ).arraySize;

            if (x <= 0)
                property.FindPropertyRelative ( "resultingItemIDs" ).arraySize++;

            for (int i = 0; i < x; i++)
            {
                if (i >= property.FindPropertyRelative ( "resultingItemIDs" ).arraySize) continue;
                EditorGUILayout.BeginHorizontal ();

                EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "resultingItemIDs" ).GetArrayElementAtIndex ( i ), new GUIContent ( "Resulting Item" ) );
                property.FindPropertyRelative ( "resultingItemIDs" ).GetArrayElementAtIndex ( i ).intValue = EditorGUILayout.Popup ( property.FindPropertyRelative ( "resultingItemIDs" ).GetArrayElementAtIndex ( i ).intValue, ItemDatabase.GetStrings () );

                if (property.FindPropertyRelative ( "resultingItemIDs" ).arraySize > 1)
                {
                    if (GUILayout.Button ( "X", GUILayout.MaxWidth ( 32 ) ))
                    {
                        property.FindPropertyRelative ( "resultingItemIDs" ).DeleteArrayElementAtIndex ( i );
                    }
                }
                EditorGUILayout.EndHorizontal ();
            }

            if(GUILayout.Button("Add Resulting Item" ))
            {
                property.FindPropertyRelative ( "resultingItemIDs" ).arraySize++;
            }

            //EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "resultingItemIDs" ), new GUIContent ( "Resulting Item" ) );
            //property.FindPropertyRelative ( "resultingItemIDs" ).intValue = EditorGUILayout.Popup ( property.FindPropertyRelative ( "resultingItemIDs" ).intValue, ItemDatabase.GetStrings () );

        }

        EditorGUILayout.Space ();
        EditorGUILayout.LabelField ( "Options", EditorStyles.boldLabel );

        EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "removesPrimary" ), new GUIContent ( "Remove Primary Item" ) );
        if (!property.FindPropertyRelative ( "usesWorldInteraction" ).boolValue)
        {
            EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "removesSecondary" ), new GUIContent ( "Remove Secondary Item" ) );
            EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "multiDirectional" ), new GUIContent ( "Multi Directional" ) );
        }

        GUILayout.EndVertical ();
        EditorGUILayout.Space ();

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty ();
    }
}
