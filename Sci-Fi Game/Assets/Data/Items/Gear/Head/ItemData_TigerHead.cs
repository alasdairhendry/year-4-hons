using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_TigerHead : ItemGearEquipable
{
    public ItemData_TigerHead (int ID) : base ( ID, "TigerHead" )
    {
        base.Name = "Tiger Head";
        base.Description = "Some poor tiger must have died for this";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
