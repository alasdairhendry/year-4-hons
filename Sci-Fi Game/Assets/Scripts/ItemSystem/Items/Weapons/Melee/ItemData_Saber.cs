public class ItemData_Saber : ItemGearWeaponMelee
{
    public ItemData_Saber (int ID) : base ( ID, "Saber" )
    {
        base.Name = "Saber";
        base.Description = "An energy infused, pole saber";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 400;
        base.FetchSprite ();
    }
}
