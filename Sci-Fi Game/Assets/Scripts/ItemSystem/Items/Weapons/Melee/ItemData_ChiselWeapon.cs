public class ItemData_ChiselWeapon : ItemGearWeaponMelee
{
    public ItemData_ChiselWeapon (int ID) : base ( ID, "Chisel" )
    {
        base.Name = "Chisel";
        base.Description = "A sharp melee weapon";

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
