public class ItemData_Pistol : ItemGearWeaponGun
{
    public ItemData_Pistol (int ID) : base ( ID, "Pistol" )
    {
        base.Name = "Pistol";
        base.Description = "A single-firegun that uses bullets";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        base.FetchSprite ();
    }
}
