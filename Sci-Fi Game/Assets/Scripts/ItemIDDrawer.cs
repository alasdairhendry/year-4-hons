using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer ( typeof ( ItemIDAttribute ) )]
public class ItemIDDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty ( position, label, property );

        position = EditorGUI.PrefixLabel ( position, GUIUtility.GetControlID ( FocusType.Passive ), label );

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect intRect = new Rect ( position.x, position.y, 32.0f, position.height );
        Rect filterButton = new Rect ( position.x + 40, position.y, position.width - 40, position.height );

        ItemIDAttribute attr = attribute as ItemIDAttribute;

        EditorGUI.PropertyField ( intRect, property, GUIContent.none );

        string stringName = "";

        if (ItemDatabase.GetStrings ().IsValidIndex ( property.intValue ))
        {
            stringName = ItemDatabase.GetStrings ()[property.intValue];
        }
        else
        {
            stringName = "Empty";
        }

        if (EditorGUI.DropdownButton ( filterButton, new GUIContent ( stringName ), FocusType.Keyboard ))
        {
            PopupFilterWindow window = EditorWindow.GetWindow<PopupFilterWindow> ();

            window.Open ( ItemDatabase.GetStrings (), "Item", filterButton, (i) =>
            {
                property.serializedObject.Update ();
                property.intValue = i;
                property.serializedObject.ApplyModifiedProperties ();
            } );
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty ();
    }

}
