using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionSetNPCAttackOption : Action
    {
        [SerializeField] public NPCData npcData;
        [SerializeField] public NPCAttackOption option;

        public override void DoAction ()
        {
            npcData.DefaultAttackOption = option;
        }
    }
}
