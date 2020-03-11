public class ItemData_EnergyDrink : ItemBaseData
{
    public ItemData_EnergyDrink (int ID) : base ( ID )
    {
        base.Name = "Empty Energy Drink Can";
        base.Description = "Fuels the mind for endless gaming sessions.";
        base.category = ItemCategory.Misc;

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 0;
        base.FetchSprite ();
    }
}
