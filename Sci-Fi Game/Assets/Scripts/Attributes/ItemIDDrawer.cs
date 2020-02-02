using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer ( typeof ( ItemIDAttribute ) )]
public class ItemIDDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        ItemIDAttribute attr = attribute as ItemIDAttribute;

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUILayout.BeginHorizontal ();
            EditorGUILayout.PropertyField ( property );
            int x = EditorGUILayout.Popup ( property.intValue, ItemDatabase.GetStrings () );
            EditorGUILayout.EndHorizontal ();

            if (property.intValue != x)
            {
                property.intValue = x;
            }
        }
        else
        {

        }
    }

}
