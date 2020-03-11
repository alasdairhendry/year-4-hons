using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_CapMask : ItemGearEquipable
{
    public ItemData_CapMask (int ID) : base ( ID, "CapMask" )
    {
        base.Name = "Cap Mask";
        base.Description = "A sweet, two-in-one design!";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
