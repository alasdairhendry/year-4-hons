public class ItemData_Gmaul : ItemGearWeaponMelee
{
    public ItemData_Gmaul (int ID) : base ( ID, "Gmaul" )
    {
        base.Name = "Granite Maul";
        base.Description = "A heavy, blunt-force weapon";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 450;
        base.FetchSprite ();
    }
}
