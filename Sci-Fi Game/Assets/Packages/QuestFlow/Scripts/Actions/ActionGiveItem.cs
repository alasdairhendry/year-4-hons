using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionGiveItem : Action
    {
        [SerializeField] public int itemID;
        [SerializeField] public int itemAmount;

        public override void DoAction ()
        {
            
        }
    }
}
