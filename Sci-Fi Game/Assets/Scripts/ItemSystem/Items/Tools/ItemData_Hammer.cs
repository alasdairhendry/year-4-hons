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

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 25;
         base.FetchSprite ();
    }
}
