using UnityEngine;

namespace QuestFlow
{
    [System.Serializable]
    public abstract class Condition : ScriptableObject
    {
        public virtual void OnEnter () { }
        public virtual void OnExit () { }
        public abstract bool GetResult ();
    }
}