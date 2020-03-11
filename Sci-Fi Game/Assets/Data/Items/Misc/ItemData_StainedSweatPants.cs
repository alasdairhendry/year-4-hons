public class ItemData_StainedSweatPants : ItemBaseData
{
    public ItemData_StainedSweatPants (int ID) : base ( ID )
    {
        base.Name = "Stained Sweatpants";
        base.Description = "I wonder when these were last washed...";
        base.category = ItemCategory.Misc;

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 0;
        base.FetchSprite ();
    }
}
