public class ItemData_FlamingoBroken : ItemGearBroken
{
    public ItemData_FlamingoBroken (int ID) : base ( ID )
    {
        base.Name = "Flamingo (Broken)";
        base.Description = "A blunt-force melee weapon";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
