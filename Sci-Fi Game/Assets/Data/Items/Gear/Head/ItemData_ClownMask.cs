using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_ClownMask : ItemGearEquipable
{
    public ItemData_ClownMask (int ID) : base ( ID, "ClownMask" )
    {
        base.Name = "Clown Mask";
        base.Description = "Maybe now, people will find me funny";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
