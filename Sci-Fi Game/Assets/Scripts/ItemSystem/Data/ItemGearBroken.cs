using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ItemGearBroken : ItemGear
{
    public ItemGearBroken (int ID) : base (ID)
    {
        this.category = ItemCategory.Ingredient;
    }

    protected override void EquipItem () { MessageBox.AddMessage ( "This equipment is broken and will need to be repaired before it can be equipped again.", MessageBox.Type.Error ); }
}