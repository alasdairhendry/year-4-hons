using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer ( typeof ( Inventory.ItemStack ) )]
public class ItemStackPropertyDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty ( position, label, property );

        // Draw label

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel = 0;
        
        int i = property.FindPropertyRelative ( "ID" ).intValue;

        GUILayout.BeginVertical ("Box");
        EditorGUILayout.BeginHorizontal ();
        EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "ID" ), new GUIContent("ID") );
        int x = EditorGUILayout.Popup ( property.FindPropertyRelative ( "ID" ).intValue, ItemDatabase.GetStrings () );
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField ( property.FindPropertyRelative ( "Amount" ) );

        if (i != x)
        {
            property.FindPropertyRelative ( "ID" ).intValue = x;
        }

        GUILayout.EndVertical ();
        EditorGUILayout.Space ();

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty ();
    }
}