
public abstract class ItemGearWeaponMelee : ItemGearWeapon
{
    //public WeaponMeleeData weaponData;

    public ItemGearWeaponMelee (int ID, string weaponDataResourceName) : base ( ID )
    {
        base.WeaponType = WeaponType.Melee;
        //this.weaponData = Resources.Load<WeaponMeleeData> ( weaponDataResourceName );
    }
}