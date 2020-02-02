using QuestFlow.QuestEngine;

namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Quest State" )]
    public class ConditionQuestState : Condition
    {
        public Quest quest;
        public QuestState state;

        public override bool GetResult ()
        {
            return QuestManager.instance.GetQuestDataByID ( quest.questID ).currentQuestState == state;
        }
    }
}