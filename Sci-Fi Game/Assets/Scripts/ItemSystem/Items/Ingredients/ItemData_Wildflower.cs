public class ItemData_Wildflower : ItemBaseData
{
    public ItemData_Wildflower (int ID) : base ( ID )
    {
        base.Name = "Wildflower";
        base.Description = "Wilder than your wildest dreams";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 30;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
