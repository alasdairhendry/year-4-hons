public class ItemData_LED : ItemBaseData
{
    public ItemData_LED (int ID) : base ( ID )
    {
        base.Name = "LED";
        base.Description = "An LED light used for crafting weapons and armour";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 50;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
