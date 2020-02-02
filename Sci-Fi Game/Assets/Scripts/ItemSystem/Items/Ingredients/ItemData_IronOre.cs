public class ItemData_IronOre : ItemBaseData
{
    public ItemData_IronOre (int ID) : base ( ID )
    {
        base.Name = "Iron Ore";
        base.Description = "Unrefined iron fragments";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 40;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
