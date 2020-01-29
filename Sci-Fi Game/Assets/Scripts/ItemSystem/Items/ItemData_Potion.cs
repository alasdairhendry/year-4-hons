public class ItemData_Potion : ItemBaseData
{
    public ItemData_Potion (int ID) : base ( ID )
    {
        base.Name = "Potion";
        base.Description = "This must have come from some sort of potion seller";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 25;
        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
