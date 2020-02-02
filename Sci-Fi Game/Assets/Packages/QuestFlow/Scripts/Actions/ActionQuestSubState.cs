using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionQuestSubState : Action
    {
        [SerializeField] public Quest quest;
        [SerializeField] public NodeBase state;

        public override void DoAction ()
        {
            QuestManager.instance.SetQuestSubstate ( quest, state );
        }
    }
}
