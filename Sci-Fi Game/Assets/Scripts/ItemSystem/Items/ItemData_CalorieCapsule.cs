public class ItemData_CalorieCapsule : ItemBaseData
{
    public ItemData_CalorieCapsule (int ID) : base ( ID )
    {
        base.Name = "Calorie Capsule";
        base.Description = "Provides a moderate amount of health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 10;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 15;
        base.BuyPrice = 75;
        base.FetchSprite ();
    }
}
