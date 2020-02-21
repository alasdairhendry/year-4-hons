public class ItemData_Silencer : ItemGearAttachment
{
    public ItemData_Silencer (int ID) : base ( ID )
    {
        base.Name = "Silencer";
        base.Description = "Reduces noise and bullet range";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
