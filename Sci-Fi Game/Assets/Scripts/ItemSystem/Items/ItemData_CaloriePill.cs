public class ItemData_CaloriePill : ItemBaseData
{
    public ItemData_CaloriePill (int ID) : base ( ID )
    {
        base.Name = "Calorie Pill";
        base.Description = "Provides a small amount of health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 10;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 10;
        base.BuyPrice = 50;
        base.FetchSprite ();
    }
}
