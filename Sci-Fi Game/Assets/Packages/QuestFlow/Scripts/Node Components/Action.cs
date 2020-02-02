using UnityEngine;

namespace QuestFlow
{
    [System.Serializable]
    public abstract class Action : ScriptableObject
    {
        public abstract void DoAction ();
    }
}
