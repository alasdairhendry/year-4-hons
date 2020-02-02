using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace QuestFlow.DialogueEngine
{
    [CustomNodeEditor ( typeof ( Start ) )]
    public class StartEditor : NodeBaseEditor
    {
        bool conditionsFoldout = false;
        bool outputFoldout = false;
        Vector2 conditionsScrollPos = new Vector2 ();
        Vector2 outputsScrollPos = new Vector2 ();

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
            GetHeader ( "S" );
        }

        public override void OnBodyGUI ()
        {
            serializedObject.Update ();

            Start node = target as Start;

            DialogueSystem.EditorGUI.DrawPorts ( target );
            DrawActor ( node );
            DialogueSystem.EditorGUI.DrawOutputs ( target, serializedObject );
        }

        public override Color GetTint ()
        {
            Color colour = base.GetTint ();
            NodePort port = target.GetOutputPort ( "output" );

            if (port.ConnectionCount <= 0 || (target.graph as Conversation).defaultActor == null)
            {
                ColorUtility.TryParseHtmlString ( "#EE847D", out colour );
            }

            return colour;
        }

        private void DrawActor (Start node)
        {
            int index = allActorData.IndexOf ( (node.graph as Conversation).defaultActor );

            EditorGUI.BeginChangeCheck ();
            index = EditorGUILayout.Popup ( new GUIContent ( "Default Actor" ), index, allActorData.Select ( x => x.actorName ).ToArray () );
            if (EditorGUI.EndChangeCheck ())
            {
                (node.graph as Conversation).defaultActor = allActorData[index];
                RefreshActors ();
            }

            if ((node.graph as Conversation).defaultActor == null)
            {
                EditorGUILayout.HelpBox ( "Assign a default actor before continuing", MessageType.Error );
            }
        }

        private void RefreshActors ()
        {
            allActorData = Conversation.GetAllInstances<ActorData> ();
        }
    }
}