public class ItemData_AxleCharger : ItemGearAttachment
{
    public ItemData_AxleCharger (int ID) : base ( ID )
    {
        base.Name = "Axle Charger";
        base.Description = "Reduces weapon charge up";

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
