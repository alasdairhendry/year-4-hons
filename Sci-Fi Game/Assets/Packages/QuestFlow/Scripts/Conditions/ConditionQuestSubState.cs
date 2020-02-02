using QuestFlow.QuestEngine;

namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Quest Sub State" )]
    public class ConditionQuestSubState : Condition
    {
        public Quest quest;
        public NodeBase state;

        public override bool GetResult ()
        {
            return QuestManager.instance.GetQuestDataByID ( quest.questID ).currentNode == state;
        }
    }
}