public class ItemData_MalletBroken : ItemGearBroken
{
    public ItemData_MalletBroken (int ID) : base ( ID )
    {
        base.Name = "Mallet (Broken)";
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
