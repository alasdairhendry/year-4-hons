namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Togglable" )]
    public class ConditionTogglable : Condition
    {
        public bool state;

        public override bool GetResult ()
        {
            return state;
        }
    }
}