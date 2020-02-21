public class ItemData_Compensator : ItemGearAttachment
{
    public ItemData_Compensator (int ID) : base ( ID )
    {
        base.Name = "Compensator";
        base.Description = "Reduces vertical recoil, but increases damage falloff";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
