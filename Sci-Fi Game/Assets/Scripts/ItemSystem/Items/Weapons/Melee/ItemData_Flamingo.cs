public class ItemData_Flamingo : ItemGearWeaponMelee
{
    public ItemData_Flamingo (int ID) : base ( ID, "Flamingo" )
    {
        base.Name = "Flamingo";
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
