public class ItemData_Pan : ItemGearWeaponMelee
{
    public ItemData_Pan (int ID) : base ( ID, "Pan" )
    {
        base.Name = "Pan";
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
