public class ItemData_GroundChocolate : ItemBaseData
{
    public ItemData_GroundChocolate (int ID) : base ( ID )
    {
        base.Name = "Ground Chocolate";
        base.Description = "I couldn't eat this. Maybe I should use it on something.";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 100;
        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
