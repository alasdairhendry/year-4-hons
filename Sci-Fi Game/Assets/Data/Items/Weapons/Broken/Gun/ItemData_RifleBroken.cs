public class ItemData_RifleBroken : ItemGearBroken
{
    public ItemData_RifleBroken (int ID) : base ( ID )
    {
        base.Name = "Rifle (Broken)";
        base.Description = "A fully automatic gun that uses bullets";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 400;
        base.FetchSprite ();
    }
}
