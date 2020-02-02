public class ItemData_BlasterRifle : ItemGearWeapon
{
    public ItemData_BlasterRifle (int ID) : base ( ID, "Blaster Rifle" )
    {
        base.Name = "Blaster Rifle";
        base.Description = "A fully automatic gun that uses energy";

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
