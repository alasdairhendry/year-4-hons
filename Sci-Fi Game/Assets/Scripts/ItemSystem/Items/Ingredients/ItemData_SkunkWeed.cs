public class ItemData_SkunkWeed : ItemBaseData
{
    public ItemData_SkunkWeed (int ID) : base ( ID )
    {
        base.Name = "Skunk Weed";
        base.Description = "So smelly it might make you faint";
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
