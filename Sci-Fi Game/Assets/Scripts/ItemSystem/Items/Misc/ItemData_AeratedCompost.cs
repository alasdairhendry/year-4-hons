public class ItemData_AeratedCompost : ItemBaseData
{
    public ItemData_AeratedCompost (int ID) : base (ID)
    {
        base.Name = "Aerated Compost";
        base.Description = "Used to cultivated flowers and plants";
        base.category = ItemCategory.Misc;

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
