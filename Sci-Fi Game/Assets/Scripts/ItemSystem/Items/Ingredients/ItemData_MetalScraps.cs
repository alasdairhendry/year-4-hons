public class ItemData_MetalScraps : ItemBaseData
{
    public ItemData_MetalScraps (int ID) : base ( ID )
    {
        base.Name = "Metal Scraps";
        base.Description = "Scraps of old metal";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 25;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
