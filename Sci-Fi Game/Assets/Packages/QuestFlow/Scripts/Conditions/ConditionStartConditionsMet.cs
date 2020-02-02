using QuestFlow.QuestEngine;

namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Quest Start Conditions Met" )]
    public class ConditionStartConditionsMet : Condition
    {
        public Quest quest;

        public override bool GetResult ()
        {
            return QuestManager.instance.QuestStartConditionsMet ( quest );
        }
    }
}