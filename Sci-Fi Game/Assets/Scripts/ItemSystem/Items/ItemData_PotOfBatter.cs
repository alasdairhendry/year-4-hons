public class ItemData_PotOfBatter : ItemBaseData
{
    public ItemData_PotOfBatter (int ID) : base ( ID )
    {
        base.Name = "Pot of Batter";
        base.Description = "I can bake wonderful treats with this.";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 30;
        base.BuyPrice = 175;
        base.FetchSprite ();
    }
}
