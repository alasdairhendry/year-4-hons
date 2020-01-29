public class ItemData_Coins : ItemBaseData
{
    public ItemData_Coins (int ID) : base ( ID )
    {
        base.Name = "Coins";
        base.Description = "Yummy money";
        base.category = ItemCategory.Money;

        base.IsSellable = false;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 0;
        base.BuyPrice = 0;
        base.FetchSprite ();
    }
}
