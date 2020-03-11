using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_LuchadorMask : ItemGearEquipable
{
    public ItemData_LuchadorMask (int ID) : base ( ID, "LuchadorMask" )
    {
        base.Name = "Luchador Mask";
        base.Description = "Displays masterful wrasstling prowess when worn";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
