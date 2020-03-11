using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer ( typeof ( Inventory.ItemStack ) )]
public class ItemStackPropertyDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty ( position, label, property );

        // Draw label
        position = EditorGUI.PrefixLabel ( position, GUIUtility.GetControlID ( FocusType.Passive ), label );

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect filterButton = new Rect ( position.x, position.y, position.width - 56, position.height );
        Rect amountRect = new Rect ( position.x + position.width - 48, position.y, 48, position.height );

        EditorGUI.PropertyField ( amountRect, property.FindPropertyRelative ( "Amount" ), GUIContent.none );

        if (EditorGUI.DropdownButton ( filterButton, new GUIContent ( ItemDatabase.GetStrings ()[property.FindPropertyRelative ( "ID" ).intValue] ), FocusType.Keyboard ))
        {
            PopupFilterWindow window = EditorWindow.GetWindow<PopupFilterWindow> ();

            window.Open ( ItemDatabase.GetStrings (), "Item", filterButton, (i) =>
            {
                property.serializedObject.Update ();
                property.FindPropertyRelative ( "ID" ).intValue = i;
                property.serializedObject.ApplyModifiedProperties ();
            } );
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty ();
    }
}