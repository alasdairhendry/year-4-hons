public class ItemData_IronIngot : ItemBaseData
{
    public ItemData_IronIngot (int ID) : base ( ID )
    {
        base.Name = "Iron Ingot";
        base.Description = "A refined, poor quality metal bar";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 140;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
