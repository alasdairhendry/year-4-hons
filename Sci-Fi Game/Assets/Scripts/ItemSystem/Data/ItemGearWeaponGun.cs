using UnityEngine;

public abstract class ItemGearWeaponGun : ItemGearWeapon
{
    public WeaponData weaponData;

    public ItemGearWeaponGun (int ID, string weaponDataResourceName) : base (ID)
    {
        base.WeaponType = WeaponType.Gun;
        this.weaponData = Resources.Load<WeaponData> ( weaponDataResourceName );
    }
}