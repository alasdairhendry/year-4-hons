public class ItemData_ScrewdriverBroken : ItemGearBroken
{
    public ItemData_ScrewdriverBroken (int ID) : base ( ID )
    {
        base.Name = "Screwdriver (Broken)";
        base.Description = "A pointed, stabbing weapon";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
