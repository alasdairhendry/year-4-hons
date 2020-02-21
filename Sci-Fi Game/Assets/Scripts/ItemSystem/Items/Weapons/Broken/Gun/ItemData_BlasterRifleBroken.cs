public class ItemData_BlasterRifleBroken : ItemGearBroken
{
    public ItemData_BlasterRifleBroken (int ID) : base ( ID )
    {
        base.Name = "Blaster Rifle (Broken)";
        base.Description = "A fully automatic gun that uses energy";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 400;
        base.FetchSprite ();
    }
}
