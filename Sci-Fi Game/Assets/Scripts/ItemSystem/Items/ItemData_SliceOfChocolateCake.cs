public class ItemData_SliceOfChocolateCake : ItemBaseData
{
    public ItemData_SliceOfChocolateCake (int ID) : base ( ID )
    {
        base.Name = "Slice Of Chocolate Cake";
        base.Description = "Mmm.. A slice of the chocolate cake!";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 100;
        base.BuyPrice = 350;
        base.FetchSprite ();
    }
}
