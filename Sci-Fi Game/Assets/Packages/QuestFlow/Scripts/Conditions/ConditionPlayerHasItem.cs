using QuestFlow.QuestEngine;
using UnityEngine;

namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Player has item" )]
    public class ConditionPlayerHasItem : Condition
    {
        public enum InventoryType { Inventory, Bank, Both }
        [SerializeField] [ItemID] public int item = 0;
        [SerializeField] public int amount = 1;

        public override bool GetResult ()
        {
            return EntityManager.instance.PlayerInventory.CheckHasItemQuantity ( item, amount );
        }
    }
}