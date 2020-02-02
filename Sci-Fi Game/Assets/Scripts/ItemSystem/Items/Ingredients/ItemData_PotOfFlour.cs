public class ItemData_PotOfFlour : ItemBaseData
{
    public ItemData_PotOfFlour (int ID) : base ( ID )
    {
        base.Name = "Pot Of Flour";
        base.Description = "I wonder where they grow wheat on this planet";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 25;
        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
