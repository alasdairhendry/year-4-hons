public class ItemData_HammerWeaponBroken : ItemGearBroken
{
    public ItemData_HammerWeaponBroken (int ID) : base ( ID )
    {
        base.Name = "Hammer (Broken)";
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
