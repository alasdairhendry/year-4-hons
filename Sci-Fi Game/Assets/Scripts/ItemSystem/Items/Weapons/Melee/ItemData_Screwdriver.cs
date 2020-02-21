public class ItemData_Screwdriver : ItemGearWeaponMelee
{
    public ItemData_Screwdriver (int ID) : base ( ID, "Screwdriver" )
    {
        base.Name = "Screwdriver";
        base.Description = "A pointed, stabbing weapon";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
