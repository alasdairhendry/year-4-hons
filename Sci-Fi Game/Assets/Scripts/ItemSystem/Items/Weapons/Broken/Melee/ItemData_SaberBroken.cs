public class ItemData_SaberBroken : ItemGearBroken
{
    public ItemData_SaberBroken (int ID) : base ( ID )
    {
        base.Name = "Saber (Broken)";
        base.Description = "An energy infused, pole saber";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
