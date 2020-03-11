public class ItemData_ExtendedMag : ItemGearAttachment
{
    public ItemData_ExtendedMag (int ID) : base ( ID )
    {
        base.Name = "Extended Mag";
        base.Description = "Increases ammo capacity, but also increases reload speed";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
