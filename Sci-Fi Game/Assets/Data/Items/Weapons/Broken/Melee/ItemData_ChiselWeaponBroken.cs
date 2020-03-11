public class ItemData_ChiselWeaponBroken : ItemGearBroken
{
    public ItemData_ChiselWeaponBroken (int ID) : base ( ID )
    {
        base.Name = "Chisel (Broken)";
        base.Description = "A sharp melee weapon";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }
}
