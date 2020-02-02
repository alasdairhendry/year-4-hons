using UnityEngine;

public abstract class ItemGearWeaponGun : ItemGearWeapon
{
    public WeaponGunData weaponData;

    public ItemGearWeaponGun (int ID, string weaponDataResourceName) : base (ID)
    {
        base.WeaponType = WeaponAttackType.Gun;
        this.weaponData = Resources.Load<WeaponGunData> ( weaponDataResourceName );
    }
}