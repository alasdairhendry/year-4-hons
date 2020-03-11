public class ItemData_MetalScraps : ItemBaseData
{
    public ItemData_MetalScraps (int ID) : base ( ID )
    {
        base.Name = "Metal Scraps";
        base.Description = "Scraps of old metal";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 25;
        base.FetchSprite ();
    }
}
