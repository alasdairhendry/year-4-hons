public class ItemData_CakeTin : ItemBaseData
{
    public ItemData_CakeTin (int ID) : base ( ID )
    {
        base.Name = "Cake Tin";
        base.Description = "A useful baking utensil";
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
