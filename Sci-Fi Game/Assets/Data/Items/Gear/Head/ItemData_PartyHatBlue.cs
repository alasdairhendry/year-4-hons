using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_PartyHatBlue : ItemGearEquipable
{
    public ItemData_PartyHatBlue (int ID) : base ( ID, "PartyHatBlue" )
    {
        base.Name = "Party Hat";
        base.Description = "A nice hat from a cracker";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 10000;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
