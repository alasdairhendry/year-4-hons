public class ItemData_Blaster : ItemGearWeaponGun
{
    public ItemData_Blaster (int ID) : base ( ID, "Blaster" )
    {
        base.Name = "Blaster";
        base.Description = "A single-firegun that uses energy";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        base.FetchSprite ();
    }
}
