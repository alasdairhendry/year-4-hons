public class ItemData_PlasticScraps : ItemBaseData
{
    public ItemData_PlasticScraps (int ID) : base ( ID )
    {
        base.Name = "Plastic Scraps";
        base.Description = "Scraps of old plastic";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 30;
        base.FetchSprite ();
    }
}
