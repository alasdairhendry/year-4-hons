public class ItemData_AxleCharger : ItemGearAttachment
{
    public ItemData_AxleCharger (int ID) : base ( ID )
    {
        base.Name = "Axle Charger";
        base.Description = "Reduces weapon charge up";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
