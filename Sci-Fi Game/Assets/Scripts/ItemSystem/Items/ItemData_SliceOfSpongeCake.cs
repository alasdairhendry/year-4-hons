public class ItemData_SliceOfSpongeCake : ItemBaseData
{
    public ItemData_SliceOfSpongeCake (int ID) : base ( ID )
    {
        base.Name = "Slice Of Sponge Cake";
        base.Description = "Mmm.. A slice of the sponge cake!";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 75;
        base.BuyPrice = 275;
        base.FetchSprite ();
    }
}
