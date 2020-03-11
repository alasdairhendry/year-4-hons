public class ItemData_WrenchBroken : ItemGearBroken
{
    public ItemData_WrenchBroken (int ID) : base ( ID )
    {
        base.Name = "Wrench (Broken)";
        base.Description = "A blunt-force melee weapon";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
