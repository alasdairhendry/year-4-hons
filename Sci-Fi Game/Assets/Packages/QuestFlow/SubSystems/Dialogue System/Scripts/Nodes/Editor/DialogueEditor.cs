using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using System.Linq;
using XNode;

namespace QuestFlow.DialogueEngine
{
    [CustomNodeEditor ( typeof ( Dialogue ) )]
    public class DialogueEditor : NodeBaseEditor
    {
        bool conditionsFoldout = false;
        bool actionsFoldout = false;
        bool outputFoldout = false;

        List<ActorData> allActorData = new List<ActorData> ();

        public override void OnCreate ()
        {
            base.OnCreate ();
            RefreshActors ();
        }

        public override void AddContextMenuItems (GenericMenu menu)
        {
            base.AddContextMenuItems ( menu );

            menu.AddItem ( new GUIContent ( "Refresh Actors" ), false, () => { RefreshActors (); } );
        }

        public override void OnHeaderGUI ()
        {
            GetHeader ( "D" );
        }

        public override void OnBodyGUI ()
        {
            serializedObject.Update ();

            Dialogue node = target as Dialogue;

            DialogueSystem.EditorGUI.DrawPorts ( target );
            DrawActor ( node );
            DialogueSystem.EditorGUI.DrawDialogue ( serializedObject );
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

        private void DrawActor (Dialogue node)
        {
            int index = allActorData.IndexOf ( node.GetActor );

            EditorGUI.BeginChangeCheck ();
            index = EditorGUILayout.Popup ( new GUIContent ( "Actor" ), index, allActorData.Select ( x => x.actorName ).ToArray () );
            if (EditorGUI.EndChangeCheck ())
            {
                node.SetActor ( allActorData[index] );
                RefreshActors ();
            }
        }

        private void RefreshActors ()
        {
            allActorData = Conversation.GetAllInstances<ActorData> ();
        }
    }
}