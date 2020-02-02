using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace QuestFlow.QuestEngine
{
    public class EditorGUI : MonoBehaviour
    {
        public static void DrawPorts (XNode.Node target, bool displayInput, bool displayOutput)
        {
            EditorGUILayout.BeginHorizontal ();
            if (displayInput)
                NodeEditorGUILayout.PortField ( new GUIContent ( "Input" ), target.GetInputPort ( "input" ), GUILayout.MinWidth ( 0 ) );
            if (displayOutput)
                NodeEditorGUILayout.PortField ( new GUIContent ( "Output" ), target.GetOutputPort ( "output" ), GUILayout.MinWidth ( 0 ) );
            EditorGUILayout.EndHorizontal ();
        }

        public static void DrawRewards (SerializedObject serializedObject, string name, string fieldName)
        {
            EditorGUILayout.PropertyField ( serializedObject.FindProperty ( fieldName ), new GUIContent ( name ) );
        }

        public static void DrawQuestLog (SerializedObject serializedObject)
        {
            EditorGUILayout.LabelField ( "Quest Log", EditorStyles.boldLabel );
            GUILayout.Space ( -20 );
            EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "questLog" ), GUIContent.none );
        }

        public static void DrawOutputs (XNode.Node target, SerializedObject serializedObject)
        {
            NodePort outputPort = target.GetOutputPort ( "output" );

            serializedObject.FindProperty ( "outputFoldout" ).boolValue = EditorGUILayout.Foldout ( serializedObject.FindProperty ( "outputFoldout" ).boolValue, "Outputs [" + outputPort.ConnectionCount + "]", true );
            bool outputFoldout = serializedObject.FindProperty ( "outputFoldout" ).boolValue;

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

                    if (outputConnectionNode is State)
                    {
                        string label = outputConnectionNode.name;
                        label = label.Replace ( "\n", "" );
                        label = label.Substring ( 0, Mathf.Min ( 32, label.Length ) );
                        EditorGUILayout.LabelField ( string.Format ( "[{0}] {1}", "Answer", label, labelStyle ) );
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

        public static void DrawConditions (SerializedObject serializedObject, NodeBase node, string foldoutDisplay)
        {
            serializedObject.FindProperty ( "conditionsFoldout" ).boolValue = EditorGUILayout.Foldout ( serializedObject.FindProperty ( "conditionsFoldout" ).boolValue, foldoutDisplay + " [" + node.conditions.Count + "]", true );
            bool conditionsFoldout = serializedObject.FindProperty ( "conditionsFoldout" ).boolValue;

            if (conditionsFoldout)
            {
                EditorGUILayout.BeginHorizontal ();

                if (GUILayout.Button ( "Add Condition", EditorStyles.miniButtonLeft ))
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
                        string name = type.Name;

                        if (type.IsDefined ( typeof ( GenericDisplayNameAttribute ), false ))
                        {
                            GenericDisplayNameAttribute attr = type.GetCustomAttribute<GenericDisplayNameAttribute> ();
                            name = attr.displayName;
                        }

                        menu.AddItem ( new GUIContent ( name ), false, () => { AddCondition ( type, node ); } );
                    }


                    menu.ShowAsContext ();
                }

                if (GUILayout.Button ( "Delete All", EditorStyles.miniButtonRight ))
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

                EditorGUILayout.EndHorizontal ();

                node.conditionRequirementType = (ConditionRequirement)EditorGUILayout.EnumPopup ( "Required", node.conditionRequirementType );

                for (int i = 0; i < node.conditions.Count; i++)
                {
                    if (node.conditions[i] == null)
                    {
                        node.conditions.RemoveAt ( i );
                        continue;
                    }

                    EditorGUILayout.BeginVertical ( "Box" );
                    EditorGUILayout.PropertyField ( serializedObject.FindProperty ( "conditions" ).GetArrayElementAtIndex ( i ) );

                    if (GUILayout.Button ( "Delete Condition" ))
                    {
                        AssetDatabase.RemoveObjectFromAsset ( node.conditions[i] );
                        AssetDatabase.SaveAssets ();
                        node.conditions.RemoveAt ( i );
                    }
                    EditorGUILayout.EndVertical ();
                }

                EditorGUILayout.Space ();
            }
        }

        public static void DrawActions (SerializedObject serializedObject, NodeBase node, string foldoutDisplay)
        {
            serializedObject.FindProperty ( "actionsFoldout" ).boolValue = EditorGUILayout.Foldout ( serializedObject.FindProperty ( "actionsFoldout" ).boolValue, foldoutDisplay + " [" + node.actions.Count + "]", true );
            bool actionsFoldout = serializedObject.FindProperty ( "actionsFoldout" ).boolValue;

            if (actionsFoldout)
            {
                EditorGUILayout.BeginHorizontal ();

                if (GUILayout.Button ( "Add Action" , EditorStyles.miniButtonLeft ))
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
                        string name = type.Name;

                        if (type.IsDefined ( typeof ( GenericDisplayNameAttribute ), false ))
                        {
                            GenericDisplayNameAttribute attr = type.GetCustomAttribute<GenericDisplayNameAttribute> ();
                            name = attr.displayName;
                        }

                        menu.AddItem ( new GUIContent ( name ), false, () => { AddAction ( type, node ); } );
                    }

                    menu.ShowAsContext ();
                }

                if (GUILayout.Button ( "Delete All", EditorStyles.miniButtonRight ))
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

                EditorGUILayout.EndHorizontal ();

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
            }
        }

        private static void AddCondition (Type type, NodeBase node)
        {
            Condition asset = ScriptableObject.CreateInstance ( type ) as Condition;
     
            node.conditions.Add ( asset );
            asset.name = type.ToString ();

            AssetDatabase.AddObjectToAsset ( asset, node );
            AssetDatabase.SaveAssets ();

            Selection.activeObject = asset;
        }

        private static void AddAction (Type type, NodeBase node)
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
