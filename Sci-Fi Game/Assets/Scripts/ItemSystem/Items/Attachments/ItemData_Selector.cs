public class ItemData_Selector : ItemGearAttachment
{
    public ItemData_Selector (int ID) : base ( ID )
    {
        base.Name = "Selector";
        base.Description = "Allows any fire mode to be selected";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 200;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
