using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_PandaHead : ItemGearEquipable
{
    public ItemData_PandaHead (int ID) : base ( ID, "PandaHead" )
    {
        base.Name = "Panda Head";
        base.Description = "Some poor panda must have died for this";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
