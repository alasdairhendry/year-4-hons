public class ItemData_Knife : ItemBaseData
{
    public ItemData_Knife (int ID) : base ( ID )
    {
        base.Name = "Knife";
        base.Description = "A sharp and pointy tool.";
        base.category = ItemCategory.Tool;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 500;
        base.BuyPrice = 3250;
        base.FetchSprite ();
    }
}
