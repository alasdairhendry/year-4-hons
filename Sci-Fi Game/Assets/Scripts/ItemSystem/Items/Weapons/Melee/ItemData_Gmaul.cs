public class ItemData_Gmaul : ItemGearWeaponMelee
{
    public ItemData_Gmaul (int ID) : base ( ID, "Gmaul" )
    {
        base.Name = "Granite Maul";
        base.Description = "A heavy, blunt-force weapon";

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
