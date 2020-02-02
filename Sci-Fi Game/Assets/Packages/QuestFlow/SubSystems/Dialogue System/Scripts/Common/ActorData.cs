using UnityEngine;

namespace QuestFlow.DialogueEngine
{
    [CreateAssetMenu ( menuName = "Quest Flow/Actor Data" )]
    public class ActorData : ScriptableObject
    {
        public string actorID;
        public string actorName;
        public Sprite sprite;
    }
}