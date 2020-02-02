using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace QuestFlow.QuestEngine
{
    [CustomNodeEditor ( typeof ( State ) )]
    public class StateEditor : NodeBaseEditor
    {
        public override void OnHeaderGUI ()
        {
            base.GetHeader ( "S" );
        }

        public override void OnBodyGUI ()
        {
            serializedObject.Update ();

            EditorGUI.DrawPorts ( target, true, true );
            EditorGUI.DrawQuestLog ( serializedObject );
            EditorGUI.DrawConditions ( serializedObject, (target as NodeBase), "Continue Conditions" );
            EditorGUI.DrawActions ( serializedObject, target as NodeBase, "On Enter Actions" );
            EditorGUI.DrawOutputs ( target, serializedObject );
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
    }
}
