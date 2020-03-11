using UnityEngine;

public abstract class ItemGearEquipable : ItemGear
{
    public GearData gearData;

    public ItemGearEquipable (int ID, string gearDataResourceName) : base (ID)
    {
        AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Equip,
           (inventoryIndex) =>
           {
               EquipItem ();
           } ) );

        defaultInteractionData = InventoryInteractionData.InteractType.Equip;

        this.category = ItemCategory.Gear;
        this.gearData = Resources.Load<GearData> ( gearDataResourceName );
    }
}