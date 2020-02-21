public class ItemData_HammerWeapon : ItemGearWeaponMelee
{
    public ItemData_HammerWeapon (int ID) : base ( ID, "Hammer" )
    {
        base.Name = "Hammer";
        base.Description = "A blunt-force melee weapon";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 300;
        base.FetchSprite ();
    }
}
