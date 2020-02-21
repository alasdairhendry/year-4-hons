using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionRemoveItemFromInvOrGear : Action
    {
        [SerializeField] [ItemID] public int itemID = 0;

        public override void DoAction ()
        {
            if (EntityManager.instance.PlayerInventory.CheckHasItem ( itemID ))
                EntityManager.instance.PlayerInventory.RemoveItem ( itemID );
            else if (EntityManager.instance.PlayerCharacter.cGear.WeaponSlotID == itemID)
            {
                EntityManager.instance.PlayerCharacter.cWeapon.SetHolsterState ( true );
                EntityManager.instance.PlayerCharacter.cGear.SetWeaponIndexNull ();
                EntityManager.instance.PlayerCharacter.cWeapon.Unequip ( false, true );
            }
        }
    }
}
