public enum WeaponType { Melee, Gun }

public abstract class ItemGearWeapon : ItemGear
{
    public WeaponType WeaponType { get; protected set; } = WeaponType.Gun;

    public ItemGearWeapon (int ID) : base (ID)
    {
        AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Equip,
           (inventoryIndex) =>
           {
               EquipItem ();
           } ) );

        defaultInteractionData = InventoryInteractionData.InteractType.Equip;

        this.category = ItemCategory.Weapon;
        this.gearSlot = GearSlot.Weapon;
    }
}