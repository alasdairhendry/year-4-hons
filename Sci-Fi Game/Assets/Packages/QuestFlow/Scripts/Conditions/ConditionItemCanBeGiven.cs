using QuestFlow.QuestEngine;
using UnityEngine;

namespace QuestFlow
{
    [System.Serializable]
    [GenericDisplayName ( "Item Can Be Given" )]
    public class ConditionItemCanBeGiven : Condition
    {
        [SerializeField] [ItemID] public int item = 0;
        [SerializeField] public int amount = 1;

        public override bool GetResult ()
        {
            return EntityManager.instance.PlayerInventory.CheckCanRecieveItem ( item, amount );
        }
    }
}