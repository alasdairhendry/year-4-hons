public class ItemData_BlasterRifle : ItemGearWeaponGun
{
    public ItemData_BlasterRifle (int ID) : base ( ID, "Blaster Rifle" )
    {
        base.Name = "Blaster Rifle";
        base.Description = "A fully automatic gun that uses energy";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 1000;
        base.FetchSprite ();
    }
}
