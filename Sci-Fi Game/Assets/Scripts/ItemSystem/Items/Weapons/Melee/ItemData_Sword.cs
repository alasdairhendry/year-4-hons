public class ItemData_Sword : ItemGearWeaponMelee
{
    public ItemData_Sword (int ID) : base ( ID, "Sword" )
    {
        base.Name = "Sword";
        base.Description = "A double-edged, sharp blade";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 300;
        base.FetchSprite ();
    }
}
