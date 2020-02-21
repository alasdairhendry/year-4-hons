public class ItemData_PanBroken : ItemGearBroken
{
    public ItemData_PanBroken (int ID) : base ( ID )
    {
        base.Name = "Pan (Broken)";
        base.Description = "A blunt-force melee weapon";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
