using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ItemGearWeapon : ItemGear
{
    public WeaponData weaponData;

    public ItemGearWeapon (int ID, string weaponDataResourceName) : base (ID)
    {
        AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Equip,
           (inventoryIndex) =>
           {
               EquipItem ();
           } ) );

        defaultInteractionData = InventoryInteractionData.InteractType.Equip;

        this.category = ItemCategory.Weapon;
        this.gearSlot = GearSlot.Weapon;
        this.weaponData = Resources.Load<WeaponData> ( weaponDataResourceName );
    }
}