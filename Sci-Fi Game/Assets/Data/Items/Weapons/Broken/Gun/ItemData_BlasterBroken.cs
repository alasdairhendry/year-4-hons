public class ItemData_BlasterBroken : ItemGearBroken
{
    public ItemData_BlasterBroken (int ID) : base ( ID )
    {
        base.Name = "Blaster (Broken)";
        base.Description = "A single-firegun that uses energy";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 275;
        base.FetchSprite ();
    }
}
