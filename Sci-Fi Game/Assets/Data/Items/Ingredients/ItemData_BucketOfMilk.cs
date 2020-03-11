public class ItemData_BucketOfMilk : ItemBaseData
{
    public ItemData_BucketOfMilk (int ID) : base ( ID )
    {
        base.Name = "Dehydrated Milk";
        base.Description = "Are there cows on this planet?";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 10;
        base.FetchSprite ();
    }
}
