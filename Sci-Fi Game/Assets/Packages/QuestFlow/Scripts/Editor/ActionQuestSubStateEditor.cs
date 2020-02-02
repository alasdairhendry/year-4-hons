using QuestFlow;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestFlow
{
    [CustomEditor ( typeof ( ActionQuestSubState ) )]
    public class ActionQuestSubStateEditor : Editor
    {
        [SerializeField] int index = 0;

        public override void OnInspectorGUI ()
        {
            EditorGUILayout.LabelField ( "SetQuestState", EditorStyles.boldLabel );
            ActionQuestSubState action = (target as ActionQuestSubState);

            action.quest = EditorGUILayout.ObjectField ( "Quest", action.quest, typeof ( QuestEngine.Quest ), false ) as QuestEngine.Quest;

            if (action.quest == null) return;
            index = action.quest.nodes.IndexOf ( action.state );

            EditorGUI.BeginChangeCheck ();
            index = EditorGUILayout.Popup ( "State", index, action.quest.nodes.Where ( x => x != null ).Select ( x => x.name ).ToArray () );
            if (EditorGUI.EndChangeCheck ())
            {
                Debug.Log ( index );
                action.state = action.quest.nodes[index] as QuestEngine.NodeBase;
            }
            EditorUtility.SetDirty ( action );
        }
    }
}
