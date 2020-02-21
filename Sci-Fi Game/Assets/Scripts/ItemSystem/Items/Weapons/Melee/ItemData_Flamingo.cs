public class ItemData_Flamingo : ItemGearWeaponMelee
{
    public ItemData_Flamingo (int ID) : base ( ID, "Flamingo" )
    {
        base.Name = "Flamingo";
        base.Description = "A blunt-force melee weapon";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 300;
        base.FetchSprite ();
    }
}
