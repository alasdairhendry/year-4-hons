public class ItemData_PhaserRifle : ItemGearWeapon
{
    public ItemData_PhaserRifle (int ID) : base ( ID, "Phaser Rifle" )
    {
        base.Name = "Phaser Rifle";
        base.Description = "A burst-fire automatic gun that uses plasma";

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
