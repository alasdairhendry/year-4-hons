public class ItemData_Phaser : ItemGearWeaponGun
{
    public ItemData_Phaser (int ID) : base ( ID, "Phaser" )
    {
        base.Name = "Phaser";
        base.Description = "A single-fire pistol gun that uses plasma";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 750;
        base.FetchSprite ();
    }
}
