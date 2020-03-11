using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_HorseHead : ItemGearEquipable
{
    public ItemData_HorseHead (int ID) : base ( ID, "HorseHead" )
    {
        base.Name = "Horse Head";
        base.Description = "Some poor horse must have died for this";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
