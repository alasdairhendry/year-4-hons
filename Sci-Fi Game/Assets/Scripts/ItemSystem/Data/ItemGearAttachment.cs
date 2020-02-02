using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ItemGearAttachment : ItemGear
{
    public ItemGearAttachment (int ID) : base (ID)
    {
        AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Attach,
          (inventoryIndex) =>
          {
              EquipItem ();
          } ) );

        defaultInteractionData = InventoryInteractionData.InteractType.Attach;

        this.category = ItemCategory.Attachment;
        this.gearSlot = GearSlot.Attachment;
        this.defaultInteractionData = InventoryInteractionData.InteractType.Attach;
    }
}