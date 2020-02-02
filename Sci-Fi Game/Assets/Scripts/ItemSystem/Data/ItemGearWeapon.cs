public enum WeaponAttackType { Melee, Gun }

public abstract class ItemGearWeapon : ItemGear
{
    public WeaponAttackType WeaponType { get; protected set; } = WeaponAttackType.Gun;

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