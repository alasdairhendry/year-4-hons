public class ItemData_ChocolateSpongeCake : ItemBaseData
{
    public ItemData_ChocolateSpongeCake (int ID) : base ( ID )
    {
        base.Name = "Chocolate Sponge Cake";
        base.Description = "Yummy, yummy.";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 200;
        base.BuyPrice = 750;
        base.FetchSprite ();
    }
}
