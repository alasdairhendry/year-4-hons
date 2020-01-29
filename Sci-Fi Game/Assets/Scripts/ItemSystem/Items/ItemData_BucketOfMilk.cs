public class ItemData_BucketOfMilk : ItemBaseData
{
    public ItemData_BucketOfMilk (int ID) : base ( ID )
    {
        base.Name = "Bucket Of Milk";
        base.Description = "Are there cows on this planet?";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 10;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
