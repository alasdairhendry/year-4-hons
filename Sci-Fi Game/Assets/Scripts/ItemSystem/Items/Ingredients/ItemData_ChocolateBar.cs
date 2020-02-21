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

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 20;
        base.FetchSprite ();
    }
}
