using UnityEngine;
using XNode;
using XNodeEditor;

namespace QuestFlow.DialogueEngine
{
    [CustomNodeEditor ( typeof ( Answer ) )]
    public class AnswerEditor : NodeBaseEditor
    {
        bool conditionsFoldout = false;
        bool actionsFoldout = false;
        bool outputFoldout = false;

        public override void OnHeaderGUI ()
        {
            GetHeader ( "A" );
        }

        public override void OnBodyGUI ()
        {
            serializedObject.Update ();

            Answer node = target as Answer;

            DialogueSystem.EditorGUI.DrawPorts ( target );
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
    }
}