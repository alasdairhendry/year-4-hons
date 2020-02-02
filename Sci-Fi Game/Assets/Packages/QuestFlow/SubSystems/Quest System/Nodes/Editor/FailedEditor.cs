using UnityEngine;
using XNode;

namespace QuestFlow.QuestEngine
{
    [CustomNodeEditor ( typeof ( Failed ) )]
    public class FailedEditor : NodeBaseEditor
    {
        public override void OnHeaderGUI ()
        {
            base.GetHeader ( "Failed" );
        }

        public override void OnBodyGUI ()
        {
            serializedObject.Update ();

            EditorGUI.DrawPorts ( target, true, false );
            EditorGUI.DrawQuestLog ( serializedObject );
            EditorGUI.DrawActions ( serializedObject, target as NodeBase, "On Enter Actions" );
            EditorGUI.DrawRewards ( serializedObject, "State-based Rewards", "optionalReward" );
        }

        public override Color GetTint ()
        {
            Color colour = base.GetTint ();
            NodePort port = target.GetInputPort ( "input" );

            if (port.ConnectionCount <= 0)
            {
                ColorUtility.TryParseHtmlString ( "#EE847D", out colour );
            }

            return colour;
        }
    }
}