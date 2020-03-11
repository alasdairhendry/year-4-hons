public class ItemData_PistolBroken : ItemGearBroken
{
    public ItemData_PistolBroken (int ID) : base ( ID )
    {
        base.Name = "Pistol (Broken)";
        base.Description = "A single-firegun that uses bullets";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 275;
        base.FetchSprite ();
    }
}
