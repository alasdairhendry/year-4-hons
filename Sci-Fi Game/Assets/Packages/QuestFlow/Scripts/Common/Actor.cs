using QuestFlow.DialogueEngine;
using QuestFlow.QuestEngine;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class Actor : MonoBehaviour
    {
        public ActorData actor;
        public Conversation defaultConversation;
        public List<Quest> questsOffered = new List<Quest> ();

        private void Update ()
        {
            RaycastHit hit;

            if (!Input.GetMouseButtonDown ( 0 )) return;

            if (Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ), out hit ))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    StartDialogue ();
                }
            }
        }

        public void StartDialogue ()
        {
            DialogueManager.instance.StartConversation ( defaultConversation );
        }
    }
}