public class ItemData_Lens : ItemBaseData
{
    public ItemData_Lens (int ID) : base ( ID )
    {
        base.Name = "Lens";
        base.Description = "A piece of glass that magnifies items held under it";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 40;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
