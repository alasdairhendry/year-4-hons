using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_AlienMask : ItemGearEquipable
{
    public ItemData_AlienMask (int ID) : base ( ID, "AlienMask" )
    {
        base.Name = "Alien Mask";
        base.Description = "I might get lab-tested if I wear this";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
