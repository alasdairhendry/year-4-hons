public class ItemData_SpongeCake : ItemBaseData
{
    public ItemData_SpongeCake (int ID) : base ( ID )
    {
        base.Name = "Sponge Cake";
        base.Description = "Yummy. I wonder what I could top this with.";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 120;
        base.BuyPrice = 500;
        base.FetchSprite ();
    }
}
