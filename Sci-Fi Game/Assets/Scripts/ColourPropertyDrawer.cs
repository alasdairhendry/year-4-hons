//using UnityEditor;
//using UnityEngine;
//using System.Linq;
//using System.Collections.Generic;

//[CustomPropertyDrawer ( typeof ( Color ) )]
//public class ColourPropertyDrawer : PropertyDrawer
//{
//    int index = 0;

//    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
//    {
//        // Using BeginProperty / EndProperty on the parent property means that
//        // prefab override logic works on the entire property.
//        EditorGUI.BeginProperty ( position, label, property );

//        // Draw label

//        // Don't make child fields be indented
//        var indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = 0;


//        GUILayout.BeginVertical ( "Box" );

//        EditorGUI.BeginChangeCheck ();
//        EditorGUILayout.PropertyField ( property, new GUIContent ( "Colour" ) );
//        if (EditorGUI.EndChangeCheck ())
//        {
//            index = 0;
//        }


//        EditorGUI.BeginChangeCheck ();
//        List<string> enums = System.Enum.GetNames ( typeof ( ColourDescription ) ).ToList ();
//        enums.Insert ( 0, "None" );

//        index = EditorGUILayout.Popup ( index, enums.ToArray () );

//        if (EditorGUI.EndChangeCheck ())
//        {
//            if (index > 0)
//                property.colorValue = ColourHelper.GetEditorColour ( (ColourDescription)index + 1 );
//        }
//        GUILayout.EndVertical ();

//        // Set indent back to what it was
//        EditorGUI.indentLevel = indent;

//        EditorGUI.EndProperty ();
//    }
//}