public class ItemData_SwordBroken : ItemGearBroken
{
    public ItemData_SwordBroken (int ID) : base ( ID )
    {
        base.Name = "Sword (Broken)";
        base.Description = "A double-edged, sharp blade";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
