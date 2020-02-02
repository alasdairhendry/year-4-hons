public class ItemData_Wrench : ItemGearWeaponMelee
{
    public ItemData_Wrench (int ID) : base ( ID, "Wrench" )
    {
        base.Name = "Wrench";
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
