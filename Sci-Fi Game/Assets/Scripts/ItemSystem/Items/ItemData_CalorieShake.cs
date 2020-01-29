public class ItemData_CalorieShake : ItemBaseData
{
    public ItemData_CalorieShake (int ID) : base ( ID )
    {
        base.Name = "Calorie Shake";
        base.Description = "Provides a greater amount of health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 10;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 25;
        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
