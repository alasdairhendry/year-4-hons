﻿using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionRemoveItem : Action
    {
        [SerializeField] [ItemID] public int itemID = 0;
        [SerializeField] public int itemAmount = 1;

        public override void DoAction ()
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( itemID, itemAmount );
        }
    }
}
