using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_ChickenHead : ItemGearEquipable
{
    public ItemData_ChickenHead (int ID) : base ( ID, "ChickenHead" )
    {
        base.Name = "Chicken Head";
        base.Description = "Some poor chicken must have died for this";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
