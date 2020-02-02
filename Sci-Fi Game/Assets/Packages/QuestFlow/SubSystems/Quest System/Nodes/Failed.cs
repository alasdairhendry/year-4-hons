namespace QuestFlow.QuestEngine
{
    [System.Serializable]
    [NodeTint ( "#DFE88B" )]
    public class Failed : NodeQuestEnd
    {
        private void Reset ()
        {
            name = "Failed";
        }
    }
}