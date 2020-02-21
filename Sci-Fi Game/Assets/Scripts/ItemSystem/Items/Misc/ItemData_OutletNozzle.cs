public class ItemData_OutletNozzle : ItemBaseData
{
    public ItemData_OutletNozzle (int ID) : base ( ID )
    {
        base.Name = "Outlet Nozzle";
        base.Description = "An outlet nozzle for Fred's oven.";
        base.category = ItemCategory.Misc;

        base.IsSellable = false;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { "onesmallfavour" };

        base.BuyPrice = 0;
        base.FetchSprite ();
    }
}
