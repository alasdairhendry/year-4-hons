public class ItemData_Charcoal : ItemBaseData
{
    public ItemData_Charcoal (int ID) : base ( ID )
    {
        base.Name = "Charcoal";
        base.Description = "Remnants of burnt wood";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 5;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
