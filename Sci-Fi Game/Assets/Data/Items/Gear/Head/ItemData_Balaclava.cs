using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Balaclava : ItemGearEquipable
{
    public ItemData_Balaclava (int ID) : base ( ID, "Balaclava" )
    {
        base.Name = "Balaclava";
        base.Description = "Who's the crooks in this crime?!";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
