public class ItemData_Selector : ItemGearAttachment
{
    public ItemData_Selector (int ID) : base ( ID )
    {
        base.Name = "Selector";
        base.Description = "Allows any fire mode to be selected";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
