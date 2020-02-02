public class ItemData_Saber : ItemGearWeaponMelee
{
    public ItemData_Saber (int ID) : base ( ID, "Saber" )
    {
        base.Name = "Saber";
        base.Description = "An energy infused, pole saber";

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
