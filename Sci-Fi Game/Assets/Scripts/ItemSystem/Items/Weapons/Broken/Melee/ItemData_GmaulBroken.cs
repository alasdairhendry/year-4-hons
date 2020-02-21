public class ItemData_GmaulBroken : ItemGearBroken
{
    public ItemData_GmaulBroken (int ID) : base ( ID )
    {
        base.Name = "Granite Maul (Broken)";
        base.Description = "A heavy, blunt-force weapon";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
