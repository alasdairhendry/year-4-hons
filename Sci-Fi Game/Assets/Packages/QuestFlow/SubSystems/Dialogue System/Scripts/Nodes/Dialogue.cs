using UnityEngine;

namespace QuestFlow.DialogueEngine
{
    [System.Serializable]
    [NodeWidth ( 256 )]
    [NodeTint ( "#DFE88B" )]
    public class Dialogue : DialogueEntry
    {
        [TextArea(6,6)] public string dialogue = "";
        [SerializeField] private ActorData actorOverride;

        protected override void Init ()
        {
            base.Init ();

            if (actorOverride == null)
                actorOverride = (graph as Conversation).defaultActor;
        }

        public void SetActor (ActorData actorData)
        {
            actorOverride = actorData;
        }

        public ActorData GetActor
        {
            get
            {
                if (actorOverride == null)
                    return (graph as Conversation).defaultActor;
                else return actorOverride;
            }
        }
    }
}