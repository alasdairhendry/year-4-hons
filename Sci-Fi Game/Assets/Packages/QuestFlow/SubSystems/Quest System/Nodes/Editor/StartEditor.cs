using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using XNode;

namespace QuestFlow.QuestEngine
{
    [CustomNodeEditor ( typeof ( Start ) )]
    public class StartEditor : NodeBaseEditor
    {
        public override void OnHeaderGUI ()
        {
            base.GetHeader ( "Start" );
        }

        public override void OnBodyGUI ()
        {
            serializedObject.Update ();

            EditorGUI.DrawPorts ( target, false, true );
            EditorGUI.DrawQuestLog ( serializedObject );
            EditorGUI.DrawConditions ( serializedObject, target as NodeBase, "Offer Conditions" );
            EditorGUI.DrawActions ( serializedObject, target as NodeBase, "On Accepted Actions" );
            EditorGUI.DrawOutputs ( target, serializedObject );
            EditorGUI.DrawRewards ( serializedObject, "Mandatory Rewards", "questReward" );
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
