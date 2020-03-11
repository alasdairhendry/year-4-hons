public class ItemData_Mallet : ItemGearWeaponMelee
{
    public ItemData_Mallet (int ID) : base ( ID, "Mallet" )
    {
        base.Name = "Mallet";
        base.Description = "A blunt-force melee weapon";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 300;
        base.FetchSprite ();
    }
}
