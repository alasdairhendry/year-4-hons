public class ItemData_Mallet : ItemGearWeaponMelee
{
    public ItemData_Mallet (int ID) : base ( ID, "Mallet" )
    {
        base.Name = "Mallet";
        base.Description = "A blunt-force melee weapon";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 200;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
