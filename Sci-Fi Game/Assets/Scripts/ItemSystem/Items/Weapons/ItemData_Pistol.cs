public class ItemData_Pistol : ItemGearWeapon
{
    public ItemData_Pistol (int ID) : base ( ID, "Pistol" )
    {
        base.Name = "Pistol";
        base.Description = "A single-firegun that uses bullets";

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
