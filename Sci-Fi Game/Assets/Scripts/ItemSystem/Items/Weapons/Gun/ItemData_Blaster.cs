public class ItemData_Blaster : ItemGearWeaponGun
{
    public ItemData_Blaster (int ID) : base ( ID, "Blaster" )
    {
        base.Name = "Blaster";
        base.Description = "A single-firegun that uses energy";

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
