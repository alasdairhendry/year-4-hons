public class ItemData_ExtendedBarrel : ItemGearAttachment
{
    public ItemData_ExtendedBarrel (int ID) : base ( ID )
    {
        base.Name = "Extended Barrel";
        base.Description = "Decreases damage falloff, but increases vertical recoil";

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
