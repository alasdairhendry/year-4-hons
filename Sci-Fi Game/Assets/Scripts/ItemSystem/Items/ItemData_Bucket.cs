public class ItemData_Bucket : ItemBaseData
{
    public ItemData_Bucket (int ID) : base ( ID )
    {
        base.Name = "Bucket";
        base.Description = "A wooden container.";
        base.category = ItemCategory.Tool;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 10;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
