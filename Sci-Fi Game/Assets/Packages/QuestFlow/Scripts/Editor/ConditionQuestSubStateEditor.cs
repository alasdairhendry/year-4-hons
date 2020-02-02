using QuestFlow;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestFlow
{
    [CustomEditor ( typeof ( ConditionQuestSubState ) )]
    public class ConditionQuestSubStateEditor : Editor
    {
        [SerializeField] int index = 0;

        public override void OnInspectorGUI ()
        {
            EditorGUILayout.LabelField ( "GetQuestState", EditorStyles.boldLabel );
            ConditionQuestSubState condition = (target as ConditionQuestSubState);

            condition.quest = EditorGUILayout.ObjectField ( "Quest", condition.quest, typeof ( QuestEngine.Quest ), false ) as QuestEngine.Quest;

            if (condition.quest == null) return;
            index = condition.quest.nodes.IndexOf ( condition.state );

            EditorGUI.BeginChangeCheck ();
            index = EditorGUILayout.Popup ( "State", index, condition.quest.nodes.Where ( x => x != null ).Select ( x => x.name ).ToArray () );
            if (EditorGUI.EndChangeCheck ())
            {
                Debug.Log ( index );
                condition.state = condition.quest.nodes[index] as QuestEngine.NodeBase;
            }
            EditorUtility.SetDirty ( condition );
        }       
    } 
}
