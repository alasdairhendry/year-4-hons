public class ItemData_Rifle : ItemGearWeaponGun
{
    public ItemData_Rifle (int ID) : base ( ID, "Rifle" )
    {
        base.Name = "Rifle";
        base.Description = "A fully automatic gun that uses bullets";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 200;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
