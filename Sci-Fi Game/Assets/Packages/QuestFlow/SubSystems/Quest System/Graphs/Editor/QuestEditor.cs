using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace QuestFlow.QuestEngine
{
    [CustomNodeGraphEditor ( typeof ( Quest ) )]
    public class QuestEditor : NodeGraphEditor
    {
        GUIStyle style = new GUIStyle ( GUI.skin.label );

        public override void OnGUI ()
        {
            base.OnGUI ();
            style.fontSize = 18;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;
            GUILayout.FlexibleSpace ();
            EditorGUILayout.LabelField ( target.name, style, GUILayout.Height ( 20 ) );
            EditorGUILayout.LabelField ( (target as Quest).questName, style, GUILayout.Height ( 20 ) );

            if (GUILayout.Button ( "Open" ))
            {
                string path = EditorUtility.OpenFilePanel ( "Open Quest", AssetDatabase.GetAssetPath ( target ).Replace ( target.name + ".asset", "" ), "asset" ).Replace ( Application.dataPath, "Assets" );

                Quest quest = AssetDatabase.LoadAssetAtPath<Quest> ( path );

                if (quest != null)
                {
                    NodeEditorWindow.Open ( quest );
                }
            }
        }

        public override void AddContextMenuItems (GenericMenu menu)
        {
            Type[] types = new Type[3] { typeof ( State ), typeof ( Success ), typeof ( Failed ) };

            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition ( Event.current.mousePosition );

            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];

                menu.AddItem ( new GUIContent ( type.Name ), false, () => {
                    XNode.Node node = CreateNode ( type, pos );
                    NodeEditorWindow.current.AutoConnect ( node );
                } );
            }
            menu.AddSeparator ( "" );
            menu.AddItem ( new GUIContent ( "Preferences" ), false, () => NodeEditorReflection.OpenPreferences () );
            menu.AddCustomContextMenuItems ( target );
        }
    } 
}
