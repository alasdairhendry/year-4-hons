using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionSetQuestInteractableState : Action
    {
        [SerializeField] public QuestActionHelper.QuestInteractable.InteractableID id;
        [SerializeField] public bool state;

        public override void DoAction ()
        {
            QuestActionHelper.instance.SetQuestInteractableState ( id, state );
        }
    }
}
