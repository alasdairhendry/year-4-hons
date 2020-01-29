public class ItemData_UnfinishedBatter : ItemBaseData
{
    public ItemData_UnfinishedBatter (int ID) : base ( ID )
    {
        base.Name = "Unfinished Batter";
        base.Description = "I should try adding an egg to this.";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 25;
        base.BuyPrice = 150;
        base.FetchSprite ();
    }
}
