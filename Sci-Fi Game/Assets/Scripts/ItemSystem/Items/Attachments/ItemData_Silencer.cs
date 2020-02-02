public class ItemData_Silencer : ItemGearAttachment
{
    public ItemData_Silencer (int ID) : base ( ID )
    {
        base.Name = "Silencer";
        base.Description = "Reduces noise and bullet range";

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
