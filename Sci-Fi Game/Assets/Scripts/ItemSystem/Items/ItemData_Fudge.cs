public class ItemData_Fudge : ItemBaseData
{
    public ItemData_Fudge (int ID) : base ( ID )
    {
        base.Name = "Fudge";
        base.Description = "A delicious sweet";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 10;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 25;
        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
