public class ItemData_Compensator : ItemGearAttachment
{
    public ItemData_Compensator (int ID) : base ( ID )
    {
        base.Name = "Compensator";
        base.Description = "Reduces vertical recoil, but increases damage falloff";

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
