public class ItemData_Chisel : ItemBaseData
{
    public ItemData_Chisel (int ID) : base ( ID )
    {
        base.Name = "Chisel";
        base.Description = "A tool used to break things";
        base.category = ItemCategory.Tool;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;
        base.category = ItemCategory.Tool;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 25;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
