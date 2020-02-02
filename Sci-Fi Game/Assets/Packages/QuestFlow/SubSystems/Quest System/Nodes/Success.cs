namespace QuestFlow.QuestEngine
{
    [System.Serializable]
    [NodeTint ( "#DFE88B" )]
    public class Success : NodeQuestEnd
    {
        private void Reset ()
        {
            name = "Success";
        }
    }
}