using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_WelderMask : ItemGearEquipable
{
    public ItemData_WelderMask (int ID) : base ( ID, "WeldingMask" )
    {
        base.Name = "Welding Mask";
        base.Description = "It can protect my face from sparks";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
