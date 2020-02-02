public class ItemData_Sword : ItemGearWeaponMelee
{
    public ItemData_Sword (int ID) : base ( ID, "Sword" )
    {
        base.Name = "Sword";
        base.Description = "A double-edged, sharp blade";

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
