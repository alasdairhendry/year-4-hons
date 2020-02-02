using QuestFlow.QuestEngine;

namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Quest Can Be Offered" )]
    public class ConditionCanBeOffered : Condition
    {
        public Quest quest;

        public override bool GetResult ()
        {
            return QuestManager.instance.QuestCanBeOffered ( quest );
        }
    }
}