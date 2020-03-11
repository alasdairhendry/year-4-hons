using QuestFlow.QuestEngine;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Player Doesnt Have Item" )]
    public class ConditionPlayerDoesntHaveItem : Condition
    {
        public enum InventoryType { Inventory, Bank, Both }
        [SerializeField] [ItemID] public List<int> item = new List<int> ();
        //[SerializeField] public int amount = 1;

        public override bool GetResult ()
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (EntityManager.instance.PlayerInventory.CheckHasItem ( item[i] ))
                {
                    return false;
                }
            }

            return true;
        }
    }
}