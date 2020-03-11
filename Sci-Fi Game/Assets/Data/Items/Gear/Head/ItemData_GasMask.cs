using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_GasMask : ItemGearEquipable
{
    public ItemData_GasMask (int ID) : base ( ID, "GasMask" )
    {
        base.Name = "Gas Mask";
        base.Description = "Just incase any gas leaks happen";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
