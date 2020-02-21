public class ItemData_Knife : ItemBaseData
{
    public ItemData_Knife (int ID) : base ( ID )
    {
        base.Name = "Knife";
        base.Description = "A sharp and pointy tool";
        base.category = ItemCategory.Tool;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 25;
        base.FetchSprite ();
    }
}
