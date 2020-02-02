namespace QuestFlow.QuestEngine
{
    [System.Serializable]
    [NodeTint ( "#7DEFB0" )]
    public class Start : NodeBase
    {
        public QuestReward questReward;

        private void Reset ()
        {
            name = "Start";
        }        
    } 
}