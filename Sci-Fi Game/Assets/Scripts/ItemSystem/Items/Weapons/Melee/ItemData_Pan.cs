public class ItemData_Pan : ItemGearWeaponMelee
{
    public ItemData_Pan (int ID) : base ( ID, "Pan" )
    {
        base.Name = "Pan";
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
