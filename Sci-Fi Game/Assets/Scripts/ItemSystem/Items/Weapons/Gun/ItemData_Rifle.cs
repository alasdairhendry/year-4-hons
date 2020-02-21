public class ItemData_Rifle : ItemGearWeaponGun
{
    public ItemData_Rifle (int ID) : base ( ID, "Rifle" )
    {
        base.Name = "Rifle";
        base.Description = "A fully automatic gun that uses bullets";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 1000;
        base.FetchSprite ();
    }
}
