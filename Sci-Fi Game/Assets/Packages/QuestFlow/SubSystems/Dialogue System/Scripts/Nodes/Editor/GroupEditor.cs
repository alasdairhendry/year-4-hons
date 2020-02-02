using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace QuestFlow.DialogueEngine
{
    [CustomNodeEditor ( typeof ( Group ) )]
    public class GroupEditor : NodeBaseEditor
    {
        bool conditionsFoldout = false;
        bool actionsFoldout = false;
        bool outputFoldout = false;

        public override void OnHeaderGUI ()
        {
            GetHeader ( "G" );
        }

        public override void OnBodyGUI ()
        {
            serializedObject.Update ();

            Group node = target as Group;

            DialogueSystem.EditorGUI.DrawPorts ( target );
            DialogueSystem.EditorGUI.DrawConditions ( serializedObject, target as DialogueEntry );
            DialogueSystem.EditorGUI.DrawActions ( serializedObject, target as DialogueEntry );
            DialogueSystem.EditorGUI.DrawOutputs ( target, serializedObject );

        }

        public override Color GetTint ()
        {
            Color colour = base.GetTint ();
            NodePort port = target.GetOutputPort ( "output" );

            if (port.ConnectionCount <= 0)
            {
                ColorUtility.TryParseHtmlString ( "#EE847D", out colour );
            }

            return colour;
        }

        private void DrawPorts (Group node)
        {
            EditorGUILayout.BeginHorizontal ();
            NodeEditorGUILayout.PortField ( new GUIContent ( "Input" ), target.GetInputPort ( "input" ), GUILayout.MinWidth ( 0 ) );
            NodeEditorGUILayout.PortField ( new GUIContent ( "Output" ), target.GetOutputPort ( "output" ), GUILayout.MinWidth ( 0 ) );
            EditorGUILayout.EndHorizontal ();
        }

        private void DrawConditions (Group node)
        {
            serializedObject.FindProperty ( "conditionsFoldout" ).boolValue = EditorGUILayout.Foldout ( serializedObject.FindProperty ( "conditionsFoldout" ).boolValue, "Conditions [" + node.conditions.Count + "]", true );
            conditionsFoldout = serializedObject.FindProperty ( "conditionsFoldout" ).boolValue;

            if (conditionsFoldout)
            {
                node.conditionRequirementType = (DialogueEntry.ConditionRequirementType)EditorGUILayout.EnumPopup ( "Required", node.conditionRequirementType );

                for (int i = 0; i < node.conditions.Count; i++)
                {
                    if (node.conditions[i] == null)
                    {
                        node.conditions.RemoveAt ( i );
                        continue;
                    }

                    EditorGUILayout.BeginVertical ( "Box" );
                    EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "conditions" ).GetArrayElementAtIndex ( i ) );

                    if (serializedObject.FindProperty ( "conditions" ).GetArrayElementAtIndex ( i ).isExpanded)
                    {
                        if (GUILayout.Button ( "Delete Condition" ))
                        {
                            AssetDatabase.RemoveObjectFromAsset ( node.conditions[i] );
                            AssetDatabase.SaveAssets ();
                            node.conditions.RemoveAt ( i );
                        }
                    }
                    EditorGUILayout.EndVertical ();
                }

                EditorGUILayout.Space ();

                if (GUILayout.Button ( "Add Condition" ))
                {
                    GenericMenu menu = new GenericMenu ();

                    Type parentType = typeof ( Condition );
                    AppDomain appDomain = AppDomain.CurrentDomain;
                    Assembly[] assemblies = appDomain.GetAssemblies ();
                    List<Type> types = new List<Type> ();

                    foreach (Assembly assembly in assemblies)
                    {
                        types.AddRange ( assembly.GetTypes () );
                    }

                    IEnumerable<Type> subclasses = types.Where ( t => t.IsSubclassOf ( parentType ) );

                    foreach (Type type in subclasses)
                    {
                        menu.AddItem ( new GUIContent ( type.Name ), false, () => { AddCondition ( type, node ); } );
                    }

                    menu.ShowAsContext ();
                }

                if (GUILayout.Button ( "Delete All Conditions" ))
                {
                    for (int i = 0; i < node.conditions.Count; i++)
                    {
                        if (node.conditions[i] == null)
                        {
                            node.conditions.RemoveAt ( i );
                            continue;
                        }

                        AssetDatabase.RemoveObjectFromAsset ( node.conditions[i] );
                        AssetDatabase.SaveAssets ();
                        node.conditions.RemoveAt ( i );
                    }
                }
            }
        }

        private void DrawActions (Group node)
        {
            serializedObject.FindProperty ( "actionsFoldout" ).boolValue = EditorGUILayout.Foldout ( serializedObject.FindProperty ( "actionsFoldout" ).boolValue, "Actions [" + node.actions.Count + "]", true );
            actionsFoldout = serializedObject.FindProperty ( "actionsFoldout" ).boolValue;

            if (actionsFoldout)
            {
                for (int i = 0; i < node.actions.Count; i++)
                {
                    if (node.actions[i] == null)
                    {
                        node.actions.RemoveAt ( i );
                        continue;
                    }

                    EditorGUILayout.BeginVertical ( "Box" );
                    EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "actions" ).GetArrayElementAtIndex ( i ) );

                    if (GUILayout.Button ( "Delete Action" ))
                    {
                        AssetDatabase.RemoveObjectFromAsset ( node.actions[i] );
                        AssetDatabase.SaveAssets ();
                        node.actions.RemoveAt ( i );
                    }

                    EditorGUILayout.EndVertical ();
                }

                EditorGUILayout.Space ();

                if (GUILayout.Button ( "Add Action" ))
                {
                    GenericMenu menu = new GenericMenu ();

                    Type parentType = typeof ( Action );
                    AppDomain appDomain = AppDomain.CurrentDomain;
                    Assembly[] assemblies = appDomain.GetAssemblies ();
                    List<Type> types = new List<Type> ();

                    foreach (Assembly assembly in assemblies)
                    {
                        types.AddRange ( assembly.GetTypes () );
                    }

                    IEnumerable<Type> subclasses = types.Where ( t => t.IsSubclassOf ( parentType ) );

                    foreach (Type type in subclasses)
                    {
                        menu.AddItem ( new GUIContent ( type.Name ), false, () => { AddAction ( type, node ); } );
                    }

                    menu.ShowAsContext ();
                }

                if (GUILayout.Button ( "Delete All Actions" ))
                {
                    for (int i = 0; i < node.actions.Count; i++)
                    {
                        if (node.actions[i] == null)
                        {
                            node.actions.RemoveAt ( i );
                            continue;
                        }

                        AssetDatabase.RemoveObjectFromAsset ( node.actions[i] );
                        AssetDatabase.SaveAssets ();
                        node.actions.RemoveAt ( i );
                    }
                }
            }
        }

        private void DrawOutputs (Group node)
        {
            NodePort outputPort = target.GetOutputPort ( "output" );

            serializedObject.FindProperty ( "outputFoldout" ).boolValue = EditorGUILayout.Foldout ( serializedObject.FindProperty ( "outputFoldout" ).boolValue, "Outputs [" + outputPort.ConnectionCount + "]", true );
            outputFoldout = serializedObject.FindProperty ( "outputFoldout" ).boolValue;

            GUIStyle labelStyle = new GUIStyle ( EditorStyles.label );
            GUIStyle buttonStyle = new GUIStyle ( EditorStyles.miniButton );
            labelStyle.wordWrap = true;

            if (outputFoldout)
            {

                for (int i = 0; i < outputPort.ConnectionCount; i++)
                {
                    EditorGUILayout.BeginHorizontal ();
                    NodePort outputConnectionPort = outputPort.GetConnection ( i );
                    Node outputConnectionNode = outputPort.GetConnection ( i ).node;

                    if (outputConnectionNode is Answer)
                    {
                        string label = (outputConnectionNode as Answer).dialogue;
                        label = label.Replace ( "\n", "" );
                        label = label.Substring ( 0, Mathf.Min ( 32, label.Length ) );
                        EditorGUILayout.LabelField ( string.Format ( "[{0}] {1}", "Answer", label, labelStyle ) );
                    }
                    else if (outputConnectionNode is Dialogue)
                    {
                        string label = (outputConnectionNode as Dialogue).dialogue;
                        label = label.Replace ( "\n", "" );
                        label = label.Substring ( 0, Mathf.Min ( 32, label.Length ) );
                        EditorGUILayout.LabelField ( string.Format ( "[{0}] {1}", "Dialogue", label, labelStyle ) );
                    }
                    else if (outputConnectionNode is Event)
                    {
                        string label = (outputConnectionNode).GetType ().ToString ();
                        label = label.Substring ( 0, Mathf.Min ( 32, label.Length ) );
                        EditorGUILayout.LabelField ( string.Format ( "[{0}] {1}", "Event", label ), labelStyle );
                    }

                    if (GUILayout.Button ( "^", GUILayout.MaxHeight ( 20 ), GUILayout.MaxWidth ( 20 ) ))
                    {
                        if (i <= 0) return;

                        outputPort.SwitchConnections ( i, i - 1 );
                    }

                    if (GUILayout.Button ( "v", GUILayout.MaxHeight ( 20 ), GUILayout.MaxWidth ( 20 ) ))
                    {
                        if (i >= outputPort.ConnectionCount - 1)
                            return;

                        outputPort.SwitchConnections ( i, i + 1 );
                    }

                    EditorGUILayout.EndHorizontal ();
                }
            }
        }

        private void AddCondition (Type type, Group node)
        {
            Condition asset = ScriptableObject.CreateInstance ( type ) as Condition;
            node.conditions.Add ( asset );
            asset.name = type.ToString ();
            AssetDatabase.AddObjectToAsset ( asset, node );
            AssetDatabase.SaveAssets ();

            Selection.activeObject = asset;
        }

        private void AddAction (Type type, Group node)
        {
            Action asset = ScriptableObject.CreateInstance ( type ) as Action;
            node.actions.Add ( asset );
            asset.name = type.ToString ();
            AssetDatabase.AddObjectToAsset ( asset, node );
            AssetDatabase.SaveAssets ();

            Selection.activeObject = asset;
        }
    }
}