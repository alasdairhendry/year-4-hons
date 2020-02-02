using UnityEngine;

public abstract class ItemGearWeaponMelee : ItemGearWeapon
{
    public WeaponMeleeData weaponData;

    public ItemGearWeaponMelee (int ID, string weaponDataResourceName) : base ( ID )
    {
        base.WeaponType = WeaponAttackType.Melee;
        this.weaponData = Resources.Load<WeaponMeleeData> ( weaponDataResourceName );
    }
}