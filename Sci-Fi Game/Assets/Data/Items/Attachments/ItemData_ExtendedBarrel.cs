public class ItemData_ExtendedBarrel : ItemGearAttachment
{
    public ItemData_ExtendedBarrel (int ID) : base ( ID )
    {
        base.Name = "Extended Barrel";
        base.Description = "Decreases damage falloff, but increases vertical recoil";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
