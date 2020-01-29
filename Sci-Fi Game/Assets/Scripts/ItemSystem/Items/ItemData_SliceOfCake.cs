public class ItemData_SliceOfCake : ItemBaseData
{
    public ItemData_SliceOfCake (int ID) : base ( ID )
    {
        base.Name = "Slice Of Cake";
        base.Description = "Mmm.. A slice of the Emporers cake!";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 1000;
        base.BuyPrice = 6500;
        base.FetchSprite ();
    }
}
