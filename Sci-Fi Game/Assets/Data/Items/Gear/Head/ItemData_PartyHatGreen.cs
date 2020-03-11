using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_PartyHatGreen : ItemGearEquipable
{
    public ItemData_PartyHatGreen (int ID) : base ( ID, "PartyHatGreen" )
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
