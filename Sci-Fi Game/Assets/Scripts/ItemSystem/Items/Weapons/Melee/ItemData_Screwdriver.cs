public class ItemData_Screwdriver : ItemGearWeaponMelee
{
    public ItemData_Screwdriver (int ID) : base ( ID, "Screwdriver" )
    {
        base.Name = "Screwdriver";
        base.Description = "A pointed, stabbing weapon";

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
