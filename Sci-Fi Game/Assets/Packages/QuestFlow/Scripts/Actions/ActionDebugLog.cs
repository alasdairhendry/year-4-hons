using UnityEngine;

namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Debug" )]
    public class ActionDebugLog : Action
    {
        public enum Type { Info, Warning, Error }
        [SerializeField] private Type type;
        [SerializeField] private string message;

        public override void DoAction ()
        {
            switch (type)
            {
                case Type.Info:
                    Debug.Log ( message );
                    break;
                case Type.Warning:
                    Debug.LogWarning ( message );
                    break;
                case Type.Error:
                    Debug.LogError ( message );
                    break;
            }
        }
    }
}