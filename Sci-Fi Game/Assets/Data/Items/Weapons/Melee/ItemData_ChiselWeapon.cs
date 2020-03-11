public class ItemData_ChiselWeapon : ItemGearWeaponMelee
{
    public ItemData_ChiselWeapon (int ID) : base ( ID, "Chisel" )
    {
        base.Name = "Chisel";
        base.Description = "A sharp melee weapon";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 300;
        base.FetchSprite ();
    }
}
