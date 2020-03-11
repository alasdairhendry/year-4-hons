using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_HockeyMask : ItemGearEquipable
{
    public ItemData_HockeyMask (int ID) : base ( ID, "HockeyMask" )
    {
        base.Name = "Hockey Mask";
        base.Description = "This looks familiar, somehow..";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
