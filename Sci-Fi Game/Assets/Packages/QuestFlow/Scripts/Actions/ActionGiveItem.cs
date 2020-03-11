using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionGiveItem : Action
    {
        [SerializeField] [ItemID] public int itemID = 0;
        [SerializeField] public int itemAmount = 1;

        public override void DoAction ()
        {
            if (EntityManager.instance.PlayerInventory.CheckCanRecieveItem ( itemID, itemAmount ))
                EntityManager.instance.PlayerInventory.AddItem ( itemID, itemAmount );
            else
            {
                EntityManager.instance.PlayerBankInventory.AddItem ( itemID, itemAmount );
                MessageBox.AddMessage ( "Some items were sent to your bank.", MessageBox.Type.Warning );
            }
        }
    }
}
