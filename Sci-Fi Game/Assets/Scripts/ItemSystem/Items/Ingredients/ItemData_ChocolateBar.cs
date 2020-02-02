public class ItemData_ChocolateBar : ItemBaseData
{
    public ItemData_ChocolateBar (int ID) : base ( ID )
    {
        base.Name = "Chocolate Bar";
        base.Description = "Chocolate Starfish and the Hot Dog Flavored Water";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 500;
        base.BuyPrice = 3000;
        base.FetchSprite ();
    }
}
