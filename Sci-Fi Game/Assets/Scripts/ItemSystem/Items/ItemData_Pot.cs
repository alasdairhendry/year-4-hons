public class ItemData_Pot : ItemBaseData
{
    public ItemData_Pot (int ID) : base ( ID )
    {
        base.Name = "Pot";
        base.Description = "A clay container.";
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
