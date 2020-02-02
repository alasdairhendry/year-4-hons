public class ItemData_Hammer : ItemBaseData
{
    public ItemData_Hammer (int ID) : base ( ID )
    {
        base.Name = "Hammer";
        base.Description = "A tool used to mould things";
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
