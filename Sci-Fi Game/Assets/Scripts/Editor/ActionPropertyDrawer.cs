using UnityEngine;
using UnityEditor;

namespace QuestFlow
{
    [CustomPropertyDrawer ( typeof ( Action ), true )]
    public class ActionPropertyDrawer : PropertyDrawer
    {
        private Editor editor = null;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            GUILayout.Space ( -20 );

            if (!editor)
                Editor.CreateCachedEditor ( property.objectReferenceValue, null, ref editor );

            if (editor)
                editor.OnInspectorGUI ();
        }
    }
}