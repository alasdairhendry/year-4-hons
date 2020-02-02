public class ItemData_Phaser : ItemGearWeapon
{
    public ItemData_Phaser (int ID) : base ( ID, "Phaser" )
    {
        base.Name = "Phaser";
        base.Description = "A single-fire pistol gun that uses plasma";

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
