using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_EmporersMask : ItemGearEquipable
{
    public ItemData_EmporersMask (int ID) : base ( ID, "EmporersMask" )
    {
        base.Name = "Emporer Mask";
        base.Description = "I probably shouldn't wear this in public";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
