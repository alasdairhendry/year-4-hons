using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_PaperBag : ItemGearEquipable
{
    public ItemData_PaperBag (int ID) : base ( ID, "PaperBag" )
    {
        base.Name = "Paper Bag";
        base.Description = "No one will ever know who I am when i'm wearing this";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
